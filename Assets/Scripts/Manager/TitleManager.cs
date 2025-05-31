using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _hishScoreText;
    [SerializeField] private TextMeshProUGUI _lastScoreText;
    [SerializeField] private float _duration;
    void Start()
    {
        InputManager.Instance.Decide.Subscribe(x =>
        {
            if (x != 1) return;
            SceneLoadManager.Instance.LoadScene(ScneneType.InGame).Forget();
        }).AddTo(this);
        AlphaChanger().Forget();
        _hishScoreText.text = $"< HighScore : { PlayerPrefs.GetInt("HighScore", 0)} >";
        _lastScoreText.text = $"< LastScore : { PlayerPrefs.GetInt("LastScore", 0)} >";
    }
    private async UniTask AlphaChanger()
    {
        await DOVirtual.Float(0, 1, _duration, f =>
        {
            _canvasGroup.alpha = f;
        }).SetLoops(-1, LoopType.Yoyo).ToUniTask(cancellationToken:destroyCancellationToken);
    }
}
