using Runtime.Common;
using UnityEngine;

namespace Runtime.Test
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