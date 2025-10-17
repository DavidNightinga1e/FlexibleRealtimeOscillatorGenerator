namespace Runtime.Synth
{
	// Everything that needs to be updated once per sample
	// has to implement that
	// 
	// Do not update any oscillators outside this, so there is no
	// occurence of frequency change from multiple connections 
	public interface ISampleProvider
	{
		public double Sample { get; }
		void UpdateSample();
	}
}