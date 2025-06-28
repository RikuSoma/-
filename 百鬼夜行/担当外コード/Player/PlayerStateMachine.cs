using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    Player player;
    private InputSystem_PlayerActions inputActions;

    private List<IPlayerState> activeStates = new List<IPlayerState>(); // 現在のアクティブ状態
    private List<IPlayerState> statesToActivate = new List<IPlayerState>(); // 遅延登録リスト

    private Vector2 moveInput;
    private Vector2 shootDirectionInput;
    private bool jumpPressed, jumpHeld, jumpReleased;
    private bool isCrouching;
    private bool dashPressed;
    private bool fireHeld;
    private bool switchModePressed;
    private bool skillPressed;

    [Header("入力")]
    public float HorizontalInput { get; private set; }
    public bool JumpPressed => jumpPressed;
    public bool JumpHeld => jumpHeld;
    public bool JumpReleased => jumpReleased;
    public bool DashPressed => dashPressed;
    public bool IsCrouching => isCrouching;
    public Vector2 ShootDirectionInput => shootDirectionInput;
    public bool FireHeld => fireHeld;
    public bool SwitchModePressed => switchModePressed;
    public bool SkillPressed => skillPressed;

    public void Init(Player owner)
    {
        player = owner;

        // 必要な初期ステートをここで登録
        ActivateState(new PlayerMoving());
        ActivateState(new NohMaskState());
        ActivateState(new NohMaskSkill());

        inputActions = new InputSystem_PlayerActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.started += ctx => jumpPressed = true;
        inputActions.Player.Jump.performed += ctx => jumpHeld = true;
        inputActions.Player.Jump.canceled += ctx =>
        {
            jumpHeld = false;
            jumpReleased = true;
        };
        inputActions.Player.Crouch.performed += ctx => isCrouching = true;
        inputActions.Player.Crouch.canceled += ctx => isCrouching = false;
        inputActions.Player.Dash.performed += ctx => dashPressed = true;
        inputActions.Player.ShootDirection.performed += ctx => shootDirectionInput = ctx.ReadValue<Vector2>();
        inputActions.Player.ShootDirection.canceled += ctx => shootDirectionInput = Vector2.zero;
        inputActions.Player.Fire.performed += ctx => fireHeld = true;
        inputActions.Player.Fire.canceled += ctx => fireHeld = false;
        inputActions.Player.SwitchMode.performed += ctx => switchModePressed = true;
        inputActions.Player.Skill.performed += ctx => skillPressed = true;

        inputActions.Player.Enable();
    }

    public void StateSetting()
    {
        HandleInput();

        // 遅延ステートの登録
        for (int i = 0; i < statesToActivate.Count; i++)
        {
            var state = statesToActivate[i];
            if (!activeStates.Contains(state))
            {
                activeStates.Add(state);
                state.Init(player, this);
            }
        }
        statesToActivate.Clear();

        // アクティブステートの処理
        for (int i = 0; i < activeStates.Count; i++)
        {
            var state = activeStates[i];
            state.HandleInput();
            state.Update();
        }

        // 入力フラグのリセット
        jumpPressed = false;
        jumpReleased = false;
        dashPressed = false;
        switchModePressed = false;
        skillPressed = false;
    }

    private void HandleInput()
    {
        HorizontalInput = moveInput.x;

        // ジャンプ
        if (jumpPressed && player.IsGrounded() && !IsStateActive<PlayerJumping>()) { ActivateState(new PlayerJumping()); }

        // ダッシュ
        if (HorizontalInput != 0f && dashPressed && !IsStateActive<PlayerDash>()) { ActivateState(new PlayerDash()); }

        // しゃがみ
        if (isCrouching && player.IsGrounded() && !IsStateActive<PlayerCrouching>()) { ActivateState(new PlayerCrouching()); }

        // しゃがみ解除
        if (!isCrouching && IsStateActive<PlayerCrouching>()) { DeactivateState(GetState<PlayerCrouching>()); }

        if (skillPressed && !IsStateActive<BulletSkill>()) { ActivateState(new BulletSkill()); }
    }

    public void ActivateState(IPlayerState state)
    {
        // 同じ型のステートがすでにactiveか、遅延登録されているなら登録しない
        var stateType = state.GetType();
        bool isDuplicate = activeStates.Exists(s => s.GetType() == stateType) || statesToActivate.Exists(s => s.GetType() == stateType);

        if (!isDuplicate)
        {
            statesToActivate.Add(state);
            Debug.Log($"[StateMachine] State added: {stateType.Name}");
        }
    }

    public void DeactivateState(IPlayerState state)
    {
        if (activeStates.Contains(state))
        {
            state.Remove();
            activeStates.Remove(state);
        }
    }

    public bool IsStateActive<T>() where T : IPlayerState
    {
        foreach (var state in activeStates)
        {
            if (state is T) { return true; }
        }
        return false;
    }

    public T GetState<T>() where T : IPlayerState
    {
        foreach (var state in activeStates)
        {
            if (state is T tState) { return tState; }
        }
        return default;
    }
    private void OnDisable()
    {
        inputActions?.Player.Disable();
    }

}
