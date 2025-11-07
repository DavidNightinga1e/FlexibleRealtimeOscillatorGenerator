using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Modals
{
	public class StringModal : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private Button okButton;
		[SerializeField] private Button cancelButton;

		private Action<StringModalResponse> _callback;

		private void Awake()
		{
			okButton.onClick.AddListener(OnOkButtonClick);
			cancelButton.onClick.AddListener(OnCancelButtonClick);
		}
		
		public void Set(string currentText, Action<StringModalResponse> callback)
		{
			inputField.text = currentText;
			_callback = callback;
		}

		private void OnCancelButtonClick()
		{
			_callback.Invoke(new StringModalResponse
			{
				ButtonPressed = ModalButton.Cancel,
				Value = inputField.text
			});
		}

		private void OnOkButtonClick()
		{
			_callback.Invoke(new StringModalResponse
			{
				ButtonPressed = ModalButton.Ok,
				Value = inputField.text
			});
		}
	}
}