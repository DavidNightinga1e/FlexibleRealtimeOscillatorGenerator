using System;
using System.Linq;
using System.Numerics;
using Runtime.Synth;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Runtime.UI
{
	public class SpectrumPresenter : MonoBehaviour
	{
		[SerializeField] private UILineRenderer _frequencyPlotRenderer;
		[SerializeField] private float _rendererWidth;
		[SerializeField] private float _rendererHeight;

		private const int _fftSampleCount = 2048;
		
		private int _frequencyPlotWriteIndex = 0;
		private float[] _frequencyPlotSamples = new float[_fftSampleCount];
		private Vector2[] _frequencyPlotPositions = new Vector2[_fftSampleCount];

		private void Awake()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent += OnSampleCompleted;
		}

		private void OnSampleCompleted(double sample)
		{
			_frequencyPlotSamples[_frequencyPlotWriteIndex] = (float)sample;

			_frequencyPlotWriteIndex++;
			_frequencyPlotWriteIndex %= _fftSampleCount;
		}

		private void Update()
		{
			GenerateFrequencyPlot(_frequencyPlotSamples);

			_frequencyPlotRenderer.Points = _frequencyPlotPositions;
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
					logFreq * _rendererWidth,
					normalizedAmp * _rendererHeight);
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

		private void OnDestroy()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent -= OnSampleCompleted;
		}
	}
}