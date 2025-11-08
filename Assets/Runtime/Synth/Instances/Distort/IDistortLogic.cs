namespace Runtime.Synth.Distort
{
	public interface IDistortLogic
	{
		double Process(double sample, double drive);
	}
}