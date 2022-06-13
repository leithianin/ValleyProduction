using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputManager : VLY_Singleton<PlayerInputManager>
{
    private PlayerInputValley inputControl;
    [SerializeField] private InputActionReference cameraMovementHandler;
    [SerializeField] private InputActionReference cameraRotationHandler;
    [SerializeField] private InputActionReference cameraPitchHandler;
    [SerializeField] private List<InputActionReference> heatmapSelectorsHandler;

    [SerializeField] private InputActionReference mouseLeftClic;

    [SerializeField] private InputActionReference mouseRightClic;

    [SerializeField] private InputActionReference mouseMiddleClic;

    [SerializeField] private InputActionReference mouseMovement;

    [SerializeField] private InputActionReference mouseScroll;

    [SerializeField] private InputActionReference escapeInput;

    [SerializeField] private Camera usedCamera;
    [SerializeField] private EventSystem usedEventSystem;
    [SerializeField] private Terrain mainTerrain;
    public static Terrain GetMainTerrain => instance.mainTerrain;
    public static float GetTerrainHeight(Vector3 positionToSearch)
    {
        return GetMainTerrain.SampleHeight(positionToSearch) + GetMainTerrain.transform.position.y;
    }

    [SerializeField] private float holdDuration;

    [SerializeField] private UnityEvent OnClicLeft;
    [SerializeField] private UnityEvent OnClicLeftWihtoutObject;
    [SerializeField] private UnityEvent<Vector3> OnClicLeftPosition;
    [SerializeField] private UnityEvent<Vector3> OnClicLeftHold;

    [SerializeField] private UnityEvent OnClicRight;
    [SerializeField] private UnityEvent OnClicRightWihtoutObject;
    [SerializeField] private UnityEvent<Vector3> OnClicRightPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicRightHold;

    [SerializeField] private UnityEvent OnClicScrollWheel;
    [SerializeField] private UnityEvent<float> OnPolar;
    [SerializeField] private UnityEvent<float> OnAzimuthal;

    [SerializeField] private UnityEvent OnKeyEscape;
    [SerializeField] private UnityEvent<bool> OnKeyLeftShift;

    [SerializeField] private UnityEvent<Vector2> OnKeyMove;
    public Action<Vector2> OnKeyMoveAction;
    public static UnityEvent<Vector2> GetOnKeyMove => instance.OnKeyMove;
    private Vector2 lastKeyDirection;
    private float lastKeyRotation;
    private float lastKeyPitch;
    private float lastScrollDirection;
    private Vector2 lastMouseMovement;
    public bool isKeyboardEnable = true;
    public bool blockMouse = false;
    public bool isCameraBlock = false;

    public static bool IsKeyboardEnable => instance.isKeyboardEnable;
    public static bool BlockMouse => instance.blockMouse;

    [SerializeField] private UnityEvent<Vector2> OnMouseMove;

    public static UnityEvent<Vector2> GetOnMouseMove => instance.OnMouseMove;
    public Action<Vector2> OnMouseMoveAction;
    [SerializeField] private UnityEvent<float> OnMouseScroll;
    public static UnityEvent<float> GetOnMouseScroll => instance.OnMouseScroll;

    [SerializeField] private UnityEvent<float> OnMouseWheelDown;
    public static UnityEvent<float> GetOnMouseWheelDown => instance.OnMouseWheelDown;

    [SerializeField] private UnityEvent OnCameraMouseMove;
    public static UnityEvent GetOnCameraMouseMove => instance.OnCameraMouseMove;

    private bool righClicHold = false;
    private bool middleClicHold = false;

    [SerializeField] private GameContext context;

    private RaycastHit raycastHit;

    private CPN_ClicHandler lastClicHandler;
    private CPN_ClicHandler clicHandlerTouched;

    public static Vector3 GetMousePosition => instance.raycastHit.point;

    public static Vector2 GetMousePosition2D => new Vector2(GetMousePosition.x, GetMousePosition.z);

    public static Camera GetCamera => instance.usedCamera;


    protected override void OnAwake()
    {
        base.OnAwake();
        inputControl = new PlayerInputValley();
    }

    private void Start()
    {
        usedCamera.eventMask = context.GetContextLayers(0);

        EnableAllInput();
    }

    private void OnEnable()
    {
        inputControl.Enable();
        inputControl.MainGame.Enable();

        mouseLeftClic.action.started += ActionLeftDown;
        mouseLeftClic.action.performed += ActionLeftHold;
        mouseLeftClic.action.canceled += ActionLeftUp;

        mouseRightClic.action.started += ActionRightDown;
        mouseRightClic.action.performed += ActionRightHold;
        mouseRightClic.action.canceled += ActionRightUp;

        mouseMiddleClic.action.started += ActionMiddleDown;
        mouseMiddleClic.action.performed += ActionMiddleHold;
        mouseMiddleClic.action.canceled += ActionMiddleUp;

        escapeInput.action.started += ActionEscape;

        heatmapSelectorsHandler[0].action.started += ActionHeatmap1;
        heatmapSelectorsHandler[1].action.started += ActionHeatmap2;
        heatmapSelectorsHandler[2].action.started += ActionHeatmap3;
        heatmapSelectorsHandler[3].action.started += ActionHeatmap4;
    }

    private void OnDisable()
    {
        mouseLeftClic.action.started -= ActionLeftDown;
        mouseLeftClic.action.performed -= ActionLeftHold;
        mouseLeftClic.action.canceled -= ActionLeftUp;

        mouseRightClic.action.started -= ActionRightDown;
        mouseRightClic.action.performed -= ActionRightHold;
        mouseRightClic.action.canceled -= ActionRightUp;

        mouseMiddleClic.action.started -= ActionMiddleDown;
        mouseMiddleClic.action.performed -= ActionMiddleHold;
        mouseMiddleClic.action.canceled -= ActionMiddleUp;

        escapeInput.action.started -= ActionEscape;

        heatmapSelectorsHandler[0].action.started -= ActionHeatmap1;
        heatmapSelectorsHandler[1].action.started -= ActionHeatmap2;
        heatmapSelectorsHandler[2].action.started -= ActionHeatmap3;
        heatmapSelectorsHandler[3].action.started -= ActionHeatmap4;

        inputControl.Disable();

        VLY_Time.SetTimeScale(1f);
    }

    #region Action inputs
    public void ActionLeftDown(InputAction.CallbackContext context)
    {
        if (!usedEventSystem.IsPointerOverGameObject())
        {
            CursorTextureManager.SetPressedCursor();
            CallLeftHoldMouseInput(raycastHit);
        }
    }

    public void ActionLeftHold(InputAction.CallbackContext context)
    {
        
    }

    public void ActionLeftUp(InputAction.CallbackContext context)
    {
        if (!usedEventSystem.IsPointerOverGameObject())
        {
            CursorTextureManager.SetReleaseCursor();
            CallLeftMouseInputs(raycastHit);
        }
    }

    public void ActionRightDown(InputAction.CallbackContext context)
    {
        if (!usedEventSystem.IsPointerOverGameObject())
        {
            righClicHold = true;
        }
    }

    public void ActionRightHold(InputAction.CallbackContext context)
    {
        if (!usedEventSystem.IsPointerOverGameObject())
        {
            CallRightHoldMouseInput(raycastHit);
        }
    }

    public void ActionRightUp(InputAction.CallbackContext context)
    {
        if (!usedEventSystem.IsPointerOverGameObject())
        {
            CallRightMouseInputs(raycastHit);
        }
        righClicHold = false;
    }

    public void ActionMiddleDown(InputAction.CallbackContext context)
    {
        if (!usedEventSystem.IsPointerOverGameObject())
        {
            middleClicHold = true;
        }
    }

    public void ActionMiddleHold(InputAction.CallbackContext context)
    {
        
    }

    public void ActionMiddleUp(InputAction.CallbackContext context)
    {
        middleClicHold = false;
    }

    public void ActionEscape(InputAction.CallbackContext context)
    {
        OnKeyEscape?.Invoke();
    }

    public void ActionHeatmap1(InputAction.CallbackContext context)
    {
        HeatmapViewController.HandleHeatmap(1);
    }
    public void ActionHeatmap2(InputAction.CallbackContext context)
    {
        HeatmapViewController.HandleHeatmap(2);
    }
    public void ActionHeatmap3(InputAction.CallbackContext context)
    {
        HeatmapViewController.HandleHeatmap(3);
    }
    public void ActionHeatmap4(InputAction.CallbackContext context)
    {
        HeatmapViewController.HandleHeatmap(4);
    }

    [ContextMenu("Test disable input")]
    public void DisableKeyMovement()
    {
        DisableInput(cameraMovementHandler);
    }

    public void DisableInput(InputActionReference toDisable)
    {
        Debug.Log("Disable : " + toDisable.name);
        toDisable.action.Disable();
    }

    public void EnableInput(InputActionReference toEnable)
    {
        Debug.Log("Enable: " + toEnable.name);
        toEnable.action.Enable();
    }

    public void EnableAllInput()
    {
        Debug.Log("Before Enable : " + cameraMovementHandler.action.enabled);

        cameraMovementHandler.action.Enable();

        Debug.Log("After Enable : " + cameraMovementHandler.action.enabled);
        cameraRotationHandler.action.Enable();
        cameraPitchHandler.action.Enable();
        foreach (InputActionReference inpt in heatmapSelectorsHandler)
        {
            inpt.action.Enable();
        }

        mouseLeftClic.action.Enable();

        mouseRightClic.action.Enable();

        mouseMiddleClic.action.Enable();

        mouseMovement.action.Enable();

        mouseScroll.action.Enable();

        escapeInput.action.Enable();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        raycastHit = GetHitMouseGameobject();

        if (!usedEventSystem.IsPointerOverGameObject())
        {
            if (raycastHit.transform != null)
            {
                clicHandlerTouched = raycastHit.transform.gameObject.GetComponent<CPN_ClicHandler>();
            }
            else
            {
                clicHandlerTouched = null;
            }

            if(mouseScroll.action.ReadValue<float>() != 0 || lastScrollDirection != 0)
            {
                OnMouseScroll?.Invoke(mouseScroll.action.ReadValue<float>());

                lastScrollDirection = mouseScroll.action.ReadValue<float>();
            }
        }

        if (!isCameraBlock)
        {
            CheckCameraInput();
        }

        //CODE REVIEW : Plusieurs bool ou un seul pour disable le Context ?
        if (isKeyboardEnable)
        {

        }

        ResetClicHandler();
    }

    private void ResetClicHandler()
    {
        if (clicHandlerTouched != lastClicHandler)
        {
            if (lastClicHandler != null)
            {
                lastClicHandler.MouseExit();
            }

            lastClicHandler = clicHandlerTouched;
            
            if (clicHandlerTouched != null)
            {
                clicHandlerTouched.MouseEnter();
            }
        }

        clicHandlerTouched = null;
    }

    IEnumerator TimerHoldLeft()
    {
        //Debug.Log("Left Coroutine Start");
        yield return new WaitForSecondsRealtime(holdDuration);
        //Debug.Log("Left Coroutine End");
        CallLeftHoldMouseInput(raycastHit);
    }

    IEnumerator TimerHoldRight()
    {
        //Debug.Log("Right Coroutine Start");
        yield return new WaitForSecondsRealtime(holdDuration);
        //Debug.Log("Right Coroutine End");
        CallRightHoldMouseInput(raycastHit);
    }

    private void CallLeftMouseInputs(RaycastHit hit)
    {
        if (!blockMouse && !usedEventSystem.IsPointerOverGameObject())
        {
            if (GetMousePosition != Vector3.zero)
            {
                if (!OnBoardingManager.blockPlacePathpoint)
                {
                    OnClicLeftPosition?.Invoke(GetMousePosition);
                }
            }

            clicHandlerTouched = raycastHit.transform.gameObject.GetComponent<CPN_ClicHandler>();

            if (clicHandlerTouched != null)
            {
                if (!OnBoardingManager.blockFinishPath)
                {
                    clicHandlerTouched.MouseDown(0);
                }
            }
            else
            {
                OnClicLeftWihtoutObject?.Invoke();
            }
        }
        OnClicLeft?.Invoke();
    }

    private void CallLeftHoldMouseInput(RaycastHit hit)
    {
        if (!OnBoardingManager.blockPlacePathpoint)
        {
            if (hit.transform != null)
            {
                OnClicLeftHold?.Invoke(GetMousePosition);
            }
        }
    }

    private void CallRightMouseInputs(RaycastHit hit)
    {
        if (!blockMouse && !usedEventSystem.IsPointerOverGameObject())
        {
            if (GetMousePosition != Vector3.zero)
            {
                OnClicRightPosition?.Invoke(GetMousePosition);
            }

            clicHandlerTouched = raycastHit.transform.gameObject.GetComponent<CPN_ClicHandler>();

            if (!OnBoardingManager.blockFinishPath)
            {
                if (clicHandlerTouched != null)
                {

                    clicHandlerTouched.MouseDown(1);

                }
                else
                {
                    OnClicRightWihtoutObject?.Invoke();
                }
            }
        }

        OnClicRight?.Invoke();
    }

    private void CallRightHoldMouseInput(RaycastHit hit)
    {
        if(hit.transform != null)
        {
            OnClicRightHold?.Invoke(hit.transform.gameObject);
        }
    }

    private void CheckCameraInput()
    {
        if (middleClicHold)
        {
            lastMouseMovement = mouseMovement.action.ReadValue<Vector2>();

            OnAzimuthal?.Invoke(lastMouseMovement.x);

            OnPolar?.Invoke(lastMouseMovement.y);
        }
        else if(lastMouseMovement != Vector2.zero)
        {
            lastMouseMovement = Vector2.zero;

            OnAzimuthal?.Invoke(lastMouseMovement.x);

            OnPolar?.Invoke(lastMouseMovement.y);
        }
        else
        {
            float rotationMovement = cameraRotationHandler.action.ReadValue<float>();

            if (rotationMovement != 0 || lastKeyRotation != 0)
            {
                OnAzimuthal?.Invoke(rotationMovement);

                lastKeyRotation = rotationMovement;
            }

            float pitchMovement = cameraPitchHandler.action.ReadValue<float>();

            if (pitchMovement != 0 || lastKeyPitch != 0)
            {
                OnPolar?.Invoke(pitchMovement);

                lastKeyPitch = pitchMovement;
            }
        }

        if (righClicHold)
        {
            OnMouseMove?.Invoke(mouseMovement.action.ReadValue<Vector2>());
        }
        else
        {
            Vector2 currentDirection = cameraMovementHandler.action.ReadValue<Vector2>();

            if (currentDirection != Vector2.zero || lastKeyDirection != Vector2.zero)
            {
                OnKeyMove?.Invoke(currentDirection);
            }

            lastKeyDirection = currentDirection;
        }
    }

    private RaycastHit GetHitMouseGameobject()
    {
        RaycastHit hit;
        Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100000.0f, GetCamera.eventMask))
        {
            return hit;
        }

        return hit;
    }

    public static void EnableOrDisableKeyboard(bool isEnable)
    {
        if(isEnable)
        {
           instance.isKeyboardEnable = true;
        }
        else
        {
            instance.isKeyboardEnable = false;
        }
    }

    public static void EnableOrDisableCameraControl(bool isCameraLocked)
    {
        instance.isCameraBlock = isCameraLocked;
    }

    public static void EnableOrDisableLockMouse(bool isMouseLocked)
    {
        instance.blockMouse = isMouseLocked;
    }

    [Obsolete]
    private Vector3 GetGroundHitPoint()
    {
        RaycastHit hit;
        Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f, LayerMask.GetMask("Terrain")))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
