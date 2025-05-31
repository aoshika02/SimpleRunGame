using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private PlayerAction _playerAction;
    #region Move
    public void SetMoveStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.started += action;
    }
    public void SetMovePerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.performed += action;
    }
    public void SetMoveCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.canceled += action;
    }
    public void RemoveMoveStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.started -= action;
    }
    public void RemoveMovePerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.performed -= action;
    }
    public void RemoveMoveCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.canceled -= action;
    }
    #endregion

    #region Jump
    public void SetJumpStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Jump.started += action;
    }
    public void SetJumpPerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Jump.performed += action;
    }
    public void SetJumpCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Jump.canceled += action;
    }
    public void RemoveJumpStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Jump.started -= action;
    }
    public void RemoveJumpPerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Jump.performed -= action;
    }
    public void RemoveJumpCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Jump.canceled -= action;
    }
    #endregion

    public IReadOnlyReactiveProperty<float> Jump=>_jump;
    private ReactiveProperty<float> _jump = new ReactiveProperty<float>();

    public IReadOnlyReactiveProperty<Vector2> Move => _move;
    private ReactiveProperty<Vector2> _move = new ReactiveProperty<Vector2>();
    public IReadOnlyReactiveProperty<float> Decide => _decide;
    private ReactiveProperty<float> _decide = new ReactiveProperty<float>();

    protected override void Awake()
    {
        //インスタンスが無いならリターンする
        if (CheckInstance() == false)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
        _playerAction=new PlayerAction();
        _playerAction.Enable();

        _playerAction.Player.Move.started += context => { _move.Value = context.ReadValue<Vector2>(); };
        _playerAction.Player.Move.performed += context => { _move.Value = context.ReadValue<Vector2>(); };
        _playerAction.Player.Move.canceled += context => { _move.Value = context.ReadValue<Vector2>(); };

        _playerAction.Player.Jump.started += context => { _jump.Value = context.ReadValue<float>(); };
        _playerAction.Player.Jump.performed += context => { _jump.Value = context.ReadValue<float>(); };
        _playerAction.Player.Jump.canceled += context => { _jump.Value = context.ReadValue<float>(); };

        _playerAction.Player.Decide.started += context => { _decide.Value = context.ReadValue<float>(); };
        _playerAction.Player.Decide.performed += context => { _decide.Value = context.ReadValue<float>(); };
        _playerAction.Player.Decide.canceled += context => { _decide.Value = context.ReadValue<float>(); };
    }
}
