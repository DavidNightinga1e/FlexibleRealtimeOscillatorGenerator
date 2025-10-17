using System;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class EnvelopeSettingsView : MonoBehaviour
	{
		[SerializeField] private EnvelopeDrawer envelopeDrawer;
		[SerializeField] private FloatKnob attackKnob;
		[SerializeField] private FloatKnob decayKnob;
		[SerializeField] private FloatKnob sustainKnob;
		[SerializeField] private FloatKnob releaseKnob;

		private const float AttackMin = 0.01f;
		private const float AttackMax = 1f;
		private const float DecayMin = 0f;
		private const float DecayMax = 1f;
		private const float ReleaseMin = 0.01f;
		private const float ReleaseMax = 2f;

		private EnvelopeSettings _settings;

		public void SetSettings(EnvelopeSettings settings)
		{
			_settings = settings;

			attackKnob.SetValueWithoutNotify(Mathf.InverseLerp(AttackMin, AttackMax, (float)settings.AttackDuration));
			decayKnob.SetValueWithoutNotify(Mathf.InverseLerp(DecayMin, DecayMax, (float)settings.DecayDuration));
			sustainKnob.SetValueWithoutNotify((float)settings.SustainValue);
			releaseKnob.SetValueWithoutNotify(
				Mathf.InverseLerp(ReleaseMin, ReleaseMax, (float)settings.ReleaseDuration));

			releaseKnob.SetValueText($"{_settings.ReleaseDuration * 1000:0} ms");
			sustainKnob.SetValueText($"{_settings.SustainValue * 100:00}%");
			decayKnob.SetValueText($"{_settings.DecayDuration * 1000:0} ms");
			attackKnob.SetValueText($"{_settings.AttackDuration * 1000:0} ms");

			UpdateEnvelope();
		}

		private void UpdateEnvelope()
		{
			envelopeDrawer.UpdateEnvelope
			(
				_settings.AttackDuration,
				_settings.DecayDuration,
				_settings.SustainValue,
				_settings.ReleaseDuration
			);
		}

		private void Awake()
		{
			attackKnob.ValueChanged += OnAttackChanged;
			decayKnob.ValueChanged += OnDecayChanged;
			sustainKnob.ValueChanged += OnSustainChanged;
			releaseKnob.ValueChanged += OnReleaseChanged;
		}

		private void OnReleaseChanged(float obj)
		{
			_settings.ReleaseDuration = Mathf.Lerp(ReleaseMin, ReleaseMax, obj);
			releaseKnob.SetValueText($"{_settings.ReleaseDuration * 1000:0} ms");
			_settings.InvokeChanged();
			UpdateEnvelope();
		}

		private void OnSustainChanged(float obj)
		{
			_settings.SustainValue = obj;
			sustainKnob.SetValueText($"{obj * 100:00}%");
			_settings.InvokeChanged();
			UpdateEnvelope();
		}

		private void OnDecayChanged(float obj)
		{
			_settings.DecayDuration = Mathf.Lerp(DecayMin, DecayMax, obj);
			decayKnob.SetValueText($"{_settings.DecayDuration * 1000:0} ms");
			_settings.InvokeChanged();
			UpdateEnvelope();
		}

		private void OnAttackChanged(float obj)
		{
			_settings.AttackDuration = Mathf.Lerp(AttackMin, AttackMax, obj);
			attackKnob.SetValueText($"{_settings.AttackDuration * 1000:0} ms");
			_settings.InvokeChanged();
			UpdateEnvelope();
		}
	}
}