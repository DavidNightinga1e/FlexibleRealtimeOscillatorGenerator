using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
	public class VoicePlayer : MonoBehaviour
	{
		[SerializeField] private Image dbMeter;
		[SerializeField] private LineRenderer _lineRenderer;

		private readonly List<Voice> _voices = new();

		private readonly object _voicesLock = new();
		private readonly object _outputLock = new();

		private int _sampleRate;
		private DelayEffect _delayEffect;

		private const int OutputBufferSize = 10000;
		
		private double[] _outputBuffer;
		private int _outputBufferPointer;
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
			_delayEffect = new DelayEffect(_sampleRate, 0.2, 0, 0);
			
			_positions = new NativeArray<Vector3>(_sampleRate, Allocator.Domain);
			_lineRenderer.positionCount = OutputBufferSize;
			_lineRenderer.SetPositions(_positions);
			_outputBuffer = new double[OutputBufferSize];
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
					double d = _outputBuffer[i];
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
						
						_meter.ProcessSample(signal);

						lock (_outputLock)
						{
							_outputBuffer[_outputBufferPointer] = signal;
							_outputBufferPointer = (_outputBufferPointer + 1) % _outputBuffer.Length;
						}
					}

					return;
				}

				for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
				{
					double signal = 0;
					double totalEnvelope = 0;

					foreach (Voice voice in _voices)
					{
						double sample = voice.UpdateSample();
						signal += sample;
						totalEnvelope += voice.Envelope.Value;
					}

					if (totalEnvelope > 1)
						signal /= totalEnvelope;

					if (double.IsNaN(signal))
					{
						Debug.LogError($"Output was NaN, voices count: {_voices.Count}");
					}

					signal = _delayEffect.Process(signal);
					
					_meter.ProcessSample(signal);

					for (int channelIndex = 0; channelIndex < channels; channelIndex++)
					{
						data[dataIndex * channels + channelIndex] += (float)signal;
					}
					
					lock (_outputLock)
					{
						_outputBuffer[_outputBufferPointer] = signal;
						_outputBufferPointer = (_outputBufferPointer + 1) % _outputBuffer.Length;
					}
				}
			}
		}
	}
}