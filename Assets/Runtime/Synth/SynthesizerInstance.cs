using System;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Synth
{
    public class SynthesizerInstance
    {
        private readonly SynthesizerPreset _preset;
        private readonly int _sampleRate;

        public event Action<double> SampleCompletedEvent;

        private readonly Voice[] _voices = new Voice[(int)(Note.C8 + 1)];
        private DistortInstance _distortInstance;
        private DelayInstance _delayInstance;
        private ReverbInstance _reverbInstance;

        public SynthesizerInstance(int sampleRate, SynthesizerPreset preset)
        {
            _sampleRate = sampleRate;
            _preset = preset;

            PrepareVoices();
            PrepareEffects();
        }

        public void OnNoteUp(Note note)
        {
            var i = (int)note;
            _voices[i].NoteUp();
        }

        public void OnNoteDown(Note note)
        {
            var i = (int)note;
            _voices[i].NoteDown();
        }

        public void OnAudioFilterRead(float[] data, int channels)
        {
            if (_preset is null)
                return;

            int dataLength = data.Length / channels;

            for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
            {
                var output = MixVoices();

                output = ApplyEffects(output);

                RaiseSampleCompleted(output);

                for (int channelIndex = 0; channelIndex < channels; channelIndex++)
                {
                    data[dataIndex * channels + channelIndex] += (float)output;
                }
            }
        }

        private void RaiseSampleCompleted(double sample)
        {
            SampleCompletedEvent?.Invoke(sample);
        }

        private void PrepareVoices()
        {
            for (int i = 0; i < _voices.Length; i++)
            {
                _voices[i] = new Voice
                (
                    _sampleRate,
                    NoteToFrequency.GetFrequency((Note)i),
                    _preset.Osc1Settings,
                    _preset.Osc2Settings,
                    _preset.Lfo1Settings,
                    _preset.Lfo2Settings,
                    _preset.FilterSettings,
                    _preset.AmpSettings,
                    _preset.Env1Settings,
                    _preset.Env2Settings
                );
            }
        }

        private void PrepareEffects()
        {
            _distortInstance = new DistortInstance(_sampleRate, _preset.DistortSettings);
            _delayInstance = new DelayInstance(_sampleRate, _preset.DelaySettings);
            _reverbInstance = new ReverbInstance(_sampleRate, _preset.ReverbSettings);
        }

        private double ApplyEffects(double sample)
        {
            sample = _distortInstance.ProcessSample(sample);
            sample = _delayInstance.ProcessSample(sample);
            sample = _reverbInstance.ProcessSample(sample);
            return sample;
        }

        private double MixVoices()
        {
            double signal = 0;
            double envelopeSum = 0;

            foreach (Voice v in _voices)
            {
                if (v.IsFinished)
                    continue;

                v.UpdateSample();
                signal += v.Sample;
                envelopeSum += v.AmpEnvelopeValue;
            }

            if (envelopeSum > 1)
                signal /= envelopeSum;

            if (double.IsNaN(signal))
            {
                Debug.LogError($"Mixer: signal was NaN");
                return 0;
            }

            if (Math.Abs(signal) > 1)
            {
                Debug.LogError($"Mixer: signal outside boundary {signal}");
                return Math.Clamp(signal, -1, 1);
            }

            return signal;
        }
    }
}