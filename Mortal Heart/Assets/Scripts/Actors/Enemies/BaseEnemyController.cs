using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemyController : SerializedMonoBehaviour, IHeath
{
    public float baseMaxHealth;
    public float baseSpeed;
    public int moneyDrop;
    public float idleTime;
    public Vector2 idleOffset;

    internal bool canTakeDamage = true;
    internal bool isActive = false;
    protected float _health = 100f;

    protected float _currentIdleTime = 0f;

    //[Header("Collider")]
    private Rigidbody _rigidbody;
    public Rigidbody RigidBody
    { get
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

    [Header("UI")]
    public Canvas hpCanvas;
    public Image hpProgress;

    [Header("Animations")]
    public Animator animator;
    public AnimationClip[] allAnimationClips;
    [HideInInspector]
    public string[] allAnimations;

    [Header("State Controller")]
    public BaseEnemyState idleState;
    public BaseEnemyState hitState;
    public BaseEnemyState deathState;

    [Space(10)]
    public BaseEnemyState[] listAttackState;

    protected FSMManager fsm;
    public Action OnEnemyDeath { get; private set; }

#if UNITY_EDITOR

    protected void OnValidate()
    {
        LoadAnimations();

        if (idleState != null)
            idleState.actorControllerForEditor = this;
        if (hitState != null)
            hitState.actorControllerForEditor = this;
        if (deathState != null)
            deathState.actorControllerForEditor = this;

        foreach (var state in listAttackState)
        {
            state.actorControllerForEditor = this;
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
        _health = -1f;
        ChangeHp(_health, baseMaxHealth);
    }

#endif

    public void CallActionAttack(int index = 0)
    {
        if (listAttackState.Length == 0) return;

        fsm.ChangeState(listAttackState[index]);
    }

    protected void Awake()
    {
        if (idleState != null)
            idleState.OnInit(this);
        if (hitState != null)
            hitState.OnInit(this);
        if (deathState != null)
            deathState.OnInit(this);

        foreach (var state in listAttackState)
        {
            state.OnInit(this);
        }

        fsm = new FSMManager();
        fsm.ChangeState(idleState);
    }

    public void Init(Action deathAction)
    {
        OnEnemyDeath += deathAction;
        _currentIdleTime = idleTime + UnityEngine.Random.Range(idleOffset.x, idleOffset.y);
        isActive = false;
    }

    protected void OnEnable()
    {
        _health = baseMaxHealth;
        hpProgress.transform.DOScaleX(1f, 0f);

        _currentIdleTime = idleTime;
    }

    protected void OnDisable()
    {
        fsm.StopCurrentState();
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        if (!GameController.Instance.IsPlaying) return;

        fsm.OnFixedUpdate();
    }

    protected void Update()
    {
        if (!isActive) return;
        if (!GameController.Instance.IsPlaying) return;

        if (_health <= 0f && fsm.currentState != deathState)
        {
            fsm.ChangeState(deathState, true);
            InventorySystem.Instance.UpdatePlayerMoney(moneyDrop);
            return;
        }

        if (fsm.currentState == idleState)
        {
            _currentIdleTime -= Time.deltaTime;
            if (_currentIdleTime < 0f)
            {
                CallActionAttack(UnityEngine.Random.Range(0, listAttackState.Length));
            }
        }

        fsm.OnUpdate();
    }

    public void ChangeToIdle(float idleTime)
    {
        _currentIdleTime = idleTime + UnityEngine.Random.Range(idleOffset.x, idleOffset.y);
        fsm.ChangeState(idleState, true);
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            _health -= damage; 
            if (_health < 0f)
                _health = 0f;

            if (damage > baseMaxHealth / 10f)
            {
                fsm.ChangeState(hitState, true);
            }
            ChangeHp(_health, baseMaxHealth);
        }
    }

    public void ChangeHp(float value, float max, bool isAnim = true)
    {
        hpCanvas.gameObject.SetActive(true);
        hpProgress.transform.DOKill();
        hpProgress.transform.DOScaleX(value / max, isAnim ? 0.5f : 0f);
    }

    public void MoveTo(Vector3 pos, float speed)
    {
        Agent.SetDestination(pos);
        Agent.speed = speed;
        //transform.DOKill();
        //transform.DOLookAt(Agent.velocity, 0.1f);
    }

    public void Stop()
    {
        Agent.SetDestination(transform.position);
        Agent.speed = 0;
        //transform.DOKill();
        //transform.DOLookAt(Agent.velocity, 0.1f);
    }
}
