using System.Collections.Generic;
using UnityEngine;

public class ParticleObjectPool : SingletonMonoBehaviour<ParticleObjectPool>
{
    [SerializeField] private ParticleSystem _bombParticle;
    private List<ParticleSystemData> _bombParticles = new List<ParticleSystemData>();
    /// <summary>
    /// 爆弾のパーティクル取得
    /// </summary>
    /// <returns></returns>
    public ParticleSystemData GetBombParticleSystem() => GetParticleSystem(ItemType.Bomb);
    /// <summary>
    /// パーティクルの取得
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    private ParticleSystemData GetParticleSystem(ItemType itemType)
    {
        List<ParticleSystemData> particleSystemDatas = new List<ParticleSystemData>();
        ParticleSystemData particleSystem = null;
        switch (itemType)
        {
            case ItemType.Bomb:
                particleSystemDatas = _bombParticles;
                break;
            default:
                Debug.LogError($"想定していない{nameof(ItemType)} : {itemType}です");
                break;
        }
        foreach (var ps in particleSystemDatas)
        {
            if (ps.IsUse == false)
            {
                particleSystem = ps;
            }
        }
        if (particleSystem == null)
        {
            particleSystem = CreateParticleSystemData(itemType);
        }
        particleSystem.IsUse = true;
        return particleSystem;
    }
    /// <summary>
    /// パーティクルの作成
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    private ParticleSystemData CreateParticleSystemData(ItemType itemType)
    {
        List<ParticleSystemData> particleSystemDatas = new List<ParticleSystemData>();
        ParticleSystem particleSystem = null;
        switch (itemType)
        {
            case ItemType.Bomb:
                particleSystemDatas = _bombParticles;
                particleSystem = _bombParticle;
                break;
            default:
                Debug.LogError($"想定していない{nameof(ItemType)} : {itemType}です");
                break;
        }
        var particleSystemData = new ParticleSystemData
        {
            ParticleSystem = Instantiate(particleSystem,transform),
            IsUse = true
        };
        particleSystemDatas.Add(particleSystemData);
        return particleSystemData;
    }
    /// <summary>
    /// パーティクルの返還
    /// </summary>
    /// <param name="particleSystemData"></param>
    /// <param name="itemType"></param>
    public void ReleaseParticleSystemData(ParticleSystemData particleSystemData, ItemType itemType)
    {
        List<ParticleSystemData> particleSystemDatas = new List<ParticleSystemData>();
        switch (itemType)
        {
            case ItemType.Bomb:
                particleSystemDatas = _bombParticles;
                break;
            default:
                Debug.LogError($"想定していない{nameof(ItemType)} : {itemType}です");
                break;
        }
        foreach (var psd in particleSystemDatas)
        {
            if (particleSystemData == psd)
            {
                psd.IsUse = false;
                break;
            }
        }
    }
    /// <summary>
    /// パーティクルのアニメーションの長さ取得
    /// </summary>
    /// <param name="particleSystem"></param>
    /// <returns></returns>
    public float GetAnimationCurveLength(ParticleSystem particleSystem)
    {
        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        if (sizeOverLifetime.enabled)
        {
            ParticleSystem.MinMaxCurve sizeCurve = sizeOverLifetime.size;
            AnimationCurve curve = null;
            switch (sizeCurve.mode)
            {
                case ParticleSystemCurveMode.Curve:
                    curve = sizeCurve.curve;
                    break;
            }
            if (curve != null && curve.length > 0)
            {
                return curve.keys[curve.length - 1].time;
            }
        }
        return 0;
    }
}
public class ParticleSystemData
{
    public ParticleSystem ParticleSystem;
    public bool IsUse;
}
