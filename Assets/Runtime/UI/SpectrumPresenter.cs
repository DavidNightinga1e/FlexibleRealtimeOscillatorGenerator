using System;
using System.Linq;
using System.Numerics;
using NUnit.Framework.Constraints;
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
		
		private FrequencyPlotAnalyzer _frequencyPlotAnalyzer = new();

		private System.Numerics.Vector2[] _points;

		private void Awake()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent += OnSampleCompleted;
		}

		private void OnSampleCompleted(double sample)
		{
			var result = _frequencyPlotAnalyzer.ProcessSample((float)sample);
			if (result != null)
				_points = result;
		}

		private void Update()
		{
			_frequencyPlotRenderer.Points = _points.Select(t => new Vector2(t.X * _rendererWidth, t.Y * _rendererHeight)).ToArray();
		}
		
		private void OnDestroy()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent -= OnSampleCompleted;
		}
	}
}