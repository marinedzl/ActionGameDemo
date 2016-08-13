using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TestTool
{
	[MenuItem("Marine/ChangeItemName")]
	static void ChangeItemName()
	{
		GameObject go = Selection.activeGameObject;
		if (go != null)
		{
			int childCount = go.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform itemSlot = go.transform.GetChild(i);
				Text itemLabel = itemSlot.GetChild(0).GetChild(0).GetComponent<Text>();
				if (itemLabel != null)
				{
					itemLabel.text = "道具" + i.ToString();
				}
			}
		}
	}
}
