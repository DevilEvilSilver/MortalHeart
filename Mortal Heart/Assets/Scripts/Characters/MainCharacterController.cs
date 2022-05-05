using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class MainCharacterController : SerializedMonoBehaviour
{
    [Header("Info")]
    public float baseMaxHealth;
    public float baseSpeed;

    internal bool canTakeDamage = true;
    protected float _health = 100f;

    [Header("Collider")]
    public Rigidbody rigidbody;

    [Header("Animations")]
    public Animation animator;
    public AnimationClip[] allAnimationClips;
    [HideInInspector]
    public string[] allAnimations;
    
    [Header("State Controller")]
    public BaseIdleState idleState;
    public BaseDeathState deathState;

    [Space(10)]
    public BaseMoveState moveState;
    public BaseDodgeState dodgeState;
    public BaseAttackState attackState_A;
    public BaseAttackState attackState_B;
    public BaseAttackState attackState_C;

    protected FSMManager fsm;

    protected InputMap inputMap;
    internal InputAction moveAction;
    internal InputAction dodgeAction;
    internal InputAction attackAction_A;
    internal InputAction attackAction_B;
    internal InputAction attackAction_C;

    private InputAction.CallbackContext _moveContext;
    internal InputAction chainAttackAction;

#if UNITY_EDITOR

    private void OnValidate()
    {
        LoadAnimations();

        if (idleState != null)
            idleState.actorControllerForEditor = this;
        if (deathState != null)
            deathState.actorControllerForEditor = this;

        if (moveState != null)
            moveState.actorControllerForEditor = this;
        if (dodgeState != null)
            dodgeState.actorControllerForEditor = this;
        if (attackState_A != null)
            attackState_A.actorControllerForEditor = this;
        if (attackState_B != null)
            attackState_B.actorControllerForEditor = this;
        if (attackState_C != null)
            attackState_C.actorControllerForEditor = this;
    }

    //[Button("Load Animations")]
    public void LoadAnimations()
    {
        allAnimations = new string[allAnimationClips.Length];
        for (int i = 0; i < allAnimationClips.Length; i++)
        {
            allAnimations[i] = allAnimationClips[i].name;
        }
    }

    [Button("Kill")]
    public void Kill()
    {
        _health = -1f;
    }

#endif

    protected void Awake()
    {
        inputMap = new InputMap();
        moveAction = inputMap.Combat.Move;
        dodgeAction = inputMap.Combat.Dodge;
        attackAction_A = inputMap.Combat.Attack_A;
        attackAction_B = inputMap.Combat.Attack_B;
        attackAction_C = inputMap.Combat.Attack_C;

        if (idleState != null)
            idleState.OnInit(this);
        if (deathState != null)
            deathState.OnInit(this);

        if (moveState != null)
            moveState.OnInit(this);
        if (dodgeState != null)
            dodgeState.OnInit(this);
        if (attackState_A != null)
            attackState_A.OnInit(this);
        if (attackState_B != null)
            attackState_B.OnInit(this);
        if (attackState_C != null)
            attackState_C.OnInit(this);

        fsm = new FSMManager();
        fsm.ChangeState(idleState);
    }

    protected void OnEnable()
    {
        _health = baseMaxHealth;

        // move
        moveAction.Enable();
        moveAction.performed += ctx =>
        {
            // state is changed in fixed update
            _moveContext = ctx;
        };

        moveAction.canceled += ctx =>
        {
            if (fsm.currentState == moveState)
                ChangeToIdle();
        };

        // dodge
        dodgeAction.Enable();
        dodgeAction.performed += ctx =>
        {
            fsm.ChangeState(dodgeState);
        };

        // attack
        attackAction_A.Enable();
        attackAction_A.performed += ctx =>
        {
            fsm.ChangeState(attackState_A);
            ActionCallback(ctx, attackState_A);
        };
        attackAction_B.Enable();
        attackAction_B.performed += ctx =>
        {
            fsm.ChangeState(attackState_B);
            ActionCallback(ctx, attackState_B);
        };
        attackAction_C.Enable();
        attackAction_C.performed += ctx =>
        {
            fsm.ChangeState(attackState_C);
            ActionCallback(ctx, attackState_C);
        };
    }

    protected void OnDisable()
    {
        fsm.StopCurrentState();

        moveAction.Disable();
        dodgeAction.Disable();
        attackAction_A.Disable();
        attackAction_B.Disable();
        attackAction_C.Disable();
    }

    private void FixedUpdate()
    {
        if (moveAction.IsPressed())
        {
            fsm.ChangeState(moveState);
            ActionCallback(_moveContext, moveState);
            // for changing dodge direction 
            ActionCallback(_moveContext, dodgeState);
        }

        fsm.OnFixedUpdate();
    }

    protected void Update()
    {
        if (_health <= 0f)
            fsm.ChangeState(deathState);

        fsm.OnUpdate();
    }

    public void GoToNextState(bool isByPassLock = false)
    {
        fsm.GoToNextState(isByPassLock);
    }

    public void ChangeToIdle()
    {
        fsm.ChangeState(idleState, true);
    }

    public void CancelAttackToDodge(BaseState nextState, InputAction action)
    {
        fsm.ChangeState(dodgeState, nextState, true);
        chainAttackAction = action;
    }

    private void ActionCallback(InputAction.CallbackContext ctx, BaseState state)
    {
        if (fsm.currentState == state)
            state.OnActionCallback(ctx);
    }
}
