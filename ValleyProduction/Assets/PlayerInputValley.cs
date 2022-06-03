// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputValley.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputValley : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputValley()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputValley"",
    ""maps"": [
        {
            ""name"": ""MainGame"",
            ""id"": ""0ec0cf11-5e1d-40e0-b27b-a514913c7141"",
            ""actions"": [
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""798c3cf8-ed62-4ce8-85f8-5e0368b95a7a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraRotation"",
                    ""type"": ""Value"",
                    ""id"": ""6d8c2bc5-550a-4d30-b7d6-6d2925966da5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heatmap_1"",
                    ""type"": ""Button"",
                    ""id"": ""3d93365a-0e43-4d1c-9ce6-394e251910c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heatmap_2"",
                    ""type"": ""Button"",
                    ""id"": ""cdac2c16-aeb2-4801-97a5-9b91b513969b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heatmap_3"",
                    ""type"": ""Button"",
                    ""id"": ""13659e86-bc3a-45e7-9b06-26d1ba01e205"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heatmap_4"",
                    ""type"": ""Button"",
                    ""id"": ""2a47ffd8-c152-4837-b47c-250eccc634ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftMouseClic"",
                    ""type"": ""Button"",
                    ""id"": ""87740b62-ce5f-46fb-9082-efbc77f32251"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightMouseClic"",
                    ""type"": ""Button"",
                    ""id"": ""b6216879-4ea9-4621-a1dc-4471c0f032fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""54c978d1-16cc-446b-9984-2d45307f8f96"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""12b5092e-0133-46b7-ad66-c64c093915ee"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d2e72f19-d7f9-4cc2-bf90-65c17b02e07e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5b89c9f0-31ed-4bde-8910-71336da8e6b4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""65f86407-f8a2-4cbd-954b-9ad1b3203a8a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""efe28aa8-dc67-4c25-94fe-7a695ef45db9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""304aadab-25be-4017-ae4f-14d19d0bd48e"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a0363130-dede-401b-af3e-16c3cab7107b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8980476f-1e88-40cf-ad7b-a101d8cb964b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ac0cb475-273a-4c3e-beec-8f44f25272ae"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""e9d007fa-a481-499e-bce6-2ff0ffc9cfff"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c0b9c921-348d-4f04-b559-417733a25e2b"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""683baf34-5927-4704-aa6e-044662f3d665"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fa5ab621-5ee0-453c-a4d2-028bddd87181"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Heatmap_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b01193d4-65eb-4d64-9755-295e80eca206"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Heatmap_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20550fee-cf5d-4148-ba36-11ebb90a509f"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Heatmap_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c81bd6e8-eb1a-42bf-b338-c27b3107b35e"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Heatmap_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f787d332-a5a5-4dca-ac01-af8b6d784f0e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftMouseClic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a001ae41-ec8d-4f67-b4fa-a769c80a5060"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightMouseClic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Tutorial_Base"",
            ""id"": ""50b368d3-c2b7-4ddd-85cf-fb08d88be404"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": []
}");
        // MainGame
        m_MainGame = asset.FindActionMap("MainGame", throwIfNotFound: true);
        m_MainGame_CameraMovement = m_MainGame.FindAction("CameraMovement", throwIfNotFound: true);
        m_MainGame_CameraRotation = m_MainGame.FindAction("CameraRotation", throwIfNotFound: true);
        m_MainGame_Heatmap_1 = m_MainGame.FindAction("Heatmap_1", throwIfNotFound: true);
        m_MainGame_Heatmap_2 = m_MainGame.FindAction("Heatmap_2", throwIfNotFound: true);
        m_MainGame_Heatmap_3 = m_MainGame.FindAction("Heatmap_3", throwIfNotFound: true);
        m_MainGame_Heatmap_4 = m_MainGame.FindAction("Heatmap_4", throwIfNotFound: true);
        m_MainGame_LeftMouseClic = m_MainGame.FindAction("LeftMouseClic", throwIfNotFound: true);
        m_MainGame_RightMouseClic = m_MainGame.FindAction("RightMouseClic", throwIfNotFound: true);
        // Tutorial_Base
        m_Tutorial_Base = asset.FindActionMap("Tutorial_Base", throwIfNotFound: true);
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

    // MainGame
    private readonly InputActionMap m_MainGame;
    private IMainGameActions m_MainGameActionsCallbackInterface;
    private readonly InputAction m_MainGame_CameraMovement;
    private readonly InputAction m_MainGame_CameraRotation;
    private readonly InputAction m_MainGame_Heatmap_1;
    private readonly InputAction m_MainGame_Heatmap_2;
    private readonly InputAction m_MainGame_Heatmap_3;
    private readonly InputAction m_MainGame_Heatmap_4;
    private readonly InputAction m_MainGame_LeftMouseClic;
    private readonly InputAction m_MainGame_RightMouseClic;
    public struct MainGameActions
    {
        private @PlayerInputValley m_Wrapper;
        public MainGameActions(@PlayerInputValley wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraMovement => m_Wrapper.m_MainGame_CameraMovement;
        public InputAction @CameraRotation => m_Wrapper.m_MainGame_CameraRotation;
        public InputAction @Heatmap_1 => m_Wrapper.m_MainGame_Heatmap_1;
        public InputAction @Heatmap_2 => m_Wrapper.m_MainGame_Heatmap_2;
        public InputAction @Heatmap_3 => m_Wrapper.m_MainGame_Heatmap_3;
        public InputAction @Heatmap_4 => m_Wrapper.m_MainGame_Heatmap_4;
        public InputAction @LeftMouseClic => m_Wrapper.m_MainGame_LeftMouseClic;
        public InputAction @RightMouseClic => m_Wrapper.m_MainGame_RightMouseClic;
        public InputActionMap Get() { return m_Wrapper.m_MainGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainGameActions set) { return set.Get(); }
        public void SetCallbacks(IMainGameActions instance)
        {
            if (m_Wrapper.m_MainGameActionsCallbackInterface != null)
            {
                @CameraMovement.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraMovement;
                @CameraRotation.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraRotation;
                @CameraRotation.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraRotation;
                @CameraRotation.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraRotation;
                @Heatmap_1.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_1;
                @Heatmap_1.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_1;
                @Heatmap_1.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_1;
                @Heatmap_2.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_2;
                @Heatmap_2.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_2;
                @Heatmap_2.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_2;
                @Heatmap_3.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_3;
                @Heatmap_3.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_3;
                @Heatmap_3.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_3;
                @Heatmap_4.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_4;
                @Heatmap_4.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_4;
                @Heatmap_4.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnHeatmap_4;
                @LeftMouseClic.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnLeftMouseClic;
                @LeftMouseClic.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnLeftMouseClic;
                @LeftMouseClic.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnLeftMouseClic;
                @RightMouseClic.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnRightMouseClic;
                @RightMouseClic.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnRightMouseClic;
                @RightMouseClic.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnRightMouseClic;
            }
            m_Wrapper.m_MainGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraMovement.started += instance.OnCameraMovement;
                @CameraMovement.performed += instance.OnCameraMovement;
                @CameraMovement.canceled += instance.OnCameraMovement;
                @CameraRotation.started += instance.OnCameraRotation;
                @CameraRotation.performed += instance.OnCameraRotation;
                @CameraRotation.canceled += instance.OnCameraRotation;
                @Heatmap_1.started += instance.OnHeatmap_1;
                @Heatmap_1.performed += instance.OnHeatmap_1;
                @Heatmap_1.canceled += instance.OnHeatmap_1;
                @Heatmap_2.started += instance.OnHeatmap_2;
                @Heatmap_2.performed += instance.OnHeatmap_2;
                @Heatmap_2.canceled += instance.OnHeatmap_2;
                @Heatmap_3.started += instance.OnHeatmap_3;
                @Heatmap_3.performed += instance.OnHeatmap_3;
                @Heatmap_3.canceled += instance.OnHeatmap_3;
                @Heatmap_4.started += instance.OnHeatmap_4;
                @Heatmap_4.performed += instance.OnHeatmap_4;
                @Heatmap_4.canceled += instance.OnHeatmap_4;
                @LeftMouseClic.started += instance.OnLeftMouseClic;
                @LeftMouseClic.performed += instance.OnLeftMouseClic;
                @LeftMouseClic.canceled += instance.OnLeftMouseClic;
                @RightMouseClic.started += instance.OnRightMouseClic;
                @RightMouseClic.performed += instance.OnRightMouseClic;
                @RightMouseClic.canceled += instance.OnRightMouseClic;
            }
        }
    }
    public MainGameActions @MainGame => new MainGameActions(this);

    // Tutorial_Base
    private readonly InputActionMap m_Tutorial_Base;
    private ITutorial_BaseActions m_Tutorial_BaseActionsCallbackInterface;
    public struct Tutorial_BaseActions
    {
        private @PlayerInputValley m_Wrapper;
        public Tutorial_BaseActions(@PlayerInputValley wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_Tutorial_Base; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Tutorial_BaseActions set) { return set.Get(); }
        public void SetCallbacks(ITutorial_BaseActions instance)
        {
            if (m_Wrapper.m_Tutorial_BaseActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_Tutorial_BaseActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public Tutorial_BaseActions @Tutorial_Base => new Tutorial_BaseActions(this);
    public interface IMainGameActions
    {
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnCameraRotation(InputAction.CallbackContext context);
        void OnHeatmap_1(InputAction.CallbackContext context);
        void OnHeatmap_2(InputAction.CallbackContext context);
        void OnHeatmap_3(InputAction.CallbackContext context);
        void OnHeatmap_4(InputAction.CallbackContext context);
        void OnLeftMouseClic(InputAction.CallbackContext context);
        void OnRightMouseClic(InputAction.CallbackContext context);
    }
    public interface ITutorial_BaseActions
    {
    }
}
