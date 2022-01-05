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

    [SerializeField] private UnityEvent OnClicLeft;
    [SerializeField] private UnityEvent<Vector3> OnClicLeftPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicLeftObject;

    [SerializeField] private UnityEvent OnClicRight;
    [SerializeField] private UnityEvent<Vector3> OnClicRightPosition;
    [SerializeField] private UnityEvent<GameObject> OnClicRightObject;

    [SerializeField] private UnityEvent OnKeyReturn;
    [SerializeField] private UnityEvent OnKeyDelete;
    [SerializeField] private UnityEvent OnKeyEscape;

    public static Action<Vector2> OnKeyMove;
    public static Action<float> OnMouseScroll;


    public static Vector3 GetMousePosition => instance.GetGroundHitPoint();

    public static Vector2 GetMousePosition2D => new Vector2(GetMousePosition.x, GetMousePosition.z);

    // Update is called once per frame
    void Update()
    {
        //Handle Mouse input outside UI
        if (usedEventSystem.currentSelectedGameObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CallLeftMouseInputs();
            }
            if (Input.GetMouseButtonDown(1))
            {
                CallRightMouseInputs();
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

    private void CallLeftMouseInputs()
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

    private void CallRightMouseInputs()
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

    private void CheckForMovementInput()
    {
        int xDirection = 0;
        int yDirection = 0;

        if(Input.GetKey(KeyCode.Z))
        {
            yDirection++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            yDirection--;
        }

        if (Input.GetKey(KeyCode.D))
        {
            xDirection++;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            xDirection--;
        }

        OnKeyMove?.Invoke(new Vector2(xDirection, yDirection));
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
