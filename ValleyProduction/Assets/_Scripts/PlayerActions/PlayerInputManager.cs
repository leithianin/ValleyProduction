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
    [SerializeField] private float holdDuration;

    [SerializeField] private UnityEvent OnClicLeft;
    [SerializeField] private UnityEvent OnClicLeftWihtoutObject;
    [SerializeField] private UnityEvent<Vector3> OnClicLeftPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicLeftHold;

    [SerializeField] private UnityEvent OnClicRight;
    [SerializeField] private UnityEvent OnClicRightWihtoutObject;
    [SerializeField] private UnityEvent<Vector3> OnClicRightPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicRightHold;

    [SerializeField] private UnityEvent OnKeyReturn;
    [SerializeField] private UnityEvent OnKeyDelete;
    [SerializeField] private UnityEvent OnKeyEscape;

    public static Action<Vector2> OnKeyMove;
    public static Action<float> OnMouseScroll;

    public static bool clicHold = false;

    private LayerMask currentLayerMask;
    public LayerMask layerMaskNoTools;
    public LayerMask layerMaskPathTool;

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
        currentLayerMask = layerMaskNoTools;
    }

    // Update is called once per frame
    void Update()
    {
        //Handle Mouse input outside UI

        if (!UIManager.instance.OnMenuOption && !usedEventSystem.IsPointerOverGameObject())
        {
            raycastHit = GetHitMouseGameobject();

            if (raycastHit.transform != null)
            {
                clicHandlerTouched = raycastHit.transform.gameObject.GetComponent<CPN_ClicHandler>();
            }
            else
            {
                clicHandlerTouched = null;
            }

            if (Input.GetMouseButtonUp(0))                      //Clic gauche relach�
            {
                CallLeftMouseInputs(raycastHit);

                if (clicHandlerTouched != null)
                {
                    clicHandlerTouched.MouseDown(0);
                }
                else
                {
                    OnClicLeftWihtoutObject?.Invoke();
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                CallRightMouseInputs(raycastHit);

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

        CheckForMovementInput();

        if(Input.mouseScrollDelta.y != 0)
        {
            OnMouseScroll?.Invoke(Input.mouseScrollDelta.y);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnKeyReturn?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            OnKeyDelete?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnKeyEscape?.Invoke();
        }

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
        yield return new WaitForSeconds(holdDuration);
        //Debug.Log("Left Coroutine End");
        CallLeftHoldMouseInput(raycastHit);
    }

    IEnumerator TimerHoldRight()
    {
        //Debug.Log("Right Coroutine Start");
        yield return new WaitForSeconds(holdDuration);
        //Debug.Log("Right Coroutine End");
        CallRightHoldMouseInput(raycastHit);
    }

    private void CallLeftMouseInputs(RaycastHit hit)
    {
        if (clicHold)
        {
            clicHold = false;
            ConstructionManager.ReplaceStructure();
        }
        else
        {
            OnClicLeft?.Invoke();

            if (GetMousePosition != Vector3.zero)
            {
                OnClicLeftPosition?.Invoke(GetMousePosition);
            }
        }
    }

    private void CallLeftHoldMouseInput(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            clicHold = true;
            OnClicLeftHold?.Invoke(hit.transform.gameObject);
        }
    }

    private void CallRightMouseInputs(RaycastHit hit)
    {
        if (!clicHold)
        {
            OnClicRight?.Invoke();

            if (GetMousePosition != Vector3.zero)
            {
                OnClicRightPosition?.Invoke(GetMousePosition);
            }
        }

        clicHold = false;
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
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        /*if(Input.GetKey(KeyCode.UpArrow))
        {
            yDirection++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            yDirection--;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            xDirection++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xDirection--;
        }*/

        if (xDirection != 0 || yDirection != 0)
        {
            OnKeyMove?.Invoke(new Vector2(xDirection, yDirection));
        }
    }

    private RaycastHit GetHitMouseGameobject()
    {
        RaycastHit hit;
        Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100000.0f, currentLayerMask))
        {
            return hit;
        }

        return hit;
    }

    private Vector3 GetGroundHitPoint()
    {
        RaycastHit hit;
        Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public static void ChangeLayerMaskForNoTools()
    {
        instance.currentLayerMask = instance.layerMaskNoTools;
    }

    public static void ChangeLayerMaskForPathTools()
    {
        instance.currentLayerMask = instance.layerMaskPathTool;
    }
}
