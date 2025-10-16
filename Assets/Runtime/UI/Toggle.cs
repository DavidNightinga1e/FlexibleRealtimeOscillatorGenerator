using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
	public class Toggle : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private Image on;
		[SerializeField] private Image off;
		
		public bool State { get; private set; }
		public event Action<bool> OnStateChanged; 

		private void Awake()
		{
			button.onClick.AddListener(OnButtonClick);
			SyncViewToState();
		}

		private void SyncViewToState()
		{
			on.enabled = State;
			off.enabled = !State;
		}

		private void OnButtonClick()
		{
			State = !State;
			SyncViewToState();
			OnStateChanged?.Invoke(State);
		}
	}
}