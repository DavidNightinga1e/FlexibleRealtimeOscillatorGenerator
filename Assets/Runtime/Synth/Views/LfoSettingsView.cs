using Runtime.UI;
using UnityEngine;

namespace Runtime.Synth.Views
{
	public class LfoSettingsView : MonoBehaviour
	{
		[SerializeField] private IntegerKnob waveformKnob;
		[SerializeField] private FloatKnob frequencyKnob;
	}
}