using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInputManager : VLY_Singleton<PlayerInputManager>
{
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

    [SerializeField] private UnityEvent OnKeyReturn;
    [SerializeField] private UnityEvent OnKeyDelete;
    [SerializeField] private UnityEvent OnKeyEscape;
    [SerializeField] private UnityEvent<bool> OnKeyLeftShift;

    [SerializeField] private UnityEvent<Vector2> OnKeyMove;
    public Action<Vector2> OnKeyMoveAction;
    public static UnityEvent<Vector2> GetOnKeyMove => instance.OnKeyMove;
    private Vector2 lastKeyDirection;
    public static bool isKeyboardEnable = true;
    public static bool blockMouse = false;

    [SerializeField] private UnityEvent<Vector2> OnMouseMove;

    public static UnityEvent<Vector2> GetOnMouseMove => instance.OnMouseMove;
    public Action<Vector2> OnMouseMoveAction;
    [SerializeField] private UnityEvent<float> OnMouseScroll;
    public static UnityEvent<float> GetOnMouseScroll => instance.OnMouseScroll;
    private float lastScrollValue;

    [SerializeField] private UnityEvent<float> OnMouseWheelDown;
    public static UnityEvent<float> GetOnMouseWheelDown => instance.OnMouseWheelDown;

    [SerializeField] private UnityEvent OnCameraMouseMove;
    public static UnityEvent GetOnCameraMouseMove => instance.OnCameraMouseMove;

    public static bool clicHold = false;

    [SerializeField] private GameContext context;

    private TimerManager.Timer holdRightTimer;
    private TimerManager.Timer holdLeftTimer;

    private RaycastHit raycastHit;

    private CPN_ClicHandler lastClicHandler;
    private CPN_ClicHandler clicHandlerTouched;

    public static Vector3 GetMousePosition => instance.raycastHit.point;

    public static Vector2 GetMousePosition2D => new Vector2(GetMousePosition.x, GetMousePosition.z);

    public static Camera GetCamera => instance.usedCamera;

    private void Start()
    {
        usedCamera.eventMask = context.GetContextLayers(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Handle Mouse input outside UI

        raycastHit = GetHitMouseGameobject();

        if (/*!UIManager.IsOnMenuBool() &&*/ !usedEventSystem.IsPointerOverGameObject())
        {
            if (raycastHit.transform != null)
            {
                clicHandlerTouched = raycastHit.transform.gameObject.GetComponent<CPN_ClicHandler>();
            }
            else
            {
                clicHandlerTouched = null;
            }

            if(Input.GetMouseButtonDown(0))
            {
                CursorTextureManager.SetPressedCursor();
                StartCoroutine(TimerHoldLeft());
            }

            if (Input.GetMouseButtonUp(0))                      //Clic gauche relaché
            {
                CursorTextureManager.SetReleaseCursor();
                StopCoroutine(StartCoroutine(TimerHoldLeft()));
                CallLeftMouseInputs(raycastHit);
            }

            if (Input.GetMouseButtonUp(1))
            {
                CallRightMouseInputs(raycastHit);

                if (!OnBoardingManager.blockFinishPath)
                {
                    Debug.Log("Clic droit");
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


            if (Input.mouseScrollDelta.y != 0 || lastScrollValue != 0)
            {
                OnMouseScroll?.Invoke(Input.mouseScrollDelta.y);
                lastScrollValue = Input.mouseScrollDelta.y;
            }
        }

        CheckForMovementInput();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnKeyEscape?.Invoke();
        }

        OnMouseMove?.Invoke(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));

        OnAzimuthal?.Invoke(Input.GetAxisRaw("Azimuthal"));

        OnPolar?.Invoke(Input.GetAxisRaw("Polar"));

        //CODE REVIEW : Plusieurs bool ou un seul pour disable le Context ?
        if (isKeyboardEnable)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnKeyReturn?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                OnKeyDelete?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                OnKeyLeftShift?.Invoke(true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                OnKeyLeftShift?.Invoke(false);
            }

            //CODE REVIEW : Faire des events
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                VLY_Time.SetTimeScale(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                VLY_Time.SetTimeScale(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                VLY_Time.SetTimeScale(3);
            }
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
        if (!blockMouse)
        {
            if (GetMousePosition != Vector3.zero)
            {
                OnClicLeftPosition?.Invoke(GetMousePosition);
            }

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
        if (!blockMouse)
        {
            if (GetMousePosition != Vector3.zero)
            {
                OnClicRightPosition?.Invoke(GetMousePosition);
            }

        }
        OnClicRight?.Invoke();
    }

    private void CallRightHoldMouseInput(RaycastHit hit)
    {
        if(hit.transform != null)
        {
            clicHold = true;
            OnClicRightHold?.Invoke(hit.transform.gameObject);
        }
    }

    private void CheckForMovementInput()
    {
        float xDirection  = Input.GetAxisRaw("Horizontal");
        float yDirection = Input.GetAxisRaw("Vertical");

        if (xDirection != 0 || yDirection != 0 || lastKeyDirection != Vector2.zero)
        {
            OnKeyMove?.Invoke(new Vector2(xDirection, yDirection));
        }

        lastKeyDirection = new Vector2(xDirection, yDirection);
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
            isKeyboardEnable = true;
        }
        else
        {
            isKeyboardEnable = false;
        }
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
