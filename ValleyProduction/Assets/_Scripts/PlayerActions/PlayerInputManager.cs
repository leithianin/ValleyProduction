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

    public static Vector3 GetMousePosition => instance.GetGroundHitPoint();

    public static Vector2 GetMousePosition2D => new Vector2(GetMousePosition.x, GetMousePosition.z);

    // Update is called once per frame
    void Update()
    {
        //Handle Mouse input outside UI

        if (usedEventSystem.currentSelectedGameObject == null)
        {
            if (Input.GetMouseButtonDown(0))                        //Clic gauche enfonc�
            {
                StartCoroutine(TimerHoldLeft());
            }
            else if(Input.GetMouseButtonUp(0))                      //Clic gauche relach�
            {
                StopAllCoroutines();
                //Debug.Log("Coroutines Stop");
                CallLeftMouseInputs();
            }

            if (Input.GetMouseButtonDown(1))                        //Clic droit enfonc�           
            {
                StartCoroutine(TimerHoldRight());
            }
            else if(Input.GetMouseButtonUp(1))
            {
                CallRightMouseInputs();
                StopAllCoroutines();
                //Debug.Log("Coroutines Stop");
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

        // Input pour tester
        if(Input.GetKeyDown(KeyCode.A))
        {
            ConstructionManager.SelectInfrastructureType(InfrastructureType.PathTools);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ConstructionManager.UnselectInfrastructureType();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ConstructionManager.SelectInfrastructureType(InfrastructureType.DeleteStructure);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ConstructionManager.UnselectStructure();
        }
    }

    IEnumerator TimerHoldLeft()
    {
        Debug.Log("Left Coroutine Start");
        yield return new WaitForSeconds(holdDuration);
        Debug.Log("Left Coroutine End");
        CallLeftHoldMouseInput();
    }

    IEnumerator TimerHoldRight()
    {
        Debug.Log("Right Coroutine Start");
        yield return new WaitForSeconds(holdDuration);
        Debug.Log("Right Coroutine End");
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
        if (Physics.Raycast(ray, out hit, 100000.0f))
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
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
