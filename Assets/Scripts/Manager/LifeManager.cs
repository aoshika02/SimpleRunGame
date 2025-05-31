using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : SingletonMonoBehaviour<LifeManager>
{
    [SerializeField] private Color _mainColor = Color.white;
    [SerializeField] private Color _bgColor = Color.white;
    [SerializeField] private List<HartImageData> _hartImageDatas = new List<HartImageData>();
    private LifeViewTaskData[] _taskDatas = new LifeViewTaskData[3];
    private bool _isCallOnce = false;
    [Serializable]
    public class HartImageData
    {
        public Image HartImage;
        public Image BGImage;
    }
    public class LifeViewTaskData
    {
        public Func<UniTask> Task;
        public CancelToken CancelToken;
        public bool IsRunning;
    }
    void Start()
    {
        Init();
    }
    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        foreach (var hartImageData in _hartImageDatas)
        {
            hartImageData.HartImage.color = _mainColor;
            hartImageData.BGImage.color = _bgColor;
        }
    }
    /// <summary>
    /// ライフの表示更新
    /// </summary>
    public void UpdateLifeView()
    {
        if (_isCallOnce == false)
        {
            for (int i = 0; i < _taskDatas.Length; i++)
            {
                _taskDatas[i] = new LifeViewTaskData
                {
                    Task = null, // 後で設定
                    CancelToken = new CancelToken(),
                    IsRunning = false
                };
            }
            _isCallOnce = true;
        }
        for (int i = 0; i < _hartImageDatas.Count; i++)
        {
            int index = i;
            if (_taskDatas[index].IsRunning == true)
            {
                _taskDatas[index].CancelToken.Cancel();
                _taskDatas[index].CancelToken.ReCreate();
            }
            _taskDatas[index].Task = async () => await ValueChangeAsync(
                    _hartImageDatas[index].BGImage,
                    _hartImageDatas[index].BGImage.fillAmount,
                    (float)(1 - Mathf.Clamp01((PlayerController.Instance.LifeValue.Value - 4f * index) / 4f)),
                    0.25f,
                    _taskDatas[index].CancelToken.GetToken()
                    );
            _taskDatas[index].IsRunning = true;
            _taskDatas[index].Task.Invoke().ContinueWith(() => 
            {
                _taskDatas[index].IsRunning = false;
            }).Forget();
        }
    }
    private async UniTask ValueChangeAsync(Image image, float start, float end, float duration, CancellationToken ct)
    {
        await DOVirtual.Float(start, end, duration, f =>
        {
            image.fillAmount = f;
        }).ToUniTask(cancellationToken: ct);
    }
}
