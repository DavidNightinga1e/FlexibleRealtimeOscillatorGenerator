using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.UI
{
	public class Menu : MonoBehaviour
	{
		[SerializeField] private GameObject menuButtonPrefab;
		[SerializeField] private GameObject pagesRoot;
		[SerializeField] private RectTransform menuButtonsContainer;

		private Page[] _pages;

		private void Start()
		{
			_pages = pagesRoot.GetComponentsInChildren<Page>(true);

			foreach (Page page in _pages)
			{
				GameObject instance = Instantiate(menuButtonPrefab, menuButtonsContainer);
				var menuButton = instance.GetComponent<MenuButton>();
				menuButton.SetPage(page);
				menuButton.Subscribe(() => OpenPage(page));
			}
			
			OpenPage(_pages.First());
		}

		private void OpenPage(Page p)
		{
			foreach (Page page in _pages)
			{
				page.gameObject.SetActive(false);
			}
			
			p.gameObject.SetActive(true);
		}
	}
}