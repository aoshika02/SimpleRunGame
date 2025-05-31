using Cysharp.Threading.Tasks;
public class BombObj : ItemObj
{
    PlayerController _playerController;
    ParticleObjectPool _particleObjectPool;
    void Start()
    {
        _playerController = PlayerController.Instance;
        _particleObjectPool=ParticleObjectPool.Instance;
    }
    protected override void GetItem()
    {
        _playerController.Damage();
        DeactivateAsync().Forget();
    }
    /// <summary>
    /// 爆発パーティクル表示
    /// </summary>
    /// <returns></returns>
    private async UniTask DeactivateAsync() 
    {
        gameObject.SetActive(false);
        var ps = _particleObjectPool.GetBombParticleSystem();
        ps.ParticleSystem.transform.position = transform.position;
        float duration = _particleObjectPool.GetAnimationCurveLength(ps.ParticleSystem);
        ps.ParticleSystem.Clear();
        ps.ParticleSystem.Play();
        await UniTask.WaitForSeconds(duration);
        _particleObjectPool.ReleaseParticleSystemData(ps,ItemType.Bomb);
    }
   
}