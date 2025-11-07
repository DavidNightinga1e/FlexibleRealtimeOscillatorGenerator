using System;

namespace Runtime.Synth
{
	public class ReverbInstance
	{
		private const int CombsCount = 8;
		private const int AllPassCount = 4;

		private CombFilter[] _combFilters = new CombFilter[CombsCount];
		private AllPassFilter[] _allPassFilters = new AllPassFilter[AllPassCount];
		
		private readonly ReverbSettings _settings;
		private readonly int _sampleRate;
		
		// tuning for 44100
		private static readonly int[] CombTunings = { 1116, 1188, 1277, 1356, 1422, 1491, 1557, 1617 };
		private static readonly int[] AllPassTunings = { 556, 441, 341, 225 };

		public ReverbInstance
		(
			int sampleRate,
			ReverbSettings reverbSettings
		)
		{
			_sampleRate = sampleRate;
			_settings = reverbSettings;

			float scale = (float)_sampleRate / 44100;

			for (int i = 0; i < _combFilters.Length; i++)
			{
				int delay = (int)(CombTunings[i] * scale);
				_combFilters[i] = new CombFilter(delay);
			}

			for (int i = 0; i < _allPassFilters.Length; i++)
			{
				int delay = (int)(AllPassTunings[i] * scale);
				_allPassFilters[i] = new AllPassFilter(delay);
			}
		}

		private void OnRoomSizeChanged(double roomSize)
		{
			double combFeedback = 0.7 + roomSize * 0.28;
			double allPassFeedback = 0.3 + roomSize * 0.4;

			foreach (CombFilter t in _combFilters) t.SetFeedback(combFeedback);
			foreach (AllPassFilter t in _allPassFilters) t.SetFeedback(allPassFeedback);
		}

		private void OnDampChanged(double damp)
		{
			damp *= 0.6;

			foreach (CombFilter t in _combFilters) t.SetDamping(damp);
		}

		public double ProcessSample(double sample)
		{
			if (!_settings.Enabled)
				return sample;
			
			OnRoomSizeChanged(_settings.RoomSize);
			OnDampChanged(_settings.Damp);

			var input  = sample * 0.015; // wtf

			double output = 0;

			foreach (CombFilter t in _combFilters) output += t.Process(input);
			foreach (AllPassFilter t in _allPassFilters) output = t.Process(output);

			double dry = 1 - _settings.Mix;
			double wet = _settings.Mix;
			return sample * dry + output * wet;
		}
	}
}