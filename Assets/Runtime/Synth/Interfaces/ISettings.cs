using System;

namespace Runtime.Synth
{
	public class SettingsBase
	{
		public event Action Changed;

		public void InvokeChanged()
		{
			Changed?.Invoke();
		}
	}
}