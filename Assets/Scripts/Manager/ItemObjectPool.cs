using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPool : SingletonMonoBehaviour<ItemObjectPool>
{
    private List<ItemObjBase> _boxObjs = new List<ItemObjBase>();
    private List<ItemObjBase> _hartObjs = new List<ItemObjBase>();
    private List<ItemObjBase> _coinObjs = new List<ItemObjBase>();
    private List<ItemObjBase> _bombObjs = new List<ItemObjBase>();
    [SerializeField] private GameObject _boxObj;
    [SerializeField] private GameObject _hartObj;
    [SerializeField] private GameObject _coinObj;
    [SerializeField] private GameObject _bombObj;

    [SerializeField] private Transform _boxObjParent;
    [SerializeField] private Transform _hartObjParent;
    [SerializeField] private Transform _coinObjParent;
    [SerializeField] private Transform _bombObjParent;
    [Serializable]
    public class ItemObjBase
    {
        public GameObject Obj;
        public bool IsUse;
        public ItemType ItemType;
    }
    #region アイテム取得
    public GameObject GetBoxObj() => GetItem(ItemType.Box);
    public GameObject GetHartObj() => GetItem(ItemType.Hart);
    public GameObject GetCoinObj() => GetItem(ItemType.Coin);
    public GameObject GetBombObj() => GetItem(ItemType.Bomb);
    public GameObject GetItem(ItemType itemType)
    {
        List<ItemObjBase> itemList = new List<ItemObjBase>();
        switch (itemType)
        {
            case ItemType.Box:
                itemList = _boxObjs;
                break;
            case ItemType.Hart:
                itemList = _hartObjs;
                break;
            case ItemType.Coin:
                itemList = _coinObjs;
                break;
            case ItemType.Bomb:
                itemList = _bombObjs;
                break;
        }
        ItemObjBase objData = null;
        foreach (var od in itemList)
        {
            if (od.IsUse == false)
            {
                objData = od;
                break;
            }
        }
        if (objData == null)
        {
            objData = CreateItemObj(itemType);
        }
        objData.IsUse = true;
        objData.Obj.SetActive(true);
        return objData.Obj;
    }
    #endregion
    /// <summary>
    /// アイテムオブジェクト生成
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    private ItemObjBase CreateItemObj(ItemType itemType)
    {
        GameObject item = null;
        Transform parent = null;
        ItemObjBase objData = null;
        List<ItemObjBase> objList = new List<ItemObjBase>();
        switch (itemType)
        {
            case ItemType.Box:
                item = _boxObj;
                parent = _boxObjParent;
                objList = _boxObjs;
                break;
            case ItemType.Hart:
                item = _hartObj;
                parent = _hartObjParent;
                objList = _hartObjs;
                break;
            case ItemType.Coin:
                item = _coinObj;
                parent = _coinObjParent;
                objList = _coinObjs;
                break;
            case ItemType.Bomb:
                item = _bombObj;
                parent = _bombObjParent;
                objList = _bombObjs;
                break;
        }
        objData = new()
        {
            Obj = Instantiate(item, parent),
            IsUse = false,
            ItemType = itemType
        };
        objList.Add(objData);
        return objData;
    }
    /// <summary>
    /// アイテム返還
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="itemType"></param>
    public void ReleaseItem(GameObject obj,ItemType itemType)
    {
        List<ItemObjBase> objList = new List<ItemObjBase>();
        switch (itemType)
        {
            case ItemType.Box:
                objList = _boxObjs;
                break;
            case ItemType.Hart:
                objList = _hartObjs;
                break;
            case ItemType.Coin:
                objList = _coinObjs;
                break;
            case ItemType.Bomb:
                objList = _bombObjs;
                break;
            default:
                break;
        }
        foreach (var od in objList)
        {
            if (od.Obj == obj)
            {
                od.IsUse = false;
                od.Obj.SetActive(false);
                break;
            }
        }
    }
}
public enum ItemType
{
    None,
    Box,
    Hart,
    Coin,
    Bomb,
}
