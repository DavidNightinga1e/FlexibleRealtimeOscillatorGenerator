using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Runtime.Common;
using Runtime.Synth.Presets;
using Runtime.Synth.Views;
using Runtime.Synth.Views.Presets;
using Runtime.UI.Keyboard;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;

namespace Runtime.Synth
{
	public class SynthesizerBehaviour : MonoBehaviour
	{
		#region MoveToWaveformView

		[SerializeField] private UILineRenderer _waveformRenderer;
		[SerializeField] private int _waveformRendererSampleRateDivider;
		[SerializeField] private float _waveformRendererWidth;
		[SerializeField] private float _waveformRendererHeight;

		private const int _waveformSampleCount = 400;

		private readonly float[] _waveformSamples = new float[_waveformSampleCount];
		private int _waveformWriteIndex = 0;
		private int _skipCount = 0;
		private readonly Vector2[] _waveformPositions = new Vector2[_waveformSampleCount];

		#endregion

		#region MoveToFrequencyPlotView

		[SerializeField] private UILineRenderer _frequencyPlotRenderer;

		private const int _fftSampleCount = 2048;
		private int _frequencyPlotWriteIndex = 0;
		private float[] _frequencyPlotSamples = new float[_fftSampleCount];
		private Vector2[] _frequencyPlotPositions = new Vector2[_fftSampleCount];

		#endregion

		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private Keyboard keyboard;
		[SerializeField] private EnvelopeSettingsView _ampSettingsView;
		[SerializeField] private EnvelopeSettingsView _env1SettingsView;
		[SerializeField] private EnvelopeSettingsView _env2SettingsView;
		[SerializeField] private OscillatorSettingsView _osc1SettingsView;
		[SerializeField] private OscillatorSettingsView _osc2SettingsView;
		[SerializeField] private LfoSettingsView _lfo1SettingsView;
		[SerializeField] private LfoSettingsView _lfo2SettingsView;
		[SerializeField] private FilterSettingsView _filterSettingsView;
		[SerializeField] private DistortSettingsView _distortSettingsView;
		[SerializeField] private DelaySettingsView _delaySettingsView;
		[SerializeField] private ReverbSettingsView _reverbSettingsView;
		[SerializeField] private PresetsView _presetsView;
		
		private SynthesizerPreset _preset = BuiltInPresets.CreateDefault();

		private int _sampleRate;

		private readonly Voice[] _voices = new Voice[(int)(Note.C8 + 1)];
		private DistortInstance _distortInstance;
		private DelayInstance _delayInstance;
		private ReverbInstance _reverbInstance;

		private Stopwatch _stopwatch = new();
		private double _lastElapsed;

		private void Awake()
		{
			_presetsView.OnPresetChanged += OnPresetChanged;
			
			keyboard.NoteDownEvent += OnNoteDown;
			keyboard.NoteUpEvent += OnNoteUp;
			
			_sampleRate = AudioSettings.outputSampleRate;
		}

		private void OnPresetChanged()
		{
			_preset = _presetsView.ActivePreset;
			OnPresetLoaded();
		}

		private void OnPresetLoaded()
		{
			PrepareSettings();
			
			PrepareVoices();
			PrepareEffects();			
		}

		private void OnDestroy()
		{
			keyboard.NoteDownEvent -= OnNoteDown;
			keyboard.NoteUpEvent -= OnNoteUp;
		}

		private void PrepareSettings()
		{
			_ampSettingsView.SetSettings(_preset.AmpSettings);
			_env1SettingsView.SetSettings(_preset.Env1Settings);
			_env2SettingsView.SetSettings(_preset.Env2Settings);

			_lfo1SettingsView.SetSettings(_preset.Lfo1Settings);
			_lfo2SettingsView.SetSettings(_preset.Lfo2Settings);

			_osc1SettingsView.SetSettings(_preset.Osc1Settings);
			_osc2SettingsView.SetSettings(_preset.Osc2Settings);

			_filterSettingsView.SetSettings(_preset.FilterSettings);
			
			_distortSettingsView.SetSettings(_preset.DistortSettings);
			_delaySettingsView.SetSettings(_preset.DelaySettings);
			_reverbSettingsView.SetSettings(_preset.ReverbSettings);
		}

