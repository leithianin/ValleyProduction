using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Infrastructure : MonoBehaviour
{
    [SerializeField] private InfrastructureData datas;

    [SerializeField] private CPN_Purchasable purchaseBehavior;

    [SerializeField, Tooltip("Actions to play when the construction is placed.")] private UnityEvent PlayOnPlace;
    [SerializeField, Tooltip("Actions to play when the construction is placed on an other construction.")] private UnityEvent PlayOnPlaceOverObject;
    [SerializeField, Tooltip("Actions to play when the construction is deleted.")] private UnityEvent PlayOnDelete;
    [SerializeField, Tooltip("Actions to play when the construction is selected.")] private UnityEvent PlayOnSelect;
    [SerializeField, Tooltip("Actions to play when the construction is unselected.")] private UnityEvent PlayOnUnselect;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnMove;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnReplace;
    [SerializeField, Tooltip("Actions to play when the construction is holded right clic")] private UnityEvent PlayOnHoldRightClic;
    [SerializeField, Tooltip("Actions to play when the construction is on mouse over")] private UnityEvent PlayOnMouseOver;
    [SerializeField, Tooltip("Actions to play when the construction is on mouse over")] private UnityEvent PlayOnMouseExit;

    [Header("Data setup")]
    [SerializeField] private UnityEvent<InfrastructureData> OnSetData;

    public InfrastructureData Data => datas;

    public InfrastructureType StructureType => datas.StructureType;

    public CPN_Purchasable Purchasable => purchaseBehavior;

    /// <summary>
    /// Used to do specific action when a construction is placed.
    /// </summary>
    /// <param name="position">The position where the construction is placed.</param>
    protected abstract void OnPlaceObject(Vector3 position);

    protected abstract void OnPlaceObject();

    /// <summary>
    /// Used to do specific action when a construction is removed.
    /// </summary>
    protected abstract bool OnRemoveObject();

    /// <summary>
    /// Used to do specific action when a construction is selected.
    /// </summary>
    protected abstract void OnSelectObject();

    /// <summary>
    /// Used to do specific action when a construction is unselected.
    /// </summary>
    protected abstract void OnUnselectObject();

    /// <summary>
    /// Used to do specific action when a construction is moved.
    /// </summary>
    protected abstract void OnMoveObject();

    protected abstract void OnReplaceObject();

    protected abstract void InfrastructureOnMouseOver();

    protected abstract void InfrastructureOnMouseExit();

    /// <summary>
    /// Hold Right Clic action.
    /// </summary>
    protected abstract void OnHoldRightClic();

    /// <summary>
    /// Play the feedbacks and special actions when the object is placed.
    /// </summary>
    /// <param name="position">The position where the object is placed.</param>
    public void PlaceObject(Vector3 position)
    {
        OnSetData?.Invoke(datas);

        PlayOnPlace?.Invoke();
        OnPlaceObject(position);
    }

    public void PlaceObject()
    {
        PlayOnPlaceOverObject?.Invoke();
        OnPlaceObject();
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is removed.
    /// </summary>
    public void RemoveObject()
    {
        PlayOnDelete?.Invoke();

        if (OnRemoveObject())
        {
            Destroy(gameObject);
        }
        
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is moved.
    /// </summary>
    public void MoveObject()
    {
        PlayOnMove?.Invoke();
        OnMoveObject();
    }

    public void ReplaceObject()
    {
        PlayOnReplace?.Invoke();
        OnReplaceObject();
    }

    public void HoldRightClic()
    {
        PlayOnHoldRightClic?.Invoke();
        OnHoldRightClic();
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is removed.
    /// </summary>
    public void SelectObject()
    {
        PlayOnSelect?.Invoke();
        OnSelectObject();
    }

    public void UnselectObject()
    {
        PlayOnUnselect?.Invoke();
        OnUnselectObject();
    }

    //L'enfant est prioritaire par rapport à son parent
    private void OnMouseOver()
    {
        InfrastructureManager.SnapInfrastructure(this);
        PlayOnMouseOver?.Invoke();
        InfrastructureOnMouseOver();
    }

    private void OnMouseExit()
    {
        InfrastructureManager.DesnapInfrastructure(this);
        PlayOnMouseExit?.Invoke();
        InfrastructureOnMouseExit();
    }
}
