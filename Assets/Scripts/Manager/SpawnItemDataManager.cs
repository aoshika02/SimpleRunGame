using UnityEngine;

public class SpawnItemDataManager : SingletonMonoBehaviour<SpawnItemDataManager>
{
    [SerializeField]private SpawnItemDatas _data;

    public StageItemDataHierarchy<ItemType> GetSpawnItemData() 
    {
        if (_data.StageItemDataList.Count==0) 
        {
            Debug.LogError("ステージデータがありません");
        }
        //ステージデータをランダムに引き渡し
        return _data.StageItemDataList[Random.Range(0, _data.StageItemDataList.Count)];
    }
}
