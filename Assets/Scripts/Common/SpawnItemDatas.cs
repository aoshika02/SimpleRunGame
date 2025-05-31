using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnItemDatas", menuName = "ScriptableObjects/SpawnItemDatas")]
public class SpawnItemDatas :ScriptableObject
{
    public List<StageItemDataHierarchy<ItemType>> StageItemDataList;
}