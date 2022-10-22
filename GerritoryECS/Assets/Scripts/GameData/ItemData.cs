using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "GameData/ItemData")]
public sealed class ItemData : ScriptableObject, IItemData
{
	[SerializeField]
	private string m_ItemName;
	public string ItemName => m_ItemName;

	[SerializeField]
	private ItemBlueprint m_ItemBlueprint;
	public IItemBlueprint ItemBlueprint => m_ItemBlueprint;

	[SerializeField]
	private EntityCreationEventController m_ItemPrefab;

	private static GameObject m_CreatedViewRoot;
	public static GameObject CreatedViewRoot
	{
		get
		{
			if (m_CreatedViewRoot == null)
			{
				m_CreatedViewRoot = new GameObject("ItemViewsRoot");
			}

			return m_CreatedViewRoot;
		}
	}

	public IEntityCreationEventController CreateItemView()
	{
		var itemUnityView = GameObject.Instantiate(m_ItemPrefab, CreatedViewRoot.transform);
		return itemUnityView;
	}
}
