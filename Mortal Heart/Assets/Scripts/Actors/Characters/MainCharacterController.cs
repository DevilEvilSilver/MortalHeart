using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class MainCharacterController : SerializedMonoBehaviour, IHeath
{
    public PlayerData playerData;

    internal bool canTakeDamage = true;
    protected float baseMaxHealth;

    //[Header("Collider")]
    private Rigidbody _rigidbody;
    public Rigidbody RigidBody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }

    [Header("Animations")]
    public Animator animator;
    public AnimationClip[] allAnimationClips;
    [HideInInspector]
    public string[] allAnimations;

    [Header("State Controller")]
    public BaseCharacterState idleState;
    public BaseCharacterState deathState;

    [Space(10)]
    public BaseCharacterState moveState;
    public BaseCharacterState dodgeState;
    public BaseCharacterState attackState_A;
    public BaseCharacterState attackState_B;
    public BaseCharacterState attackState_C;

    protected FSMManager fsm;

    internal InputAction moveAction;
    internal InputAction dodgeAction;
    internal InputAction attackAction_A;
    internal InputAction attackAction_B;
    internal InputAction attackAction_C;

    private InputAction.CallbackContext _moveContext;
    internal InputAction chainAttackAction;

    private const string BOOL_MOVE = "move";
    private const string TRIGGER_DODGE = "dodge";
    private const string TRIGGER_ATTACK_A = "attack_a";
    private const string TRIGGER_ATTACK_B = "attack_b";
    private const string TRIGGER_ATTACK_C = "attack_c";
    private const string TRIGGER_DEATH = "death";

#if UNITY_EDITOR

    protected void OnValidate()
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

    [Button("Load Animations")]
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
        playerData.Hp = -1f;
    }

#endif

    protected void Awake()
    {
        moveAction = InputManager.Instance.moveAction;
        dodgeAction = InputManager.Instance.dodgeAction;
        attackAction_A = InputManager.Instance.attackAction_A;
        attackAction_B = InputManager.Instance.attackAction_B;
        attackAction_C = InputManager.Instance.attackAction_C;

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
        baseMaxHealth = GameController.Instance.currSaveData.baseMaxHealth;
        ChangeHealth(0f, false);

        // move
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        // dodge
        dodgeAction.performed += OnDodge;
        // attack
        attackAction_A.performed += OnAttackA;
        attackAction_B.performed += OnAttackB;
        attackAction_C.performed += OnAttackC;
    }

    protected void OnDisable()
    {
        fsm.StopCurrentState();

        // move
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        // dodge
        dodgeAction.performed -= OnDodge;
        // attack
        attackAction_A.performed -= OnAttackA;
        attackAction_B.performed -= OnAttackB;
        attackAction_C.performed -= OnAttackC;
    }

    #region InputCallback
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        // state is changed in fixed update
        _moveContext = ctx;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        //animator.SetBool(BOOL_MOVE, false);
        if (fsm.currentState == moveState)
            ChangeToIdle();
    }

    private void OnDodge(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_DODGE);
        fsm.ChangeState(dodgeState);
    }

    private void OnAttackA(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_ATTACK_A);
        fsm.ChangeState(attackState_A);
        ActionCallback(ctx, attackState_A);
    }

    private void OnAttackB(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_ATTACK_B);
        fsm.ChangeState(attackState_B);
        ActionCallback(ctx, attackState_B);
    }

    private void OnAttackC(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_ATTACK_C);
        fsm.ChangeState(attackState_C);
        ActionCallback(ctx, attackState_C);
    }
    #endregion

    private void FixedUpdate()
    {
        if (moveAction.IsPressed())
        {
            //animator.SetBool(BOOL_MOVE, true);

            fsm.ChangeState(moveState);
            ActionCallback(_moveContext, moveState);
            // for changing dodge direction 
            ActionCallback(_moveContext, dodgeState);
        }

        fsm.OnFixedUpdate();
    }

    protected void Update()
    {
        if (playerData.Hp <= 0f)
            fsm.ChangeState(deathState, true);

        fsm.OnUpdate();
    }

    public void GoToNextState(bool isByPassLock = false)
    {
        fsm.GoToNextState(isByPassLock);
    }

    public void ChangeToIdle()
    {
        if (moveAction.IsPressed())
        {
            animator.SetBool(BOOL_MOVE, true);

            fsm.ChangeState(moveState);
            ActionCallback(_moveContext, moveState);
            // for changing dodge direction 
            ActionCallback(_moveContext, dodgeState);
            return;
        }

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

    public void ChangeHealth(float change, bool isAnim = true)
    {
        if (fsm.currentState == deathState) return;

        playerData.Hp += change;
        if (playerData.Hp > baseMaxHealth)
            playerData.Hp = baseMaxHealth;
        else if (playerData.Hp < 0f)
            playerData.Hp = 0f;
        GameplayScreen.Instance.OnHPChange(playerData.Hp, baseMaxHealth, isAnim);
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
            ChangeHealth(-damage);
    }
}
