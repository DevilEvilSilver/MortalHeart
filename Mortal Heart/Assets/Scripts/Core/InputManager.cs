using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class InputManager : Singleton<InputManager>
{
    private InputMap _inputMap;

    private InputActionMap _coreInputMap;
    private InputActionMap _navigationUIMap;
    private InputActionMap _commonControlMap;
    private InputActionMap _combatMap;
    private InputActionMap _interactMap;

    internal InputAction pauseAction;
    internal InputAction tabAction;

    internal InputAction leftItemAction;
    internal InputAction rightItemAction;
    internal InputAction useItemAction;
    internal InputAction moveAction;
    internal InputAction dodgeAction;

    internal InputAction attackAction_A;
    internal InputAction attackAction_B;
    internal InputAction attackAction_C;

    internal InputAction pickUpAction;

    public void Init()
    {
        _inputMap = new InputMap();

        _coreInputMap = _inputMap.CoreInput;
        _navigationUIMap = _inputMap.UI;
        _commonControlMap = _inputMap.CommonControl;
        _combatMap = _inputMap.Combat;
        _interactMap = _inputMap.Interact;

        pauseAction = _inputMap.CoreInput.Pause;
        tabAction = _inputMap.CoreInput.Tab;

        leftItemAction = _inputMap.CommonControl.LeftItem;
        rightItemAction = _inputMap.CommonControl.RightItem;
        useItemAction = _inputMap.CommonControl.UseItem;
        moveAction = _inputMap.CommonControl.Move;
        dodgeAction = _inputMap.CommonControl.Dodge;

        attackAction_A = _inputMap.Combat.Attack_A;
        attackAction_B = _inputMap.Combat.Attack_B;
        attackAction_C = _inputMap.Combat.Attack_C;

        pickUpAction = _inputMap.Interact.PickUp;

        pauseAction.Enable();
        tabAction.Enable();
        leftItemAction.Enable();
        rightItemAction.Enable();
        useItemAction.Enable();
        moveAction.Enable();
        dodgeAction.Enable();
        attackAction_A.Enable();
        attackAction_B.Enable();
        attackAction_C.Enable();
        pickUpAction.Enable();

        _coreInputMap.Enable();
    }

    public void ActiveMap(GameState gameState)
    {
        DisableInputMaps();
        switch (gameState)
        {
            case GameState.UINavigation:
            default:
                {
                    _navigationUIMap.Enable();
                    break;
                }
            case GameState.InCombat:
                {
                    _commonControlMap.Enable();
                    _combatMap.Enable();
                    break;
                }
            case GameState.Interact:
                {
                    _commonControlMap.Enable();
                    _interactMap.Enable();
                    break;
                }
        }
    }

    private void DisableInputMaps()
    {
        _navigationUIMap.Disable();
        _commonControlMap.Disable();
        _combatMap.Disable();
        _interactMap.Disable();
    }
}
