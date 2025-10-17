namespace Runtime.Synth
{
	public class EnvelopeSettings : SettingsBase
	{
		public double AttackDuration { get; set; }
		public double DecayDuration { get; set; }
		public double SustainValue { get; set; }
		public double ReleaseDuration { get; set; }

		public static EnvelopeSettings CreateDefault() => new()
		{
			AttackDuration = 0.1,
			DecayDuration = 0.1,
			SustainValue = 0.8,
			ReleaseDuration = 0.2
		};
	}
}