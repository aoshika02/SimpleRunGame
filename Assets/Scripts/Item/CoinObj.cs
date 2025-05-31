using UnityEngine;

public class CoinObj : ItemObj
{
    [SerializeField] private int _point = 50;
    ScoreManager _scoreManager;
    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
    }
    protected override void GetItem() 
    {
        _scoreManager.AddBonusValue(_point);
        Deactivate();
    }
}