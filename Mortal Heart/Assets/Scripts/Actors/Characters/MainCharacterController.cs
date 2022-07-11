using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class MainCharacterController : SerializedMonoBehaviour, IHeath
{
    public PlayerData playerData;

    protected bool canTakeDamage = true;
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

    private NavMeshAgent _agent;
    public NavMeshAgent Agent
    {
        get
        {
            if (_agent == null)
                _agent = GetComponent<NavMeshAgent>();
            return _agent;
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
    public BaseCharacterState hitState;
    public BaseCharacterState useItemState;
    public BaseCharacterState pickUpState;
    public BaseCharacterState[] skillSet_A;
    public BaseCharacterState[] skillSet_B;
    public BaseCharacterState[] skillSet_C;

    protected FSMManager fsm;

    internal InputAction moveAction;
    internal InputAction dodgeAction;
    internal InputAction useItemAction;

    internal InputAction pickUpAction;

    internal InputAction attackAction_A;
    internal InputAction attackAction_B;
    internal InputAction attackAction_C;

    private InputAction.CallbackContext _moveContext;
    internal InputAction chainAttackAction;
    internal int skillSetIndex;

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
        if (hitState != null)
            hitState.actorControllerForEditor = this;
        if (useItemState != null)
            useItemState.actorControllerForEditor = this;
        if (pickUpState != null)
            pickUpState.actorControllerForEditor = this;
        foreach (var attack in skillSet_A)
        {
            if (attack != null)
                attack.actorControllerForEditor = this;
        }
        foreach (var attack in skillSet_B)
        {
            if (attack != null)
                attack.actorControllerForEditor = this;
        }
        foreach (var attack in skillSet_C)
        {
            if (attack != null)
                attack.actorControllerForEditor = this;
        }
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
        useItemAction = InputManager.Instance.useItemAction;
        pickUpAction = InputManager.Instance.pickUpAction;
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
        if (hitState != null)
            hitState.OnInit(this);
        if (useItemState != null)
            useItemState.OnInit(this);
        if (pickUpState != null)
            pickUpState.OnInit(this);
        foreach (var attack in skillSet_A)
        {
            if (attack != null)
                attack.OnInit(this);
        }
        foreach (var attack in skillSet_B)
        {
            if (attack != null)
                attack.OnInit(this);
        }
        foreach (var attack in skillSet_C)
        {
            if (attack != null)
                attack.OnInit(this);
        }

        fsm = new FSMManager();
        fsm.ChangeState(idleState);
    }

    protected void OnEnable()
    {
        baseMaxHealth = GameController.Instance.currSaveData.baseMaxHealth;
        ChangeHealth(0f, false);
        skillSetIndex = 0;

        // move
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        // dodge
        dodgeAction.performed += OnDodge;
        // useItem
        useItemAction.performed += OnUseItem;
        // pickUp
        pickUpAction.performed += OnPickUp;
        // attack
        attackAction_A.performed += OnAttackA;
        attackAction_B.performed += OnAttackB;
        attackAction_C.performed += OnAttackC;

        OnActive();
    }

    protected void OnDisable()
    {
        fsm.StopCurrentState();

        // move
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        // dodge
        dodgeAction.performed -= OnDodge;
        // useItem
        useItemAction.performed -= OnUseItem;
        // pickUp
        pickUpAction.performed -= OnPickUp;
        // attack
        attackAction_A.performed -= OnAttackA;
        attackAction_B.performed -= OnAttackB;
        attackAction_C.performed -= OnAttackC;

        OnDeactive();
    }

    protected virtual void OnActive()
    {

    }

    protected virtual void OnDeactive()
    {

    }

    #region InputCallback
    protected virtual void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        // state is changed in fixed update
        _moveContext = ctx;
    }

    protected virtual void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        //animator.SetBool(BOOL_MOVE, false);
        if (fsm.currentState == moveState)
            ChangeToIdle();
    }

    protected virtual void OnDodge(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_DODGE);
        fsm.ChangeState(dodgeState);
    }

    protected virtual void OnUseItem(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_DODGE);
        fsm.ChangeState(useItemState);
    }

    protected virtual void OnPickUp(InputAction.CallbackContext ctx)
    {
        //animator.SetTrigger(TRIGGER_DODGE);
        fsm.ChangeState(pickUpState);
    }

    protected virtual void OnAttackA(InputAction.CallbackContext ctx)
    {
        if (skillSetIndex >= skillSet_A.Length)
        {
            skillSetIndex = 0;
        }

        //animator.SetTrigger(TRIGGER_ATTACK_A);
        fsm.ChangeState(skillSet_A[skillSetIndex]);
        ActionCallback(ctx, skillSet_A[skillSetIndex]);
    }

    protected virtual void OnAttackB(InputAction.CallbackContext ctx)
    {
        if (skillSetIndex >= skillSet_B.Length)
        {
            skillSetIndex = 0;
        }

        //animator.SetTrigger(TRIGGER_ATTACK_B);
        fsm.ChangeState(skillSet_B[skillSetIndex]);
        ActionCallback(ctx, skillSet_B[skillSetIndex]);
    }

    protected virtual void OnAttackC(InputAction.CallbackContext ctx)
    {
        if (skillSetIndex >= skillSet_C.Length)
        {
            skillSetIndex = 0;
        }

        //animator.SetTrigger(TRIGGER_ATTACK_C);
        fsm.ChangeState(skillSet_C[skillSetIndex]);
        ActionCallback(ctx, skillSet_C[skillSetIndex]);
    }
    #endregion

    private void FixedUpdate()
    {
        if (!GameController.Instance.IsPlaying) return;

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

    protected virtual void Update()
    {
        if (!GameController.Instance.IsPlaying) return;

        if (playerData.Hp <= 0f)
            fsm.ChangeState(deathState, true);

        fsm.OnUpdate();
    }

    public void GoToNextState(InputAction.CallbackContext ctx, bool isByPassLock = false)
    {
        if (fsm.nextState != null)
        {
            fsm.GoToNextState(isByPassLock);
            fsm.currentState.OnActionCallback(ctx);
        }    
    }

    public void ChangeToIdle()
    {
        if (moveAction.IsPressed())
        {
            //animator.SetBool(BOOL_MOVE, true);

            fsm.ChangeState(moveState, true);
            ActionCallback(_moveContext, moveState);
            // for changing dodge direction 
            ActionCallback(_moveContext, dodgeState);
            return;
        }

        fsm.ChangeState(idleState, true);
    }

    public void CancelAttackToDodge(InputAction.CallbackContext ctx)
    {
        var state = GetNextCombo(ctx.action);

        if (state != null)
        {
            (dodgeState as BaseDodgeState).comboCtx = ctx;
            chainAttackAction = ctx.action;
            fsm.ChangeState(dodgeState, state, true);
        }
    }

    public void ChainNextCombo(InputAction.CallbackContext ctx)
    {
        var state = GetNextCombo(ctx.action);

        if (state != null)
        {
            fsm.ChangeState(state, true);
            ActionCallback(ctx, state);
        }
    }

    public BaseState GetNextCombo(InputAction action)
    {
        skillSetIndex++;
        if (attackAction_A == action)
        {
            if (skillSetIndex >= skillSet_A.Length)
            {
                skillSetIndex = 0;
                return null;
            }

            return skillSet_A[skillSetIndex];
        }
        if (attackAction_B == action)
        {
            if (skillSetIndex >= skillSet_B.Length)
            {
                skillSetIndex = 0;
                return null;
            }

            return skillSet_B[skillSetIndex];
        }
        if (attackAction_C == action)
        {
            if (skillSetIndex >= skillSet_C.Length)
            {
                skillSetIndex = 0;
                return null;
            }

            return skillSet_C[skillSetIndex];
        }
        return null;
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

    public virtual void ToggleInvulnerable(bool isInvulnerable)
    {
        canTakeDamage = !isInvulnerable;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            ChangeHealth(-damage);
            if (damage > 0f)
            {
                fsm.ChangeState(hitState, true);
            }
        }
    }

    public virtual void OnDealDamage(float damage)
    {

    }
}
