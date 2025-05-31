using UnityEngine;

public class ItemObj : MonoBehaviour
{
    protected string _tagName = "Player";

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="pos"></param>
    public virtual void Init(Vector3 pos) 
    {
        transform.position = pos;
    }
    /// <summary>
    /// アイテム取得時の処理
    /// </summary>
    protected virtual void GetItem()
    {
        //継承先で処理をセット
    }
    public virtual void Activate() 
    {
        gameObject.SetActive(true);
    }
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_tagName))
        {
            GetItem();
        }
    }
}
