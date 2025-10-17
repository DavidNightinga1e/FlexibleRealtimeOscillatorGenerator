using System.Collections.Generic;
using Runtime.Common;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Test
{
	public class VoicePlayer : MonoBehaviour
	{
		[SerializeField] private DelayModule _delayModule;
		[SerializeField] private Image dbMeter;
		[SerializeField] private LineRenderer _lineRenderer;

		private readonly List<Voice> _voices = new();

		private readonly object _voicesLock = new();
		private readonly object _outputLock = new();

		private int _sampleRate;
		private DelayEffect _delayEffect;

		private const int OutputBufferSize = 10000;
		
		private double[] _waveformBuffer;
		private int _waveformBufferPointer;
		private NativeArray<Vector3> _positions;
		private VUMeter _meter = new();

		public void AddVoice(Voice voice)
		{
			lock (_voicesLock)
			{
				_voices.Add(voice);
			}
		}

		public void RemoveVoice(Voice voice)
		{
			lock (_voicesLock)
			{
				_voices.Remove(voice);
			}
		}

		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
			_delayEffect = new DelayEffect(_sampleRate, _delayModule._delayTime, _delayModule._feedback, _delayModule._mix);
			
			_positions = new NativeArray<Vector3>(_sampleRate, Allocator.Domain);
			_lineRenderer.positionCount = OutputBufferSize;
			_lineRenderer.SetPositions(_positions);
			_waveformBuffer = new double[OutputBufferSize];
		}

		private void SetDbMeter(float v)
		{
			float lerp = Mathf.InverseLerp(-60, 0, v);
			dbMeter.rectTransform.anchorMax = new Vector2(lerp, 1);
		}

		private void Update()
		{
			lock (_outputLock)
			{
				for (int i = 0; i < OutputBufferSize; i++)
				{
					double d = _waveformBuffer[i];
					_positions[i] = new Vector3(14 * ((float)i / OutputBufferSize) - 7, (float)d, 0);
				}
			}

			_lineRenderer.SetPositions(_positions);
			
			SetDbMeter((float)_meter.GetPeakDbfs());
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			lock (_voicesLock)
			{
				int dataLength = data.Length / channels;

				if (_voices.Count == 0)
				{
					for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
					{
						var signal = _delayEffect.Process(0);
						
						for (int channelIndex = 0; channelIndex < channels; channelIndex++)
						{
							data[dataIndex * channels + channelIndex] += (float)signal;
						}
						
						OnSampleComplete(signal);
					}

					return;
				}

				for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
				{
					double signal = MixVoices();

					signal = _delayEffect.Process(signal);

					for (int channelIndex = 0; channelIndex < channels; channelIndex++)
					{
						data[dataIndex * channels + channelIndex] += (float)signal;
					}
					
					OnSampleComplete(signal);
				}
			}
		}

		private void OnSampleComplete(double sample)
		{
			_meter.ProcessSample(sample);
			AddSampleToWaveformBuffer(sample);
		}

		private void AddSampleToWaveformBuffer(double signal)
		{
			lock (_outputLock)
			{
				_waveformBuffer[_waveformBufferPointer] = signal;
				_waveformBufferPointer = (_waveformBufferPointer + 1) % _waveformBuffer.Length;
			}
		}

		private double MixVoices()
		{
			double signal = 0;
			double totalEnvelope = 0;

			foreach (Voice voice in _voices)
			{
				double sample = voice.UpdateSample();
				signal += sample;
				totalEnvelope += voice.AmpEnvelope.Value;
			}

			if (totalEnvelope > 1)
				signal /= totalEnvelope;

			if (double.IsNaN(signal))
			{
				Debug.LogError($"Output was NaN, voices count: {_voices.Count}");
			}

			return signal;
		}
	}
}