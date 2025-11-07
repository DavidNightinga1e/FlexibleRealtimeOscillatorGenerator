using System;
using UnityEngine;

namespace Runtime.UI.Modals
{
	public class ModalsController : MonoBehaviour
	{
		[SerializeField] private ModalsView modalsView;
		
		private Action<StringModalResponse> _stringModalCallback;

		private int _activeModalsCount = 0;

		private void Awake()
		{
			modalsView.Hide();
		}

		public void ShowStringModal(string currentText, Action<StringModalResponse> callback)
		{
			if (_activeModalsCount is 0)
				modalsView.Show();
			_activeModalsCount++;
			
			_stringModalCallback = callback;
			
			modalsView.StringModal.Set(currentText, OnStringModalCallback);
			
			modalsView.StringModal.gameObject.SetActive(true);
		}

		private void OnStringModalCallback(StringModalResponse obj)
		{
			_activeModalsCount--;
			if (_activeModalsCount is 0)
				modalsView.Hide();
			
			modalsView.StringModal.gameObject.SetActive(false);
			
			_stringModalCallback?.Invoke(obj);
			_stringModalCallback = null;
		}
	}
}