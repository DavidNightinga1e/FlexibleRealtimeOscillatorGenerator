namespace Runtime.Synth.Presenters
{
	public class LfoFrequencyPresenter : LogFrequencyPresenter
	{
		protected override double MinFrequency { get; } = 0.01;
		protected override double MaxFrequency { get; } = 20.0;
		protected override double CurveStrength { get; } = 1;

		protected override string GetTextByFrequency(double frequency)
		{
			return frequency < 10 ? $"{frequency:0.00} Hz" : $"{frequency:0.0} Hz";
		}
	}
}