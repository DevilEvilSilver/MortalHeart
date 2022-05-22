//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Settings/Input/InputMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputMap : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMap"",
    ""maps"": [
        {
            ""name"": ""Combat"",
            ""id"": ""34cd0270-cfab-43a5-b2e8-aaa676bf9864"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""575e610e-3572-4c38-a9e6-3b59e745f758"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""8785f2f8-d395-40f7-a419-e900f9d4d791"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack_A"",
                    ""type"": ""Button"",
                    ""id"": ""528cbef5-9876-48d0-867a-28b6c82a9ed7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack_B"",
                    ""type"": ""Button"",
                    ""id"": ""eeabb318-2ec6-4181-b29a-8e05b8cc5b18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack_C"",
                    ""type"": ""Button"",
                    ""id"": ""c10f35cb-941e-4343-b42f-1077ac57a713"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3c95c1cd-5526-485d-a778-1e8c0e1598db"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone,NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""95e4b4b3-a7a1-4b30-9db5-78b85f142497"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""37db79c2-5ac6-47f6-84d4-e82c26c4b8b3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b3d25fee-5a30-4e19-990f-a6c4011ed8ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a4350ecb-c250-4000-a3e7-bced2a5b82c3"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""17462b97-57ce-4b2f-b426-97bc41058dcb"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4e0185c3-5640-445d-9a7e-d4269fc4c037"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d63301d8-edf8-4e6a-ac39-54339c999b87"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42065a0f-bca6-46f9-a49e-d4ba91a43c86"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""394700cf-4091-4361-b53c-60f6b74403ae"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09838e0f-1e27-46d2-8c0f-e6d8edaaab3a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_B"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e2d5701-2c7f-47fe-9df0-537ff45e8cb1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_B"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cac92f71-f303-4a2c-ae14-832c0f3d849a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_C"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ccc4e546-8f14-4010-b0d2-8b6e9cbe9b9f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_C"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Combat
        m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
        m_Combat_Move = m_Combat.FindAction("Move", throwIfNotFound: true);
        m_Combat_Dodge = m_Combat.FindAction("Dodge", throwIfNotFound: true);
        m_Combat_Attack_A = m_Combat.FindAction("Attack_A", throwIfNotFound: true);
        m_Combat_Attack_B = m_Combat.FindAction("Attack_B", throwIfNotFound: true);
        m_Combat_Attack_C = m_Combat.FindAction("Attack_C", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Combat
    private readonly InputActionMap m_Combat;
    private ICombatActions m_CombatActionsCallbackInterface;
    private readonly InputAction m_Combat_Move;
    private readonly InputAction m_Combat_Dodge;
    private readonly InputAction m_Combat_Attack_A;
    private readonly InputAction m_Combat_Attack_B;
    private readonly InputAction m_Combat_Attack_C;
    public struct CombatActions
    {
        private @InputMap m_Wrapper;
        public CombatActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Combat_Move;
        public InputAction @Dodge => m_Wrapper.m_Combat_Dodge;
        public InputAction @Attack_A => m_Wrapper.m_Combat_Attack_A;
        public InputAction @Attack_B => m_Wrapper.m_Combat_Attack_B;
        public InputAction @Attack_C => m_Wrapper.m_Combat_Attack_C;
        public InputActionMap Get() { return m_Wrapper.m_Combat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
        public void SetCallbacks(ICombatActions instance)
        {
            if (m_Wrapper.m_CombatActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnMove;
                @Dodge.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnDodge;
                @Attack_A.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_A;
                @Attack_A.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_A;
                @Attack_A.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_A;
                @Attack_B.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_B;
                @Attack_B.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_B;
                @Attack_B.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_B;
                @Attack_C.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_C;
                @Attack_C.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_C;
                @Attack_C.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnAttack_C;
            }
            m_Wrapper.m_CombatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @Attack_A.started += instance.OnAttack_A;
                @Attack_A.performed += instance.OnAttack_A;
                @Attack_A.canceled += instance.OnAttack_A;
                @Attack_B.started += instance.OnAttack_B;
                @Attack_B.performed += instance.OnAttack_B;
                @Attack_B.canceled += instance.OnAttack_B;
                @Attack_C.started += instance.OnAttack_C;
                @Attack_C.performed += instance.OnAttack_C;
                @Attack_C.canceled += instance.OnAttack_C;
            }
        }
    }
    public CombatActions @Combat => new CombatActions(this);
    public interface ICombatActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnAttack_A(InputAction.CallbackContext context);
        void OnAttack_B(InputAction.CallbackContext context);
        void OnAttack_C(InputAction.CallbackContext context);
    }
}
