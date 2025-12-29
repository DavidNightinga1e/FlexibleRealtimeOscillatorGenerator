using System;
using Runtime.Common;
using Runtime.UI.Keyboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Sequencer
{
    public class SequencerBehaviour : MonoBehaviour
    {
        [SerializeField] private TrackSettingsView trackSettingsView;
        [SerializeField] private Keyboard keyboard;
        [SerializeField] private InstrumentView instrumentView;
        [SerializeField] private Button playButton;
        [SerializeField] private Button stopButton;
        [SerializeField] private Button recButton;
        [SerializeField] private TextMeshProUGUI isRecLabel;
        
        private bool _isRecording;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlay);
            stopButton.onClick.AddListener(OnStop);
            recButton.onClick.AddListener(OnRec);
            
            keyboard.NoteUpEvent += OnNoteUp;
            keyboard.NoteDownEvent += OnNoteDown;
        }

        private void OnNoteDown(Note obj)
        {
            if (_isRecording)
                instrumentView.RecNoteDown(obj);
        }

        private void OnNoteUp(Note obj)
        {
            if (_isRecording)
                instrumentView.RecNoteUp(obj);
        }

        private void OnRec()
        {
            instrumentView.StartPlayback();
            isRecLabel.enabled = true;
            _isRecording = true;
        }

        private void OnStop()
        {
            instrumentView.StopPlayback();
            instrumentView.OnRecLoopEnd();
            isRecLabel.enabled = false;
            _isRecording = false;
        }

        private void OnPlay()
        {
            instrumentView.StartPlayback();
        }


        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}