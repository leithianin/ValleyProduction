using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Infrastructure : MonoBehaviour
{
    [SerializeField, Tooltip("Actions to play when the construction is placed.")] private UnityEvent PlayOnPlace;
    [SerializeField, Tooltip("Actions to play when the construction is deleted.")] private UnityEvent PlayOnDelete;
    [SerializeField, Tooltip("Actions to play when the construction is selected.")] private UnityEvent PlayOnSelect;

    /// <summary>
    /// Used to do specific action when a construction is placed.
    /// </summary>
    /// <param name="position">The position where the construction is placed.</param>
    protected abstract void OnPlaceObject(Vector3 position);

    /// <summary>
    /// Used to do specific action when a construction is removed.
    /// </summary>
    protected abstract void OnRemoveObject();

    /// <summary>
    /// Used to do specific action when a construction is selected.
    /// </summary>
    protected abstract void OnSelectObject();

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
        OnRemoveObject();
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is removed.
    /// </summary>
    public void SelectObject()
    {
        PlayOnSelect?.Invoke();
        OnSelectObject();
    }
}
