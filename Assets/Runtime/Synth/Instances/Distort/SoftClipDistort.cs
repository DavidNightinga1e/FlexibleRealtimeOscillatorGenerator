using System;

namespace Runtime.Synth.Distort
{
	public class SoftClipDistort : IDistortLogic
	{
		public double Process(double sample, double drive)
		{
			return 2.0 / Math.PI * Math.Atan(sample * drive);
		}
	}
}