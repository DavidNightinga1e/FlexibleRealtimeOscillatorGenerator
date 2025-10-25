using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Test
{
	public class Synthesizer : MonoBehaviour
	{
		[SerializeField] private EnvelopeModule env1;
		[SerializeField] private EnvelopeModule env2;
		[SerializeField] private LfoModule lfo1;
		[SerializeField] private LfoModule lfo2;
		[SerializeField] private LowPassFilterModule _lowPassFilterModule;
		[SerializeField] private List<OscillatorModule> _oscillatorModules;
		[SerializeField] private EnvelopeModule _envelopeModule;
		[SerializeField] private VoicePlayer _voicePlayer;
		[SerializeField] private Keyboard _keyboard;

		private readonly Dictionary<Note, Voice> _activeVoices = new();

		private int _sampleRate;

		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
			_keyboard.NoteOn += NoteOn;
			_keyboard.NoteOff += NoteOff;
		}

		private void NoteOff(Note note)
		{
			Voice voice = _activeVoices[note];
			voice.Release();
		}

		private void NoteOn(Note note)
		{
			if (_activeVoices.ContainsKey(note))
			{
				if (_activeVoices[note].IsReleasing)
				{
					_activeVoices[note].Attack();
				}

				return;
			}

			double frequency = NoteToFrequency.GetFrequency(note);
			List<Oscillator> oscillators = _oscillatorModules
				.Where(t => t.master)
				.Select(t =>
				{
					LfoModule vibratoLfo = t.vibratoLfo switch
					{
						LfoSelection.Off => null,
						LfoSelection.Lfo1 => lfo1,
						LfoSelection.Lfo2 => lfo2,
						_ => throw new ArgumentOutOfRangeException()
					};
					Oscillator lfo = null;
					if (vibratoLfo != null)
						lfo = new Oscillator(vibratoLfo.wave, vibratoLfo.frequency, _sampleRate, null, 0, 0);
					return new Oscillator(t.waveform, frequency, _sampleRate, lfo, t.vibratoAmount, t.octaveShift);
				})
				.ToList();
			
			EnvelopeParameters filterEnvelopeParameters = _lowPassFilterModule.EnvelopeSelection switch
			{
				EnvelopeSelection.Off => null,
				EnvelopeSelection.Env1 => env1.Parameters,
				EnvelopeSelection.Env2 => env2.Parameters,
				_ => throw new ArgumentOutOfRangeException()
			};
			Oscillator lfo = _lowPassFilterModule.LfoSelection switch
			{
				LfoSelection.Off => null,
				LfoSelection.Lfo1 => new Oscillator(lfo1.wave, lfo1.frequency, _sampleRate, null, 0),
				LfoSelection.Lfo2 => new Oscillator(lfo2.wave, lfo2.frequency, _sampleRate, null, 0),
				_ => throw new ArgumentOutOfRangeException()
			};
			var voice = new Voice(oscillators, _lowPassFilterModule, _envelopeModule.Parameters, filterEnvelopeParameters, lfo, _sampleRate);
			_activeVoices.Add(note, voice);
			_voicePlayer.AddVoice(voice);
		}

		private void Update()
		{
			var voicesToRemove = _activeVoices
				.Where(t => t.Value.IsFinished)
				.ToList();
			foreach (var pair in voicesToRemove)
			{
				_activeVoices.Remove(pair.Key);
				_voicePlayer.RemoveVoice(pair.Value);
			}
		}

		private void OnDestroy()
		{
			_keyboard.NoteOn -= NoteOn;
			_keyboard.NoteOff -= NoteOff;
		}
	}
}