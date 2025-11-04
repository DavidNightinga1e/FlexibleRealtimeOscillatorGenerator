using System;
using UnityEngine;

namespace Runtime.UI
{
	public class Page : MonoBehaviour
	{
		[SerializeField] private string pageName;
		
		public string PageName => pageName;
		
		public event Action<bool> OnVisibilityChanged;

		private void OnEnable()
		{
			OnVisibilityChanged?.Invoke(true);
		}

		private void OnDisable()
		{
			OnVisibilityChanged?.Invoke(false);
		}
	}
}