using System;
using Runtime.Synth;
using Runtime.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime
{
	[DefaultExecutionOrder(-100)]
	public class PresetEditorContextInstance : MonoBehaviour
	{
		[SerializeField] private SynthesizerBehaviour synthesizerBehaviour;
		
		public SynthesizerBehaviour SynthesizerBehaviour => synthesizerBehaviour;
		
		private void Awake()
		{
			ApplicationContextInstance.Instance.PresetEditorContext = this;
		}
	}
}