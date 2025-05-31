using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using TMPro;
public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private const int MIN_SCORE = 0;
    private const int MAX_SCORE = 99999999;
    private int _score = 0;
    private ReactiveProperty<int> _movedDistance = new ReactiveProperty<int>(0);
    private ReactiveProperty<int> _bonusValue = new ReactiveProperty<int>(0);
    private ReactiveProperty<int> _stackBonusValue = new ReactiveProperty<int>(0);
    private bool _isAct;
    [SerializeField] private int _rate = 10;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Start()
    {
        Observable.Merge(
            _movedDistance.AsObservable(),
            _bonusValue.AsObservable()
            ).Subscribe(_ => UpdateScore()).AddTo(this);

        _stackBonusValue.Subscribe(_ =>
        {
            AddScoreBonus();
        }).AddTo(this);
        PlayerController.Instance.PosZ.Subscribe(x => 
        {
            _movedDistance.Value = (int)x;
        }).AddTo(this);
    }
    /// <summary>
    /// スコア加算
    /// </summary>
    private async void AddScoreBonus()
    {
        if (_isAct) return;
        _isAct = true;
        while (_stackBonusValue.Value > 0)
        {
            _stackBonusValue.Value -= _rate;
            _bonusValue.Value += _rate;
            await UniTask.Yield();
        }
        _isAct = false;
    }
    /// <summary>
    /// スコア更新
    /// </summary>
    private void UpdateScore()
    {
        _score = _movedDistance.Value * 100 + _bonusValue.Value;

        if (_score < MIN_SCORE || _score > MAX_SCORE)
        {
            _score = Mathf.Clamp(_score, MIN_SCORE, MAX_SCORE);
        }
        _scoreText.text = $"Score:{_score}";
    }
    /// <summary>
    /// スコアスタック加算
    /// </summary>
    /// <param name="bonusValue"></param>
    public void AddBonusValue(int bonusValue)
    {
        _stackBonusValue.Value += bonusValue;
    }
    /// <summary>
    /// スコア取得
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return _score;
    }
}
