using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
	public class Synthesizer : MonoBehaviour
	{
		[SerializeField] private LowPassFilterModule _lowPassFilterModule;
		[SerializeField] private List<OscillatorModule> _oscillatorModules;
		[SerializeField] private EnvelopeModule _envelopeModule;
		[SerializeField] private VoicePlayer _voicePlayer;
		[SerializeField] private Keyboard _keyboard;

		private readonly Dictionary<Note, Voice> _activeVoices = new();

		private int _sampleRate;
		private LowPassFilter _lowPassFilter;

		private void Start()
		{
			_sampleRate = AudioSettings.outputSampleRate;
			_keyboard.OnNotePressed += OnNotePressed;
			_keyboard.OnNoteReleased += OnNoteReleased;

			_lowPassFilter = new LowPassFilter(_lowPassFilterModule.cutoff, _lowPassFilterModule.q, _sampleRate);
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
				.Where(t => t.master)
				.Select(t => new Oscillator(t.waveform, frequency, _sampleRate, t.octaveShift))
				.ToList();
			var voice = new Voice(oscillators, _lowPassFilter, _envelopeModule.Parameters, _sampleRate, frequency);
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
			
			_lowPassFilter.SetParameters(_lowPassFilterModule.cutoff, _lowPassFilterModule.q, _sampleRate);
		}

		private void OnDestroy()
		{
			_keyboard.OnNotePressed -= OnNotePressed;
			_keyboard.OnNoteReleased -= OnNoteReleased;
		}
	}
}