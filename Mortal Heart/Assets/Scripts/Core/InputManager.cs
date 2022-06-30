using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class InputManager : Singleton<InputManager>
{
    private InputMap _inputMap;

    private InputActionMap _navigationUIMap;
    private InputActionMap _commonControlMap;
    private InputActionMap _combatMap;
    private InputActionMap _interactMap;

    internal InputAction moveAction;
    internal InputAction dodgeAction;
    internal InputAction leftItemAction;
    internal InputAction rightItemAction;
    internal InputAction attackAction_A;
    internal InputAction attackAction_B;
    internal InputAction attackAction_C;

    public void Init()
    {
        _inputMap = new InputMap();

        _navigationUIMap = _inputMap.UI;
        _commonControlMap = _inputMap.CommonControl;
        _combatMap = _inputMap.Combat;
        _interactMap = _inputMap.Interact;

        moveAction = _inputMap.CommonControl.Move;
        leftItemAction = _inputMap.CommonControl.LeftItem;
        rightItemAction = _inputMap.CommonControl.RightItem;
        dodgeAction = _inputMap.CommonControl.Dodge;
        attackAction_A = _inputMap.Combat.Attack_A;
        attackAction_B = _inputMap.Combat.Attack_B;
        attackAction_C = _inputMap.Combat.Attack_C;

        moveAction.Enable();
        leftItemAction.Enable();
        rightItemAction.Enable();
        dodgeAction.Enable();
        attackAction_A.Enable();
        attackAction_B.Enable();
        attackAction_C.Enable();

        ActiveMap(GameState.InCombat);
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
