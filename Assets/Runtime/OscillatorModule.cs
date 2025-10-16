using UnityEngine;

namespace Runtime
{
	public class OscillatorModule : MonoBehaviour
	{
		public bool master;
		public Waveform waveform;
		public int octaveShift;
		public float vibratoAmount;
		public LfoSelection vibratoLfo;
	}
}