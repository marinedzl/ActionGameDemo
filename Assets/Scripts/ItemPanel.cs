using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
	public Text m_itemName;

	Button m_selectedItem;

	void Start()
	{
		OnClickItem(null);
	}

	public void OnClickItem(Button item)
	{
		if (m_selectedItem != null)
		{
			m_selectedItem.interactable = true;
		}
		if (item != null)
		{
			m_selectedItem = item;
			m_selectedItem.interactable = false;
			m_itemName.gameObject.SetActive(true);
			m_itemName.text = m_selectedItem.transform.GetChild(0).GetComponent<Text>().text;
		}
		else
		{
			m_itemName.gameObject.SetActive(false);
		}
	}
}
