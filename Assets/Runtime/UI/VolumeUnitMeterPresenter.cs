using System;
using System.Globalization;
using Runtime.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
	public class VolumeUnitMeterPresenter : MonoBehaviour
	{
		[SerializeField] private RectMask2D _mask;
		[SerializeField] private TextMeshProUGUI _text;
		
		private VUMeter _meter = new VUMeter();

		private void Awake()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent += OnSampleCompleted;
		}

		private void OnSampleCompleted(double obj)
		{
			_meter.ProcessSample(obj);
		}

		private void Update()
		{
			var peakDbfs = (float)_meter.GetPeakDbfs();
			SetMaskValue(1- Mathf.InverseLerp(-20, 0, peakDbfs));
			_text.text = peakDbfs.ToString("0", CultureInfo.InvariantCulture);
		}

		private void SetMaskValue(float valueNormalized)
		{
			var rt = (RectTransform)transform;
			float value = valueNormalized * rt.sizeDelta.y;
			Vector4 maskPadding = _mask.padding;
			maskPadding.w = value;
			_mask.padding = maskPadding;
		}

		private void OnDestroy()
		{
			ApplicationContext.PresetEditor.SynthesizerBehaviour.SampleCompletedEvent -= OnSampleCompleted;
		}
	}
}