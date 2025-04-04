//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Player/2PActions.inputactions
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

public partial class @_2PActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @_2PActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""2PActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""14e8b036-82c5-48ec-ab6e-62cbbe4a49bc"",
            ""actions"": [
                {
                    ""name"": ""Shooting"",
                    ""type"": ""Button"",
                    ""id"": ""77472d65-13eb-4a29-b372-466f5c2b2766"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SetMine"",
                    ""type"": ""Button"",
                    ""id"": ""50cdeb17-2754-435b-8d96-c7643e92d42a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateRight"",
                    ""type"": ""Button"",
                    ""id"": ""eaae4b26-5bdb-47db-9427-b7d6905e3357"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateLeft"",
                    ""type"": ""Button"",
                    ""id"": ""fde82810-3b10-4210-a80d-75fab02aedfc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LoadResultScene"",
                    ""type"": ""Button"",
                    ""id"": ""f29b7dae-5273-4be6-b3a6-2b79c2f2dec1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""44dcabda-94c6-4530-9ded-cc7f2e68180b"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shooting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5548e48a-6a2d-44c1-90a3-ceaf6f0ce4ce"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SetMine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73bbd5ae-f892-4e2a-b1cb-b64e0b6186ff"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a8a297a-c8ea-4ad5-ad17-3b4cd495255a"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c93fb2a2-9c20-4526-80b6-aa044b0732f7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LoadResultScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Shooting = m_Player.FindAction("Shooting", throwIfNotFound: true);
        m_Player_SetMine = m_Player.FindAction("SetMine", throwIfNotFound: true);
        m_Player_RotateRight = m_Player.FindAction("RotateRight", throwIfNotFound: true);
        m_Player_RotateLeft = m_Player.FindAction("RotateLeft", throwIfNotFound: true);
        m_Player_LoadResultScene = m_Player.FindAction("LoadResultScene", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Shooting;
    private readonly InputAction m_Player_SetMine;
    private readonly InputAction m_Player_RotateRight;
    private readonly InputAction m_Player_RotateLeft;
    private readonly InputAction m_Player_LoadResultScene;
    public struct PlayerActions
    {
        private @_2PActions m_Wrapper;
        public PlayerActions(@_2PActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shooting => m_Wrapper.m_Player_Shooting;
        public InputAction @SetMine => m_Wrapper.m_Player_SetMine;
        public InputAction @RotateRight => m_Wrapper.m_Player_RotateRight;
        public InputAction @RotateLeft => m_Wrapper.m_Player_RotateLeft;
        public InputAction @LoadResultScene => m_Wrapper.m_Player_LoadResultScene;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Shooting.started += instance.OnShooting;
            @Shooting.performed += instance.OnShooting;
            @Shooting.canceled += instance.OnShooting;
            @SetMine.started += instance.OnSetMine;
            @SetMine.performed += instance.OnSetMine;
            @SetMine.canceled += instance.OnSetMine;
            @RotateRight.started += instance.OnRotateRight;
            @RotateRight.performed += instance.OnRotateRight;
            @RotateRight.canceled += instance.OnRotateRight;
            @RotateLeft.started += instance.OnRotateLeft;
            @RotateLeft.performed += instance.OnRotateLeft;
            @RotateLeft.canceled += instance.OnRotateLeft;
            @LoadResultScene.started += instance.OnLoadResultScene;
            @LoadResultScene.performed += instance.OnLoadResultScene;
            @LoadResultScene.canceled += instance.OnLoadResultScene;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Shooting.started -= instance.OnShooting;
            @Shooting.performed -= instance.OnShooting;
            @Shooting.canceled -= instance.OnShooting;
            @SetMine.started -= instance.OnSetMine;
            @SetMine.performed -= instance.OnSetMine;
            @SetMine.canceled -= instance.OnSetMine;
            @RotateRight.started -= instance.OnRotateRight;
            @RotateRight.performed -= instance.OnRotateRight;
            @RotateRight.canceled -= instance.OnRotateRight;
            @RotateLeft.started -= instance.OnRotateLeft;
            @RotateLeft.performed -= instance.OnRotateLeft;
            @RotateLeft.canceled -= instance.OnRotateLeft;
            @LoadResultScene.started -= instance.OnLoadResultScene;
            @LoadResultScene.performed -= instance.OnLoadResultScene;
            @LoadResultScene.canceled -= instance.OnLoadResultScene;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnShooting(InputAction.CallbackContext context);
        void OnSetMine(InputAction.CallbackContext context);
        void OnRotateRight(InputAction.CallbackContext context);
        void OnRotateLeft(InputAction.CallbackContext context);
        void OnLoadResultScene(InputAction.CallbackContext context);
    }
}
