namespace Runtime.Synth
{
	public class ReverbSettings : SettingsBase
	{
		public bool Enabled { get; set; }
		public double DecayTime { get; set; }
		public double Mix { get; set; }
		
		public const double MaxDecayTime = 2.0;

		public static ReverbSettings CreateDefault() => new()
		{
			Enabled = false,
			DecayTime = 0.2,
			Mix = 0.5
		};
	}
}