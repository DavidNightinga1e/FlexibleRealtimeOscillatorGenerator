using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
	public class SinewaveGenerator : MonoBehaviour
	{
		[Serializable]
		public class Settings
		{
			public float Amplitude;
			public float Frequency;
		}

		private class Sine
		{
			public float Amplitude;
			public float Frequency;

			public double Phase;
		}
		
		[SerializeField] private Text _dspTimeText;
		[SerializeField] private LineRenderer _lineRenderer;

		public List<Settings> SinesSetup;

		private List<Sine> _sines;

		private int _sampleRate;
		private double _sine;
		
		private double _lastTime;

		private int _ptr;
		
		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
			_sines = SinesSetup.Select(t => new Sine
			{
				Amplitude = t.Amplitude,
				Frequency = t.Frequency,
				Phase = 0
			}).ToList();
		}

		private void Update()
		{
			_dspTimeText.text = $"time: {_lastTime:0.000}, sine: {_sine:0.000}";
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			int dataLength = data.Length / channels;
			
			for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
			{
				float output = 0;
				foreach (Sine sine in _sines)
				{
					double phaseIncrement = 2.0 * Math.PI * sine.Frequency / _sampleRate;
					sine.Phase += phaseIncrement;
					while (sine.Phase >= Math.PI * 2)
					{
						sine.Phase -= Math.PI * 2;
					}
					output += sine.Amplitude * Mathf.Sin((float)(sine.Phase));
				}
				_sine = output;
		
				for (int channelIndex = 0; channelIndex < channels; channelIndex++)
				{
					data[dataIndex * channels + channelIndex] += output;
				}
			}
			
			_lastTime = AudioSettings.dspTime;
		}
	}
}