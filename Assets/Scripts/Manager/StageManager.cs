using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class StageManager : SingletonMonoBehaviour<StageManager>
{
    [SerializeField] private float _stageDepth;
    [SerializeField] private GameObject _stage;
    [SerializeField] private List<Stage> _stages = new List<Stage>();
    private float _playerPosZ = 0;
    private int _index = 0;

    private void Start()
    {
        PlayerController.Instance.PosZ.Subscribe(x =>
        {
            _playerPosZ = (int)x;
            if (_index != (int)(_playerPosZ / _stageDepth))
            {
                _index = (int)(_playerPosZ / _stageDepth);
                UpdateStagePos();
            }
        }).AddTo(this);
    }
    /// <summary>
    /// ステージの座標更新
    /// </summary>
    private void UpdateStagePos()
    {
        var data = SpawnItemDataManager.Instance.GetSpawnItemData();
        var obj = GetStage();
        obj.Active();
        obj.SetStagePos( new(0, 0, (_stages.Count + _index - 1) * _stageDepth), data);
    }
    /// <summary>
    /// ステージの取得
    /// </summary>
    /// <returns></returns>
    private Stage GetStage()
    {
        foreach (var stage in _stages)
        {
            if (_playerPosZ - stage.GetPos().z >= _stageDepth)
            {
                return stage;
            }
        }
        return CreateStage();
    }
    /// <summary>
    /// ステージ作成
    /// </summary>
    /// <returns></returns>
    private Stage CreateStage()
    {
        var obj = Instantiate(_stage).GetComponent<Stage>();
        obj.transform.SetParent(transform);
        obj.Deactive();
        _stages.Add(obj);
        return obj;
    }
    /// <summary>
    /// ステージ追加用
    /// </summary>
    /// <param name="stage"></param>
    public void SetStage(Stage stage) 
    {
        _stages.Add(stage);
    }
}
