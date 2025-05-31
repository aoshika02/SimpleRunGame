using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
{
    private bool _isLoad = false;
    protected override void Awake()
    {
        //インスタンスが無いならリターンする
        if (CheckInstance() == false)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// シーンの読み込み
    /// </summary>
    /// <param name="scneneType"></param>
    /// <returns></returns>
    public async UniTask LoadScene(ScneneType scneneType)
    {
        if (_isLoad) return;
        _isLoad = true;
        await SceneManager.LoadSceneAsync((int)scneneType);
        _isLoad = false;
    }
}
public enum ScneneType
{
    Title = 0,
    InGame = 1

}
