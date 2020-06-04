// GENERATED AUTOMATICALLY FROM 'Assets/_Game/KeyAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace _Game._Scripts
{
    public class @KeyAction : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @KeyAction()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""KeyAction"",
    ""maps"": [
        {
            ""name"": ""ToggleUI"",
            ""id"": ""941bd0a9-2724-4ef0-a2e4-fd83647aff29"",
            ""actions"": [
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""4b86ac1a-fe99-470b-9424-c5539da7235a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8fab7386-459a-4aee-90ca-54ef90a4933e"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // ToggleUI
            m_ToggleUI = asset.FindActionMap("ToggleUI", throwIfNotFound: true);
            m_ToggleUI_Tab = m_ToggleUI.FindAction("Tab", throwIfNotFound: true);
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

        // ToggleUI
        private readonly InputActionMap m_ToggleUI;
        private IToggleUIActions m_ToggleUIActionsCallbackInterface;
        private readonly InputAction m_ToggleUI_Tab;
        public struct ToggleUIActions
        {
            private @KeyAction m_Wrapper;
            public ToggleUIActions(@KeyAction wrapper) { m_Wrapper = wrapper; }
            public InputAction @Tab => m_Wrapper.m_ToggleUI_Tab;
            public InputActionMap Get() { return m_Wrapper.m_ToggleUI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ToggleUIActions set) { return set.Get(); }
            public void SetCallbacks(IToggleUIActions instance)
            {
                if (m_Wrapper.m_ToggleUIActionsCallbackInterface != null)
                {
                    @Tab.started -= m_Wrapper.m_ToggleUIActionsCallbackInterface.OnTab;
                    @Tab.performed -= m_Wrapper.m_ToggleUIActionsCallbackInterface.OnTab;
                    @Tab.canceled -= m_Wrapper.m_ToggleUIActionsCallbackInterface.OnTab;
                }
                m_Wrapper.m_ToggleUIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Tab.started += instance.OnTab;
                    @Tab.performed += instance.OnTab;
                    @Tab.canceled += instance.OnTab;
                }
            }
        }
        public ToggleUIActions @ToggleUI => new ToggleUIActions(this);
        public interface IToggleUIActions
        {
            void OnTab(InputAction.CallbackContext context);
        }
    }
}
