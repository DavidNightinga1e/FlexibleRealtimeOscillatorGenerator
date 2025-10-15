using UnityEngine;

namespace Runtime
{
	public class LowPassFilterModule : MonoBehaviour
	{
		public bool master;
		[Range(10, 4000)] public double cutoff;
		[Range(0.7f, 10f)] public double q;
	}
}