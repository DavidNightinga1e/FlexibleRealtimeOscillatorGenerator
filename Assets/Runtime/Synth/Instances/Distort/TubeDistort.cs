namespace Runtime.Synth.Distort
{
	public class TubeDistort : IDistortLogic
	{
		public const double Bias = 0.5;
		
		public double Process(double sample, double drive)
		{
			double x = sample * drive;

			return x switch
			{
				< -Bias => -0.5f * Bias,
				> Bias => 0.5f * Bias,
				_ => x - (x * x) / (2.0f * Bias)
			};
		}
	}
}