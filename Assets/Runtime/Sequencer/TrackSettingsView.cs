using System;
using TMPro;
using UnityEngine;

namespace Runtime.Sequencer
{
    public class TrackSettingsView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField bpmInputField;
        [SerializeField] private TMP_InputField tickCountInputField;
        [SerializeField] private TMP_InputField loopDurationInputField;

        public int Bpm { get; private set; } = 120;
        public int TickCount { get; private set; } = 4;
        public int LoopDuration { get; private set; } = 5;

        private void Awake()
        {
            bpmInputField.text = Bpm.ToString();
            tickCountInputField.text = TickCount.ToString();
            loopDurationInputField.text = LoopDuration.ToString();
            
            bpmInputField.onValueChanged.AddListener(OnBpmValueChanged);
            tickCountInputField.onValueChanged.AddListener(OnTickCountValueChanged);
            loopDurationInputField.onValueChanged.AddListener(OnLoopDurationValueChanged);
        }

        private void OnLoopDurationValueChanged(string arg0)
        {
            LoopDuration = int.Parse(arg0);
        }

        private void OnTickCountValueChanged(string arg0)
        {
            TickCount = int.Parse(arg0);
        }

        private void OnBpmValueChanged(string arg0)
        {
            Bpm = int.Parse(arg0);
        }
    }
}