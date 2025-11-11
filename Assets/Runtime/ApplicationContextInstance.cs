using System;
using Runtime.UI.Modals;
using UnityEngine;

namespace Runtime
{
	[DefaultExecutionOrder(-200)]
	public class ApplicationContextInstance : MonoBehaviour
	{
		public static ApplicationContextInstance Instance { get; private set; }
		
		[SerializeField] private ModalsController modalsController;
		
		public ModalsController ModalsController => modalsController;
		
		public PresetEditorContextInstance PresetEditorContext { get; set; }

		private void Awake()
		{
			Instance = this;
		}
	}
}