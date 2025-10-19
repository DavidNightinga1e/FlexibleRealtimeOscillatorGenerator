namespace Runtime.Synth
{
	public class DelaySettings : SettingsBase
	{
		public bool Enabled { get; set; }
		public double DelayTime { get; set; }
		public double Feedback { get; set; }
		public double Mix { get; set; }

		public static DelaySettings CreateDefault() => new()
		{
			Enabled = false,
			DelayTime = 0.2,
			Feedback = 0.6,
			Mix = 0.5
		};
	}
}