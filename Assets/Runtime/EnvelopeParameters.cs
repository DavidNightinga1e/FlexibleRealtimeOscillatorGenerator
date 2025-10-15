using System;

namespace Runtime
{
	[Serializable]
	public class EnvelopeParameters
	{
		public double Attack = 0.1;
		public double Decay = 0.1;
		public double Sustain = 0.9;
		public double Release = 0.2;
	}
}