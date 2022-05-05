using UnityEngine.InputSystem;
using UnityEngine;

public abstract class BaseState 
{
    public bool isLock { get; protected set; }

    public virtual void OnEnter()
    {
        isLock = false;
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnStop()
    {

    }

    public virtual void OnActionCallback(InputAction.CallbackContext ctx)
    {

    }
}