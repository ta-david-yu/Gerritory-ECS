using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemData
{
	public string ItemName { get; }
	public IItemBlueprint ItemBlueprint { get; }

	public IEntityCreationEventController CreateItemView();
}
