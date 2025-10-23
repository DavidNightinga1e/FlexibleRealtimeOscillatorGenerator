using Runtime.Common;
using Runtime.Synth.Presenters;
using Runtime.Test;
using UnityEngine;
using Toggle = Runtime.UI.Toggle;

namespace Runtime.Synth.Views
{
	public class OscillatorSettingsView : MonoBehaviour
	{
		[SerializeField] private Toggle enabledToggle;
		[SerializeField] private WaveformPresenter waveformPresenter;
		[SerializeField] private GainPresenter gainPresenter;
		[SerializeField] private OctaveShiftPresenter octaveShiftPresenter;
		[SerializeField] private EnvelopeSelectionPresenter envelopeSelectionPresenter;
		[SerializeField] private VibratoAmountPresenter vibratoAmountPresenter;
		[SerializeField] private LfoSelectionPresenter vibratoLfoSelectionPresenter;
		[SerializeField] private GainPresenter tremoloDepthPresenter;
		[SerializeField] private LfoSelectionPresenter tremoloLfoSelectionPresenter;

		private OscillatorSettings _settings;

		public void SetSettings(OscillatorSettings settings)
		{
			_settings = settings;

			enabledToggle.SetValueWithoutNotify(_settings.Enabled);
			waveformPresenter.SetValueWithoutNotify(_settings.Waveform);
			gainPresenter.SetValueWithoutNotify(_settings.Gain);
			octaveShiftPresenter.SetValueWithoutNotify(_settings.OctaveShift);
			envelopeSelectionPresenter.SetValueWithoutNotify(_settings.EnvelopeSelection);
			vibratoLfoSelectionPresenter.SetValueWithoutNotify(_settings.VibratoLfoSelection);
			vibratoAmountPresenter.SetValueWithoutNotify(_settings.VibratoAmountSemitones);
			tremoloDepthPresenter.SetValueWithoutNotify(_settings.TremoloDepth);
			tremoloLfoSelectionPresenter.SetValueWithoutNotify(_settings.TremoloLfoSelection);
		}

		private void Awake()
		{
			enabledToggle.ValueChanged += OnEnabledValueChanged;
			waveformPresenter.ValueChanged += OnWaveformChanged;
			gainPresenter.ValueChanged += OnGainChanged;
			octaveShiftPresenter.ValueChanged += OnOctaveShiftChanged;
			envelopeSelectionPresenter.ValueChanged += OnEnvelopeSelectionChanged;
			vibratoLfoSelectionPresenter.ValueChanged += OnVibratoLfoSelectionChanged;
			vibratoAmountPresenter.ValueChanged += OnVibratoAmountChanged;
			tremoloDepthPresenter.ValueChanged += OnTremoloDepthValueChange;
			tremoloLfoSelectionPresenter.ValueChanged += OnTremoloLfoSelectionChanged;
		}

		private void OnTremoloLfoSelectionChanged(LfoSelection obj)
		{
			_settings.TremoloLfoSelection = obj;
			_settings.InvokeChanged();
		}

		private void OnTremoloDepthValueChange(double obj)
		{
			_settings.TremoloDepth = obj;
			_settings.InvokeChanged();
		}

		private void OnVibratoAmountChanged(double obj)
		{
			_settings.VibratoAmountSemitones = obj;
			_settings.InvokeChanged();
		}

		private void OnVibratoLfoSelectionChanged(LfoSelection obj)
		{
			_settings.VibratoLfoSelection = obj;
			_settings.InvokeChanged();
		}

		private void OnEnvelopeSelectionChanged(EnvelopeSelection obj)
		{
			_settings.EnvelopeSelection = obj;
			_settings.InvokeChanged();
		}

		private void OnOctaveShiftChanged(int obj)
		{
			_settings.OctaveShift = obj;
			_settings.InvokeChanged();
		}

		private void OnGainChanged(double obj)
		{
			_settings.Gain = obj;
			_settings.InvokeChanged();
		}

		private void OnWaveformChanged(Waveform obj)
		{
			_settings.Waveform = obj;
			_settings.InvokeChanged();
		}

		private void OnEnabledValueChanged(bool obj)
		{
			_settings.Enabled = obj;
			_settings.InvokeChanged();
		}

		private void OnDestroy()
		{
			enabledToggle.ValueChanged -= OnEnabledValueChanged;
			waveformPresenter.ValueChanged -= OnWaveformChanged;
			gainPresenter.ValueChanged -= OnGainChanged;
			octaveShiftPresenter.ValueChanged -= OnOctaveShiftChanged;
			envelopeSelectionPresenter.ValueChanged -= OnEnvelopeSelectionChanged;
			vibratoLfoSelectionPresenter.ValueChanged -= OnVibratoLfoSelectionChanged;
			vibratoAmountPresenter.ValueChanged -= OnVibratoAmountChanged;
			tremoloDepthPresenter.ValueChanged -= OnTremoloDepthValueChange;
			tremoloLfoSelectionPresenter.ValueChanged -= OnTremoloLfoSelectionChanged;
		}
	}
}