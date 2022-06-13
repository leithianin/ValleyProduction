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
                    ""name"": ""CameraPitch"",
                    ""type"": ""Value"",
                    ""id"": ""fde51e52-a701-4679-8141-ce32497fc7ac"",
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
                },
                {
                    ""name"": ""MiddleMouseClic"",
                    ""type"": ""Button"",
                    ""id"": ""6e179e52-1c7b-430c-a798-1652ea985ab3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""Value"",
                    ""id"": ""5229a6d1-cbe4-4ad7-9c22-ef56179e32f3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""23d7e62d-051e-4242-baad-8e3315a127e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""99a6d3ea-8aa6-479a-b9c8-2d2251e75a44"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
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
                    ""name"": ""Arrows"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""20d8a39e-e8ea-4ffc-9be8-e20669e935c4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""17ae5b91-3da2-4c41-85ec-aca56779faab"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraPitch"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a3eb0e19-4b7f-4333-8974-719025c382b3"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraPitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4ce1f75a-8de1-4f49-a53d-e01ef42c412e"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraPitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ca3548c5-6d64-4b17-b9b2-63150b83733b"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleMouseClic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c290c84-2e1c-4dfe-97b1-b6e834ea8c19"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4693b524-f185-4179-afd5-9643f456ffe8"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""Scroll"",
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
        m_MainGame_CameraPitch = m_MainGame.FindAction("CameraPitch", throwIfNotFound: true);
        m_MainGame_Heatmap_1 = m_MainGame.FindAction("Heatmap_1", throwIfNotFound: true);
        m_MainGame_Heatmap_2 = m_MainGame.FindAction("Heatmap_2", throwIfNotFound: true);
        m_MainGame_Heatmap_3 = m_MainGame.FindAction("Heatmap_3", throwIfNotFound: true);
        m_MainGame_Heatmap_4 = m_MainGame.FindAction("Heatmap_4", throwIfNotFound: true);
        m_MainGame_LeftMouseClic = m_MainGame.FindAction("LeftMouseClic", throwIfNotFound: true);
        m_MainGame_RightMouseClic = m_MainGame.FindAction("RightMouseClic", throwIfNotFound: true);
        m_MainGame_MiddleMouseClic = m_MainGame.FindAction("MiddleMouseClic", throwIfNotFound: true);
        m_MainGame_MouseMovement = m_MainGame.FindAction("MouseMovement", throwIfNotFound: true);
        m_MainGame_Escape = m_MainGame.FindAction("Escape", throwIfNotFound: true);
        m_MainGame_Scroll = m_MainGame.FindAction("Scroll", throwIfNotFound: true);
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
    private readonly InputAction m_MainGame_CameraPitch;
    private readonly InputAction m_MainGame_Heatmap_1;
    private readonly InputAction m_MainGame_Heatmap_2;
    private readonly InputAction m_MainGame_Heatmap_3;
    private readonly InputAction m_MainGame_Heatmap_4;
    private readonly InputAction m_MainGame_LeftMouseClic;
    private readonly InputAction m_MainGame_RightMouseClic;
    private readonly InputAction m_MainGame_MiddleMouseClic;
    private readonly InputAction m_MainGame_MouseMovement;
    private readonly InputAction m_MainGame_Escape;
    private readonly InputAction m_MainGame_Scroll;
    public struct MainGameActions
    {
        private @PlayerInputValley m_Wrapper;
        public MainGameActions(@PlayerInputValley wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraMovement => m_Wrapper.m_MainGame_CameraMovement;
        public InputAction @CameraRotation => m_Wrapper.m_MainGame_CameraRotation;
        public InputAction @CameraPitch => m_Wrapper.m_MainGame_CameraPitch;
        public InputAction @Heatmap_1 => m_Wrapper.m_MainGame_Heatmap_1;
        public InputAction @Heatmap_2 => m_Wrapper.m_MainGame_Heatmap_2;
        public InputAction @Heatmap_3 => m_Wrapper.m_MainGame_Heatmap_3;
        public InputAction @Heatmap_4 => m_Wrapper.m_MainGame_Heatmap_4;
        public InputAction @LeftMouseClic => m_Wrapper.m_MainGame_LeftMouseClic;
        public InputAction @RightMouseClic => m_Wrapper.m_MainGame_RightMouseClic;
        public InputAction @MiddleMouseClic => m_Wrapper.m_MainGame_MiddleMouseClic;
        public InputAction @MouseMovement => m_Wrapper.m_MainGame_MouseMovement;
        public InputAction @Escape => m_Wrapper.m_MainGame_Escape;
        public InputAction @Scroll => m_Wrapper.m_MainGame_Scroll;
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
                @CameraPitch.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraPitch;
                @CameraPitch.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraPitch;
                @CameraPitch.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnCameraPitch;
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
                @MiddleMouseClic.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnMiddleMouseClic;
                @MiddleMouseClic.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnMiddleMouseClic;
                @MiddleMouseClic.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnMiddleMouseClic;
                @MouseMovement.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnMouseMovement;
                @Escape.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnEscape;
                @Scroll.started -= m_Wrapper.m_MainGameActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_MainGameActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_MainGameActionsCallbackInterface.OnScroll;
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
                @CameraPitch.started += instance.OnCameraPitch;
                @CameraPitch.performed += instance.OnCameraPitch;
                @CameraPitch.canceled += instance.OnCameraPitch;
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
                @MiddleMouseClic.started += instance.OnMiddleMouseClic;
                @MiddleMouseClic.performed += instance.OnMiddleMouseClic;
                @MiddleMouseClic.canceled += instance.OnMiddleMouseClic;
                @MouseMovement.started += instance.OnMouseMovement;
                @MouseMovement.performed += instance.OnMouseMovement;
                @MouseMovement.canceled += instance.OnMouseMovement;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
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
        void OnCameraPitch(InputAction.CallbackContext context);
        void OnHeatmap_1(InputAction.CallbackContext context);
        void OnHeatmap_2(InputAction.CallbackContext context);
        void OnHeatmap_3(InputAction.CallbackContext context);
        void OnHeatmap_4(InputAction.CallbackContext context);
        void OnLeftMouseClic(InputAction.CallbackContext context);
        void OnRightMouseClic(InputAction.CallbackContext context);
        void OnMiddleMouseClic(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
    }
    public interface ITutorial_BaseActions
    {
    }
}
