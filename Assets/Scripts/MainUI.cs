using UnityEngine;

public class MainUI : MonoBehaviour
{
	public RectTransform[] m_panels;

	RectTransform m_currentPanel;

	void Start()
	{
		OnOpenPanel(m_panels[2]);
	}

	public void OnOpenPanel(RectTransform panel)
	{
		foreach (var item in m_panels)
		{
			item.gameObject.SetActive(false);
		}
		panel.gameObject.SetActive(true);
		m_currentPanel = panel;
	}
}
