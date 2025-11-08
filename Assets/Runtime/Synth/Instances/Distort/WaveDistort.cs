using System;
using UnityEngine;

namespace Runtime.Synth.Distort
{
	public class WaveDistort : IDistortLogic
	{
		private const double Symmetry = 0.5;
		
		public double Process(double sample, double drive)
		{
			drive = Mathf.InverseLerp((float)drive, (float)DistortSettings.DriveMinValue, (float)DistortSettings.DriveMaxValue);
			double k = 2.0 * drive / (1.0 - drive);
			double symmetricInput = sample * (1.0f + (Symmetry - 0.5f) * 0.5f);
        
			return symmetricInput * (1.0f + k) / (1.0f + k * Math.Abs(symmetricInput));
		}
	}
}