using UnityEngine.InputSystem;

public class FSMManager
{
    public BaseState currentState { get; private set; }
    public BaseState nextState { get; set; }
    public BaseState previousState { get; private set; }

    private bool _isActive;

    public FSMManager()
    {
        _isActive = true;
    }

    public void OnUpdate()
    {
        if (!_isActive) return;
        if (currentState == null) return;

        currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        if (!_isActive) return;
        if (currentState == null) return;

        currentState.OnFixedUpdate();
    }

    public void ChangeState(BaseState newState, bool isByPassLock = false)
    {
        if (!_isActive) return;
        if (!isByPassLock && newState == currentState) return;

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
        if (!_isActive) return;
        if (!isByPassLock && newState == currentState) return;
        nextState = _nextState;

        ChangeState(newState, isByPassLock);
    }

    public void StopCurrentState()
    {
        if (currentState != null)
        {
            currentState.OnStop();
            _isActive = false;
        }
    }

    public void GoToNextState(bool isByPassLock = false)
    {
        if (!_isActive) return;
        if (nextState == null) return;
        ChangeState(nextState, isByPassLock);
    }

    public void GoToPreviouseState()
    {
        if (!_isActive) return;
        if (previousState == null) return;
        ChangeState(previousState);
    }
}