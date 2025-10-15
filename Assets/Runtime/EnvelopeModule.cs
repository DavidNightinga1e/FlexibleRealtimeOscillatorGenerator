using UnityEngine;

namespace Runtime
{
	public class EnvelopeModule : MonoBehaviour
	{
		public EnvelopeParameters Parameters;

		[ContextMenu("Load Default")]
		public void LoadDefault()
		{
			Parameters = new EnvelopeParameters();
		}
	}
}