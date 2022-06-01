using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public abstract class InfrastructurePreview : MonoBehaviour
{
    // The Infrastructure to place
    [SerializeField, Tooltip("The Infrastructure to place.")] private Infrastructure realInfrastructure;

    // A list of all the object blocking the placement of the construction.
    protected List<GameObject> objectBlockingPose = new List<GameObject>();

    // The senvitivity with which the preview will check the Navmesh.
    [SerializeField] protected float navMeshSensitivity = 3f;

    [SerializeField] protected float rotateSpeed = 50f;

    [SerializeField] protected bool canRotate;

    // The mesh renderer of the preview.
    [SerializeField] private MeshRenderer mesh;

    private NavMeshHit navmeshHit;

    [SerializeField, Tooltip("Feedbacks to play when the player succeed to place the object.")] protected UnityEvent PlayOnAskToPlaceTrue;
    [SerializeField, Tooltip("Feedbacks to play when the player try to place the object without being able to.")] protected UnityEvent PlayOnAskToPlaceFalse;

    [SerializeField, Tooltip("Feedbacks to play when the player succeed to place the object.")] protected UnityEvent PrevisualiseOnAskToPlaceTrue;
    [SerializeField, Tooltip("Feedbacks to play when the player try to place the object without being able to.")] protected UnityEvent PrevisualiseOnAskToPlaceFalse;

    protected bool availabilityState = true;
    protected bool lastFrameAvailabilityState = true;

    [SerializeField]  protected bool doesSnap = false;

    public float NavmeshSensitivityReal => navMeshSensitivity;

    public bool CanRotate => canRotate;

    /// <summary>
    /// Getter for the Infrastructure.
    /// </summary>
    public Infrastructure RealInfrastructure => realInfrastructure;

    public void SetSnap(bool toSet)
    {
        Debug.Log("Set snap");
        doesSnap = toSet;
    }

    /// <summary>
    /// Used to do specific action when we want to check if the construction can be placed.
    /// </summary>
    /// <param name="position">The position of the construction preview.</param>
    /// <returns>Return true if the player can place the construction.</returns>
    protected abstract bool OnCanPlaceObject(Vector3 position);

    /// <summary>
    /// Used to do specific action when the player try to place the construction.
    /// </summary>
    /// <param name="position">The position where the player want to place the construction.</param>
    protected abstract void OnAskToPlace(Vector3 position);

    /// <summary>
    /// Called when the player try to place the construction.
    /// </summary>
    /// <param name="position">The position where the player want to place the construction.</param>
    /// <returns>Return true if the player can place the construction.</returns>
    public bool AskToPlace(Vector3 position)
    {
        bool canPlace = CanPlaceObject(position);

        OnAskToPlace(position);

        if (canPlace)
        {
            PlayOnAskToPlaceTrue?.Invoke();
        }
        else
        {
            PlayOnAskToPlaceFalse?.Invoke();
        }
        return canPlace;
    }

    /// <summary>
    /// Called when we want to check if the construction can be placed at the specified position.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns>Return true if the player can place the construction.</returns>
    public bool CanPlaceObject(Vector3 position)
    {
        bool toReturn = true;

        //position = new Vector3(position.x, 0, position.z);

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(position, out hit, NavmeshSensitivityReal, NavMesh.AllAreas)) //Check si on est sur un terrain praticable
        {
            toReturn = false;
        }

        if (objectBlockingPose.Count > 0)
        {
            toReturn = false;
        }

        if(realInfrastructure.Purchasable == null || !realInfrastructure.Purchasable.IsPurchasable())
        {
            toReturn = false;
        }

        return toReturn && OnCanPlaceObject(position);
    }

    /// <summary>
    /// Spawn the construction preview.
    /// </summary>
    /// <param name="position">The position of the spawn.</param>
    public void SpawnObject(Vector3 position)
    {

    }

    /// <summary>
    /// Destroy the construction preview GameObject.
    /// </summary>
    public void DespawnObject()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Update the material of the mesh depending of the availability.
    /// </summary>
    public void CheckAvailability()
    {
        if (CanPlaceObject(transform.position) != availabilityState)
        {
            availabilityState = CanPlaceObject(transform.position);
            if (lastFrameAvailabilityState != availabilityState)
            {
                if (availabilityState)
                {
                    PrevisualiseOnAskToPlaceTrue?.Invoke();
                }
                else
                {
                    PrevisualiseOnAskToPlaceFalse?.Invoke();
                }
            }
        }

        lastFrameAvailabilityState = availabilityState;
    }

    public virtual Vector3 TrySetPosition()
    {
        Vector3 targetPosition = PlayerInputManager.GetMousePosition;

        if (NavMesh.SamplePosition(targetPosition, out navmeshHit, NavmeshSensitivityReal, NavMesh.AllAreas))
        {
            if (Vector3.Distance(navmeshHit.position, targetPosition) < NavmeshSensitivityReal)
            {
                targetPosition = navmeshHit.position;
            }
        }

        return targetPosition;
    }

    public virtual float TrySetRotation()
    {
        return -(Input.GetAxis("Mouse X") * rotateSpeed * Time.unscaledDeltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NavMeshObstacle>() != null && !objectBlockingPose.Contains(other.gameObject))
        {
            objectBlockingPose.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectBlockingPose.Contains(other.gameObject))
        {
            objectBlockingPose.Remove(other.gameObject);
        }
    }
}