		private void Update()
		{
			for (int i = 0; i < _waveformSampleCount; i++)
			{
				_waveformPositions[i] = new Vector2
				(
					_waveformRendererWidth * ((float)i / _waveformSampleCount),
					_waveformRendererHeight * (_waveformSamples[i] / 2 + 0.5f)
				);
			}

			_waveformRenderer.Points = _waveformPositions;

			GenerateFrequencyPlot(_frequencyPlotSamples);

			_frequencyPlotRenderer.Points = _frequencyPlotPositions;
			
			timeText.text = _lastElapsed.ToString("00.00ms");
		}

		private void OnNoteUp(Note note)
		{
			var i = (int)note;
			_voices[i].NoteUp();
		}

		private void OnNoteDown(Note note)
		{
			var i = (int)note;
			_voices[i].NoteDown();
		}

		private void PrepareVoices()
		{
			for (int i = 0; i < _voices.Length; i++)
			{
				_voices[i] = new Voice
				(
					_sampleRate,
					NoteToFrequency.GetFrequency((Note)i),
					_preset.Osc1Settings,
					_preset.Osc2Settings,
					_preset.Lfo1Settings,
					_preset.Lfo2Settings,
					_preset.FilterSettings,
					_preset.AmpSettings,
					_preset.Env1Settings,
					_preset.Env2Settings
				);
			}
		}

		private void PrepareEffects()
		{
			_distortInstance = new DistortInstance(_sampleRate, _preset.DistortSettings);
			_delayInstance = new DelayInstance(_sampleRate, _preset.DelaySettings);
			_reverbInstance = new ReverbInstance(_sampleRate, _preset.ReverbSettings);
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (_preset is null)
				return;
			
			_stopwatch.Restart();
			
			int dataLength = data.Length / channels;

			if (dataLength != _frequencyPlotSamples.Length)
				_frequencyPlotSamples = new float[dataLength];

			for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
			{
				var output = MixVoices();
				
				output = ApplyEffects(output);

				for (int channelIndex = 0; channelIndex < channels; channelIndex++)
				{
					data[dataIndex * channels + channelIndex] += (float)output;

					_skipCount++;
					if (_skipCount >= _waveformRendererSampleRateDivider)
					{
						_skipCount = 0;
						_waveformSamples[_waveformWriteIndex++] = (float)output;
						_waveformWriteIndex %= _waveformSamples.Length;
					}

					_frequencyPlotSamples[_frequencyPlotWriteIndex++] = (float)output;
					_frequencyPlotWriteIndex %= _frequencyPlotSamples.Length;
				}
			}

			_lastElapsed = (double)_stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond;
			
			_stopwatch.Stop();
		}

		private double ApplyEffects(double sample)
		{
			sample = _distortInstance.ProcessSample(sample);
			sample = _delayInstance.ProcessSample(sample);
			sample = _reverbInstance.ProcessSample(sample);
			return sample;
		}

		private double MixVoices()
		{
			double signal = 0;
			double envelopeSum = 0;

			foreach (Voice v in _voices)
			{
				if (v.IsFinished)
					continue;

				v.UpdateSample();
				signal += v.Sample;
				envelopeSum += v.AmpEnvelopeValue;
			}

			if (envelopeSum > 1)
				signal /= envelopeSum;

			if (double.IsNaN(signal))
			{
				Debug.LogError($"Mixer: signal was NaN");
				return 0;
			}

			if (Math.Abs(signal) > 1)
			{
				Debug.LogError($"Mixer: signal outside boundary {signal}");
				return Math.Clamp(signal, -1, 1);
			}

			return signal;
		}

		public void GenerateFrequencyPlot(float[] samples)
		{
			var dcRemoved = RemoveDCOffset(samples);

			// Step 2: Apply high-quality window
			var windowed = ApplyBlackmanWindow(dcRemoved);

			// Step 3: Perform FFT
			var spectrum = ComputeFFT(windowed);

			// Convert to magnitude and generate plot points
			ConvertToPlotPoints(spectrum);
		}

