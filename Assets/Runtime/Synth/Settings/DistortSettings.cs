namespace Runtime.Synth
{
	public class DistortSettings : SettingsBase
	{
		public bool Enabled { get; set; }

		public static DistortSettings CreateDefault() => new()
		{
			Enabled = false
		};
	}
}