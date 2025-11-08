using System;

namespace Runtime.Synth.Distort
{
	public class HardClipDistort : IDistortLogic
	{
		private const double Threshold = 0.5;
		
		public double Process(double sample, double drive)
		{
			double amplified = sample * drive;
			return Math.Clamp(amplified, -Threshold, +Threshold);
		}
	}
}