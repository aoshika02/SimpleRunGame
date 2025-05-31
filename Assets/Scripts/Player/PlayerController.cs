using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    //移動関係のベクター
    Vector3 _moveDirection = Vector3.zero;
    //移動上限用変数
    [SerializeField] private float _stageWidth;
    //InputSystemからの入力
    private float _directionX = 0f;
    private ReactiveProperty<float> _isJumpValue = new ReactiveProperty<float>(0);
    private float _jumpThreshold = 0.5f;
    //ジャンプフラグ
    private bool _isJump = false;
    //プレイヤーのパラメータ
    [SerializeField] private float _forwardSpeed = 0f;
    [SerializeField] private float _jumpSpeed = 0;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _gravity;

    private Rigidbody _rb;

    private ReactiveProperty<int> _lifeValue = new ReactiveProperty<int>(12);
    public IReactiveProperty<int> LifeValue => _lifeValue;

    private ReactiveProperty<float> _posZ = new ReactiveProperty<float>();
    public IReadOnlyReactiveProperty<float> PosZ => _posZ;
    private const int MIN_LIFE = 0;
    private const int MAX_LIFE = 12;

    private bool _isStan = false;

    InputManager _inputManager;
    PlayerAnimController _playerAnimController;

    private void Start()
    {
        _lifeValue.Value = MAX_LIFE;
        _rb = GetComponent<Rigidbody>();
        _inputManager = InputManager.Instance;
        _playerAnimController = PlayerAnimController.Instance;
        //コールバック関数の登録
        _inputManager.SetMoveStartedAction(PlayerMoveStart);
        _inputManager.SetMovePerformedAction(PlayerMoveStart);
        _inputManager.SetMoveCanceledAction(PlayerMoveStart);

        _inputManager.SetJumpStartedAction(PlayerJumpStart);
        _inputManager.SetJumpPerformedAction(PlayerJumpStart);
        _inputManager.SetJumpCanceledAction(PlayerJumpStart);

        _isJumpValue.Subscribe(x =>
        {
            if (_isJump == false && _isJumpValue.Value >= _jumpThreshold)
            {
                _isJump = true;
                _moveDirection.y = _jumpSpeed;
                _playerAnimController.AnimJump(_isJump);
            }
        }).AddTo(this);
        _lifeValue.Subscribe(x =>
        {
            LifeManager.Instance.UpdateLifeView();
        }).AddTo(this);
    }
    private void Update()
    {
        if (_isJump == false)
        {
            _moveDirection.x = NormalizeDirectionX(_directionX);
        }
        else
        {
            _moveDirection.x = 0;
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        _moveDirection.z = _forwardSpeed;

        if (_isStan == true)
        {
            _moveDirection.x = 0;
            _moveDirection.z = 0;
        }
        _rb.velocity = _moveDirection * _playerSpeed;
        _posZ.Value = transform.position.z;
    }
    /// <summary>
    /// X方向の移動ベクターの正規化
    /// </summary>
    /// <param name="directionX"></param>
    /// <returns></returns>
    private float NormalizeDirectionX(float directionX)
    {
        bool maxPosXFlag = _stageWidth * 2 / 3 <= transform.position.x && directionX > 0;
        bool minPosXFlag = -_stageWidth * 2 / 3 >= transform.position.x && directionX < 0;
        if (maxPosXFlag || minPosXFlag) return directionX = 0;
        return directionX;
    }
    /// <summary>
    /// 回復
    /// </summary>
    public void Heal()
    {
        if (MAX_LIFE > _lifeValue.Value)
        {
            _lifeValue.Value += 1;
        }
    }
    /// <summary>
    /// ライフ公開
    /// </summary>
    /// <returns></returns>
    public int GetLife()
    {
        return _lifeValue.Value;
    }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage()
    {
        if (MIN_LIFE <= _lifeValue.Value)
        {
            _lifeValue.Value -= 4;
            if (MIN_LIFE > _lifeValue.Value) _lifeValue.Value = MIN_LIFE;
            Stan().Forget();
        }
    }
    /// <summary>
    /// スタン処理
    /// </summary>
    /// <returns></returns>
    private async UniTask Stan()
    {
        _isStan = true;
        _playerAnimController.AnimStan(_isStan);
        float length = _playerAnimController.GetAnimationClipLength("Stumble Backwards", 2);
        await UniTask.WaitForSeconds(length);
        if (_lifeValue.Value == MIN_LIFE) 
        {
            int score = ScoreManager.Instance.GetScore();
            PlayerPrefs.SetInt("LastScore", score);
            if(score > PlayerPrefs.GetInt("HighScore", 0)) 
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            SceneLoadManager.Instance.LoadScene(ScneneType.Title).Forget();
            return;
        }
        _isStan = false;
        _playerAnimController.AnimStan(_isStan);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ParamConsts.GROUND))
        {
            _moveDirection.y = 0;
            _isJump = false;
            _playerAnimController.AnimJump(_isJump);
        }
    }
    #region 登録する関数
    private void PlayerMoveStart(InputAction.CallbackContext context)
    {
        _directionX = context.ReadValue<Vector2>().x;
    }
    private void PlayerMovePerformed(InputAction.CallbackContext context)
    {
        _directionX = context.ReadValue<Vector2>().x;
    }
    private void PlayerMoveCanceled(InputAction.CallbackContext context)
    {
        _directionX = 0;
    }
    private void PlayerJumpStart(InputAction.CallbackContext context)
    {
        _isJumpValue.Value = context.ReadValue<float>();
    }
    private void PlayerJumpPerformed(InputAction.CallbackContext context)
    {
        _isJumpValue.Value = context.ReadValue<float>();
    }
    private void PlayerJumpCanceled(InputAction.CallbackContext context)
    {
        _isJumpValue.Value = 0;
    }
    #endregion
    private void OnDestroy()
    {
        //コールバック関数の解除
        _inputManager.RemoveMoveStartedAction(PlayerMoveStart);
        _inputManager.RemoveMovePerformedAction(PlayerMovePerformed);
        _inputManager.RemoveMoveCanceledAction(PlayerMoveCanceled);

        _inputManager.RemoveJumpStartedAction(PlayerJumpStart);
        _inputManager.RemoveJumpPerformedAction(PlayerJumpPerformed);
        _inputManager.RemoveJumpCanceledAction(PlayerJumpCanceled);
    }
}
