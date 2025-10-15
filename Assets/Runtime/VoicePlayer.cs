using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
	public class VoicePlayer : MonoBehaviour
	{
		private readonly List<Voice> _voices = new();

		private readonly object _lock = new();

		private int _sampleRate;

		public void AddVoice(Voice voice)
		{
			lock (_lock)
			{
				_voices.Add(voice);
			}
		}

		public void RemoveVoice(Voice voice)
		{
			lock (_lock)
			{
				_voices.Remove(voice);
			}
		}

		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			lock (_lock)
			{
				if (_voices.Count == 0)
					return;

				int dataLength = data.Length / channels;

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

					for (int channelIndex = 0; channelIndex < channels; channelIndex++)
					{
						data[dataIndex * channels + channelIndex] += (float)signal;
					}
				}
			}
		}
	}
}