		private float[] RemoveDCOffset(float[] samples)
		{
			// Calculate mean (DC offset)
			float mean = samples.Average();

			// Remove DC component
			var corrected = new float[samples.Length];
			for (int i = 0; i < samples.Length; i++)
			{
				corrected[i] = samples[i] - mean;
			}

			return corrected;
		}

		private float[] ApplyHammingWindow(float[] samples)
		{
			var windowed = new float[samples.Length];
			int n = samples.Length;

			for (int i = 0; i < n; i++)
			{
				// Hamming window: 0.54 - 0.46 * cos(2πi/(n-1))
				float window = 0.54f - 0.46f * Mathf.Cos(2 * Mathf.PI * i / (n - 1));
				windowed[i] = samples[i] * window;
			}

			return windowed;
		}

		private float[] ApplyBlackmanWindow(float[] samples)
		{
			var windowed = new float[samples.Length];
			int n = samples.Length;

			for (int i = 0; i < n; i++)
			{
				// Blackman window has better frequency resolution
				float a0 = 0.42f;
				float a1 = 0.5f;
				float a2 = 0.08f;

				float window = a0
				               - a1 * Mathf.Cos(2 * Mathf.PI * i / (n - 1))
				               + a2 * Mathf.Cos(4 * Mathf.PI * i / (n - 1));

				windowed[i] = samples[i] * window;
			}

			return windowed;
		}

		private void ConvertToPlotPoints(Complex[] spectrum)
		{
			int maxUsefulBin = _frequencyPlotSamples.Length / 2;
			if (_frequencyPlotPositions.Length != maxUsefulBin)
			{
				_frequencyPlotPositions = new Vector2[maxUsefulBin];
			}

			for (int i = 1; i < maxUsefulBin; i++) // Start from 1 to skip DC offset
			{
				float magnitude = (float)spectrum[i].Magnitude;
				float dB = 20 * Mathf.Log10(magnitude + 1e-6f);

				// Simple and correct normalization
				float normalizedFreq = (float)i / maxUsefulBin;
				float logFreq = Mathf.Log10(normalizedFreq * 9 + 1);
				float normalizedAmp = Mathf.Clamp01((dB + 80) / 80);

				_frequencyPlotPositions[i] = new Vector2(
					logFreq * _waveformRendererWidth,
					normalizedAmp * _waveformRendererHeight);
			}
		}

		private Complex[] ComputeFFT(float[] samples)
		{
			// Pad or truncate to FFT size
			var input = new Complex[samples.Length];
			int length = samples.Length;

			for (int i = 0; i < length; i++)
			{
				input[i] = new Complex(samples[i], 0);
			}

			// Perform FFT
			FFT(input, true);

			return input;
		}

		private void FFT(Complex[] buffer, bool forward)
		{
			int n = buffer.Length;
			if ((n & (n - 1)) != 0) // Check if power of 2
				throw new ArgumentException("FFT buffer size must be power of 2");

			// Bit-reversal permutation
			for (int i = 1, j = 0; i < n; i++)
			{
				int bit = n >> 1;
				for (; (j & bit) != 0; bit >>= 1)
					j ^= bit;
				j ^= bit;

				if (i < j)
					(buffer[i], buffer[j]) = (buffer[j], buffer[i]);
			}

			// Iterative FFT
			for (int len = 2; len <= n; len <<= 1)
			{
				float angle = 2 * Mathf.PI / len * (forward ? -1 : 1);
				Complex wlen = new Complex(Mathf.Cos(angle), Mathf.Sin(angle));

				for (int i = 0; i < n; i += len)
				{
					Complex w = new Complex(1, 0);
					for (int j = 0; j < len / 2; j++)
					{
						Complex u = buffer[i + j];
						Complex v = buffer[i + j + len / 2] * w;

						buffer[i + j] = u + v;
						buffer[i + j + len / 2] = u - v;
						w *= wlen;
					}
				}
			}

			// Normalize for inverse FFT
			if (!forward)
			{
				for (int i = 0; i < n; i++)
					buffer[i] /= n;
			}
		}
	}
}