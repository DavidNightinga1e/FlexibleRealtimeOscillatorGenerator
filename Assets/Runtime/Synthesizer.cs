using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
	public class Synthesizer : MonoBehaviour
	{
		[SerializeField] private List<OscillatorModule> _oscillatorModules;
		[SerializeField] private EnvelopeModule _envelopeModule;
		[SerializeField] private VoicePlayer _voicePlayer;
		[SerializeField] private Keyboard _keyboard;

		private readonly Dictionary<Note, Voice> _activeVoices = new();
		
		private int _sampleRate;

		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
			_keyboard.OnNotePressed += OnNotePressed;
			_keyboard.OnNoteReleased += OnNoteReleased;
		}

		private void OnNoteReleased(Note note)
		{
			Voice voice = _activeVoices[note];
			voice.Release();
		}

		private void OnNotePressed(Note note)
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
				.Select(t => new Oscillator(t.waveform, frequency, _sampleRate, t.octaveShift))
				.ToList();
			var voice = new Voice(oscillators, _envelopeModule.Parameters, _sampleRate, frequency);
			_activeVoices.Add(note, voice);
			_voicePlayer.AddVoice(voice);
		}

		private void Update()
		{
			var voicesToRemove = _activeVoices
				.Where(t => t.Value.IsReleasing && t.Value.Envelope.Value < 0.00001)
				.ToList();
			foreach (var pair in voicesToRemove)
			{
				_activeVoices.Remove(pair.Key);
				_voicePlayer.RemoveVoice(pair.Value);
			}
		}

		private void OnDestroy()
		{
			_keyboard.OnNotePressed -= OnNotePressed;
			_keyboard.OnNoteReleased -= OnNoteReleased;
		}
	}
}