using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Synth.Views.Presets
{
	public class PresetInstanceView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private Image background;
		
		private readonly Color _backgroundIdleColor = Color.clear;
		private readonly Color _backgroundSelectedColor = new Color(0.33f, 0.88f, 0.89f);
		private readonly Color _textIdleColor = Color.white;
		private readonly Color _textSelectedColor = Color.black;
		
		private string _fileName;

		public string FileName
		{
			get => _fileName;
			set
			{
				text.text = Path.GetFileNameWithoutExtension(value);
				_fileName = value;
			}
		}

		public void SetSelected(bool value)
		{
			text.color = value ? _textSelectedColor : _textIdleColor;
			background.color = value ? _backgroundSelectedColor : _backgroundIdleColor;
		}
	}
}