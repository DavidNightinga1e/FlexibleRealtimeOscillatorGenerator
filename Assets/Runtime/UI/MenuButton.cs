using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
	public class MenuButton : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private Image backgroundImage;
		[SerializeField] private TextMeshProUGUI text;
		
		private readonly Color _backgroundColor = new Color(0.18f, 0.18f, 0.22f);
		private readonly Color _textColor = new Color(1f, 1f, 1f, 0.8f);	
		private readonly Color _backgroundColorSelected = new Color(0.13f, 0.13f, 0.16f);
		private readonly Color _textColorSelected = new Color(0.33f, 0.88f, 0.89f);
		
		public Page Page { get; private set; }

		public void SetPage(Page page)
		{
			Page = page;
			text.text = page.PageName;
			Page.OnVisibilityChanged += SetSelected;
		}

		public void SetSelected(bool selected)
		{
			backgroundImage.color = selected ? _backgroundColorSelected : _backgroundColor;
			text.color = selected ? _textColorSelected : _textColor;
		}

		public void Subscribe(Action action)
		{
			button.onClick.AddListener(action.Invoke);
		}

		private void OnDestroy()
		{
			if (Page)
				Page.OnVisibilityChanged -= SetSelected;
		}
	}
}