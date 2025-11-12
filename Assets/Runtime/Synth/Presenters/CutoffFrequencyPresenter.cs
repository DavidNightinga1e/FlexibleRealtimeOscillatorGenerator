namespace Runtime.Synth.Presenters
{
	public class CutoffFrequencyPresenter : LogFrequencyPresenter
	{
		protected override double MinFrequency { get; } = 10;
		protected override double MaxFrequency { get; } = 20000.0;

		protected override string GetTextByFrequency(double frequency)
		{
			return frequency < 1000 ? $"{frequency:0} Hz" : $"{frequency / 1000:0.0} kHz";
		}
	}
}