using UnityEngine;

namespace Runtime.Test
{
	public class LowPassFilterModule : MonoBehaviour
	{
		public bool master;
		[Range(10, 4000)] public double cutoff;
		[Range(0.7f, 10f)] public double q;
		public LfoSelection LfoSelection;
		public float lfoAmount;
		public EnvelopeSelection EnvelopeSelection;
		public float envelopeAmount;
	}
}