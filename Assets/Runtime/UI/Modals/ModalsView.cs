using UnityEngine;

namespace Runtime.UI.Modals
{
	public class ModalsView : MonoBehaviour
	{
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private StringModal stringModal;
		
		public StringModal StringModal => stringModal;

		public void Show()
		{
			gameObject.SetActive(true);
			canvasGroup.alpha = 1;
		}

		public void Hide()
		{
			gameObject.SetActive(false);
			canvasGroup.alpha = 0;
		}
	}
}