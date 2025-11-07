using System;
using Runtime.UI.Modals;
using UnityEngine;

namespace Runtime
{
	public class ApplicationContextInstance : MonoBehaviour
	{
		public static ApplicationContextInstance Instance { get; private set; }
		
		[SerializeField] private ModalsController modalsController;
		
		public ModalsController ModalsController => modalsController;

		private void Awake()
		{
			Instance = this;
		}
	}
}