using System.Collections.Generic;
using UnityEngine;
public class Stage : MonoBehaviour
{
    [SerializeField] public StageItemDataHierarchy<Transform> PointData;
    private List<(GameObject obj, ItemType itemType)> _itemobjs = new List<(GameObject, ItemType)>();
    ItemObjectPool _itemObjectPool;
    private void Start()
    {
        _itemObjectPool = ItemObjectPool.Instance;
        StageManager.Instance.SetStage(this);
    }
    /// <summary>
    /// ステージの座標更新
    /// </summary>
    /// <param name="target"></param>
    /// <param name="itemDatas"></param>
    public void SetStagePos(Vector3 target, StageItemDataHierarchy<ItemType> itemDatas)
    {
        RemoveItemObjs();
        gameObject.transform.position = target;
        SetItemObj(itemDatas);
    }
    /// <summary>
    /// ステージにアイテムを割り当て
    /// </summary>
    /// <param name="itemDatas"></param>
    private void SetItemObj(StageItemDataHierarchy<ItemType> itemDatas)
    {
        GameObject obj = null;
        for (int k = 0; k < itemDatas.DataBases.Length; k++)
        {
            for (int j = 0; j < itemDatas.DataBases[k].DataBlocks.Length; j++)
            {
                for (int i = 0; i < itemDatas.DataBases[k].DataBlocks[j].Cell.Length; i++)
                {
                    switch (itemDatas.DataBases[k].DataBlocks[j].Cell[i])
                    {
                        case ItemType.Box:
                            obj = _itemObjectPool.GetBoxObj();
                            break;
                        case ItemType.Hart:
                            obj = _itemObjectPool.GetHartObj();
                            break;
                        case ItemType.Coin:
                            obj = _itemObjectPool.GetCoinObj();
                            break;
                        case ItemType.Bomb:
                            obj = _itemObjectPool.GetBombObj();
                            break;
                        default:
                            obj = null;
                            break;
                    }
                    var pos = PointData?.DataBases?[k]?.DataBlocks?[j]?.Cell?[i];
                    if (obj == null || pos == null) continue;
                    _itemobjs.Add((obj, itemDatas.DataBases[k].DataBlocks[j].Cell[i]));
                    obj.transform.position = PointData.DataBases[k].DataBlocks[j].Cell[i].position;
                }
            }
        }
    }
    /// <summary>
    /// 座標取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos()
    {
        return gameObject.transform.position;
    }
    /// <summary>
    /// 割り当てられたアイテムをすべて返還
    /// </summary>
    public void RemoveItemObjs()
    {
        if (_itemobjs.Count == 0) return;
        foreach (var itemObj in _itemobjs)
        {
            _itemObjectPool.ReleaseItem(itemObj.obj, itemObj.itemType);
        }
        _itemobjs.Clear();
    }
    public void Active()
    {
        gameObject.SetActive(true);
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
