using Runtime.Synth;
using UnityEngine;

namespace Runtime.UI
{
	public class OutputWaveformPresenter : MonoBehaviour
	{
		[SerializeField] private UILineRenderer _waveformRenderer;
		[SerializeField] private int _waveformRendererSampleRateDivider;
		[SerializeField] private float _waveformRendererWidth;
		[SerializeField] private float _waveformRendererHeight;
		
		private const int _waveformSampleCount = 400;

		private readonly float[] _waveformSamples = new float[_waveformSampleCount];
		private int _waveformWriteIndex = 0;
		private int _skipCount = 0;
		private readonly Vector2[] _waveformPositions = new Vector2[_waveformSampleCount];

		private void Awake()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent += OnSampleCompleted;
		}

		private void OnSampleCompleted(double sample)
		{
			_skipCount++;
			if (_skipCount < _waveformRendererSampleRateDivider) 
				return;
			
			_skipCount = 0;
			_waveformSamples[_waveformWriteIndex] = (float)sample;

			_waveformWriteIndex++;
			_waveformWriteIndex %= _waveformSamples.Length;
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
		}
		
		private void OnDestroy()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent -= OnSampleCompleted;
		}
	}
}