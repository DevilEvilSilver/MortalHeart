﻿using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class BaseEnemyController : SerializedMonoBehaviour
{
    public float baseMaxHealth;
    public float baseSpeed;
    public float idleTime;

    internal bool canTakeDamage = true;
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

    [Header("Animations")]
    public Animator animator;
    public AnimationClip[] allAnimationClips;
    [HideInInspector]
    public string[] allAnimations;

    [Header("State Controller")]
    public BaseEnemyState idleState;
    public BaseEnemyState deathState;

    [Space(10)]
    public BaseEnemyState[] listAttackState;

    protected FSMManager fsm;

#if UNITY_EDITOR

    protected void OnValidate()
    {
        LoadAnimations();

        if (idleState != null)
            idleState.actorControllerForEditor = this;
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
        if (deathState != null)
            deathState.OnInit(this);

        foreach (var state in listAttackState)
        {
            state.OnInit(this);
        }

        fsm = new FSMManager();
        fsm.ChangeState(idleState);
    }

    protected void OnEnable()
    {
        _health = baseMaxHealth;
        _currentIdleTime = idleTime;
    }

    protected void OnDisable()
    {
        fsm.StopCurrentState();
    }

    private void FixedUpdate()
    {
        fsm.OnFixedUpdate();
    }

    protected void Update()
    {
        if (_health <= 0f)
            fsm.ChangeState(deathState);

        if (fsm.currentState == idleState)
        {
            _currentIdleTime -= Time.deltaTime;
            if (_currentIdleTime < 0f)
            {
                CallActionAttack(Random.Range(0, listAttackState.Length));
            }
        }

        fsm.OnUpdate();
    }

    public void ChangeToIdle(float idleTime)
    {
        _currentIdleTime = idleTime;
        fsm.ChangeState(idleState, true);
    }
}
