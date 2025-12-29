using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Common;
using Runtime.Synth;
using Runtime.Synth.Presets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Sequencer
{
    public class InstrumentView : MonoBehaviour
    {
        [SerializeField] private Button recButton;
        [SerializeField] private Button muteButton;
        [SerializeField] private Button soloButton;
        [SerializeField] private TMP_Dropdown presetDropdown;

        private List<Command> _toReleasePlaybackCommands = new List<Command>();
        private List<Command> _standbyPlaybackCommands = new List<Command>();

        private List<Command> _commands = new List<Command>();
        private List<Command> _newCommands = new List<Command>();

        private List<Command> _heldCommands = new();

        private bool _isPlayback;
        private float _playbackStartTime;

        private SynthesizerInstance _synthesizerInstance;

        private void Awake()
        {
            var optionDatas = PresetUtilities.GetPresetNames().Select(t => new TMP_Dropdown.OptionData(t)).ToList();
            presetDropdown.ClearOptions();
            presetDropdown.AddOptions(optionDatas);
        }

        private void OnEnable()
        {
            _synthesizerInstance =
                new SynthesizerInstance(AudioSettings.outputSampleRate, BuiltInPresets.CreateDefault());
        }

        private void OnDisable()
        {
            _synthesizerInstance = null;
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (_synthesizerInstance == null)
                return;

            if (!_isPlayback)
                return;

            _synthesizerInstance.OnAudioFilterRead(data, channels);
        }

        private void Update()
        {
            var commands = _standbyPlaybackCommands.Where(t => t.OnTime <= Time.time - _playbackStartTime).ToList();
            foreach (var c in commands)
            {
                _synthesizerInstance.OnNoteDown(c.Note);
                _standbyPlaybackCommands.Remove(c);
            }
            
            _toReleasePlaybackCommands.AddRange(commands);

            var toRelease = _toReleasePlaybackCommands.Where(t => t.OffTime <= Time.time - _playbackStartTime).ToList();
            foreach (var c in toRelease)
            {
                _synthesizerInstance.OnNoteUp(c.Note);
                _toReleasePlaybackCommands.Remove(c);
            }
        }

        public void StartPlayback()
        {
            _isPlayback = true;
            _playbackStartTime = Time.time;
            _standbyPlaybackCommands.Clear();
            _toReleasePlaybackCommands.Clear();
            _standbyPlaybackCommands.AddRange(_commands);
        }

        public void StopPlayback()
        {
            _isPlayback = false;
        }

        public void RecNoteDown(Note note)
        {
            _heldCommands.Add(new Command
            {
                Note = note,
                OnTime = Time.time - _playbackStartTime,
                OffTime = 0
            });
            _synthesizerInstance.OnNoteDown(note);
        }

        public void RecNoteUp(Note note)
        {
            var command = _heldCommands.First(t => t.Note == note);
            command.OffTime = Time.time - _playbackStartTime;

            _heldCommands.Remove(command);

            _newCommands.Add(command);
            
            _synthesizerInstance.OnNoteUp(note);
        }

        public void OnRecLoopEnd()
        {
            foreach (var c in _heldCommands)
            {
                c.OffTime = Time.time - _playbackStartTime;
                _synthesizerInstance.OnNoteUp(c.Note);
            }

            _newCommands.AddRange(_heldCommands);
            _heldCommands.Clear();

            _commands.AddRange(_newCommands);
            _newCommands.Clear();
        }
    }
}