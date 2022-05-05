using UnityEngine.InputSystem;

public class FSMManager
{
    public BaseState currentState { get; private set; }
    public BaseState nextState { get; set; }
    public BaseState previousState { get; private set; }

    public FSMManager()
    {

    }

    public void OnUpdate()
    {
        if (currentState == null) return;

        currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        if (currentState == null) return;

        currentState.OnFixedUpdate();
    }

    public void ChangeState(BaseState newState, bool isByPassLock = false)
    {
        if (newState == currentState) return;

        if (currentState != null)
        {
            if (currentState.isLock && !isByPassLock) return;

            currentState.OnExit();
            previousState = currentState;
        }

        currentState = newState;
        currentState.OnEnter();
    }

    public void ChangeState(BaseState newState, BaseState _nextState, bool isByPassLock = false)
    {
        if (newState == currentState) return;
        nextState = _nextState;

        ChangeState(newState, isByPassLock);
    }

    public void StopCurrentState()
    {
        if (currentState != null)
        {
            currentState.OnStop();
        }
    }

    public void GoToNextState(bool isByPassLock = false)
    {
        if (nextState == null) return;
        ChangeState(nextState, isByPassLock);
    }

    public void GoToPreviouseState()
    {
        if (previousState == null) return;
        ChangeState(previousState);
    }
}