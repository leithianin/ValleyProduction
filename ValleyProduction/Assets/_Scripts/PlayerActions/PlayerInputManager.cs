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
    [SerializeField] private UnityEvent<Vector3> OnClicLeftPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicLeftObject;
    [SerializeField] private UnityEvent<GameObject> OnClicLeftHold;

    [SerializeField] private UnityEvent OnClicRight;
    [SerializeField] private UnityEvent<Vector3> OnClicRightPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicRightObject;
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

    public static Vector3 GetMousePosition => instance.GetGroundHitPoint();

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
            if (usedEventSystem.currentSelectedGameObject == null)
            {
                if (Input.GetMouseButtonDown(0))                        //Clic gauche enfoncé
                {
                    //holdLeftTimer = TimerManager.CreateRealTimer(holdDuration, CallLeftHoldMouseInput);
                    //StartCoroutine(TimerHoldLeft());
                }
                else if (Input.GetMouseButtonUp(0))                      //Clic gauche relaché
                {
                    //StopAllCoroutines();
                    //holdLeftTimer?.Stop();
                    //holdLeftTimer = null;
                    //Debug.Log("Coroutines Stop");
                    CallLeftMouseInputs();
                }

                if (Input.GetMouseButtonDown(1))                        //Clic droit enfoncé           
                {
                    holdRightTimer = TimerManager.CreateRealTimer(holdDuration, CallRightHoldMouseInput);
                    //StartCoroutine(TimerHoldRight());
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    CallRightMouseInputs();
                    //StopAllCoroutines();
                    holdRightTimer?.Stop();
                    holdRightTimer = null;
                    //Debug.Log("Coroutines Stop");
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


        /*if (Input.GetKeyDown(KeyCode.E))
        {
            VLY_Time.PauseTime();
        }*/
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

    IEnumerator TimerHoldLeft()
    {
        //Debug.Log("Left Coroutine Start");
        yield return new WaitForSeconds(holdDuration);
        //Debug.Log("Left Coroutine End");
        CallLeftHoldMouseInput();
    }

    IEnumerator TimerHoldRight()
    {
        //Debug.Log("Right Coroutine Start");
        yield return new WaitForSeconds(holdDuration);
        //Debug.Log("Right Coroutine End");
        CallRightHoldMouseInput();
    }

    private void CallLeftMouseInputs()
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

                if (GetHitMouseGameobject(out GameObject hitObject))
                {
                    OnClicLeftObject?.Invoke(hitObject);
                }
            }
        }
    }

    private void CallLeftHoldMouseInput()
    {
        if (GetHitMouseGameobject(out GameObject hitObject))
        {
            clicHold = true;
            OnClicLeftHold?.Invoke(hitObject);
        }
    }

    private void CallRightMouseInputs()
    {
        if (!clicHold)
        {
            OnClicRight?.Invoke();

            if (GetMousePosition != Vector3.zero)
            {
                OnClicRightPosition?.Invoke(GetMousePosition);

                if (GetHitMouseGameobject(out GameObject hitObject))
                {
                    OnClicRightObject?.Invoke(hitObject);
                }
            }
        }

        clicHold = false;
    }

    private void CallRightHoldMouseInput()
    {
        if(GetHitMouseGameobject(out GameObject hitObject))
        {
            clicHold = true;
            OnClicRightHold?.Invoke(hitObject);
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

    private bool GetHitMouseGameobject(out GameObject hitObject)
    {
        RaycastHit hit;
        Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100000.0f, currentLayerMask))
        {
            hitObject = hit.transform.gameObject;
            return true;
        }

        hitObject = null;
        return false;
    }

    private Vector3 GetGroundHitPoint()
    {
        RaycastHit hit;
        Ray ray = usedCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f, currentLayerMask))
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
