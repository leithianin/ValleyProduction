using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Infrastructure : MonoBehaviour
{
    [SerializeField, Tooltip("Actions to play when the construction is placed.")] private UnityEvent PlayOnPlace;
    [SerializeField, Tooltip("Actions to play when the construction is deleted.")] private UnityEvent PlayOnDelete;
    [SerializeField, Tooltip("Actions to play when the construction is selected.")] private UnityEvent PlayOnSelect;
    [SerializeField, Tooltip("Actions to play when the construction is unselected.")] private UnityEvent PlayOnUnselect;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnMove;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnReplace;
    [SerializeField, Tooltip("Actions to play when the construction is holded right clic")] private UnityEvent PlayOnHoldRightClic;

    /// <summary>
    /// Used to do specific action when a construction is placed.
    /// </summary>
    /// <param name="position">The position where the construction is placed.</param>
    protected abstract void OnPlaceObject(Vector3 position);

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
        PlayOnPlace?.Invoke();
        OnPlaceObject(position);
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is removed.
    /// </summary>
    public void RemoveObject()
    {
        PlayOnDelete?.Invoke();

        if(OnRemoveObject())
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
}
