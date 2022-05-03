using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Infrastructure : MonoBehaviour
{
    [SerializeField] private InfrastructureData datas;

    [SerializeField] private CPN_Purchasable purchaseBehavior;

    [SerializeField] private bool isOpen = true;

    [SerializeField, Tooltip("Actions to play when the construction is placed.")] private UnityEvent PlayOnPlace;
    [SerializeField, Tooltip("Actions to play when the construction is placed on an other construction.")] private UnityEvent PlayOnPlaceOverObject;
    [SerializeField, Tooltip("Actions to play when the construction is deleted.")] private UnityEvent PlayOnDelete;
    [SerializeField, Tooltip("Actions to play when the construction is selected.")] private UnityEvent PlayOnSelect;
    [SerializeField, Tooltip("Actions to play when the construction is unselected.")] private UnityEvent PlayOnUnselect;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnStartMove;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnMove;
    [SerializeField, Tooltip("Actions to play when the construction is moved.")] private UnityEvent PlayOnReplace;
    [SerializeField, Tooltip("Actions to play when the construction is holded right clic")] private UnityEvent PlayOnHoldRightClic;
    [SerializeField, Tooltip("Actions to play when the structure is opened/closed")] private UnityEvent OnOpenStructure;
    [SerializeField, Tooltip("Actions to play when the structure is opened/closed")] private UnityEvent OnCloseStructure;

    [Header("Data setup")]
    [SerializeField] private UnityEvent<InfrastructureData> OnSetData;

    public InfrastructureData Data => datas;

    public InfrastructureType StructureType => datas.StructureType;

    public bool IsOpen => isOpen;

    public CPN_Purchasable Purchasable => purchaseBehavior;

    [Header("Structure Information")]
    public InterestPoint interestPoint;

    public InfrastructureDataRunTime infraDataRunTime = new InfrastructureDataRunTime();

    public class InfrastructureDataRunTime
    {
        public string name = string.Empty;
        public int visitorsTotal = 0;
        public int moneyTotal = 0;
    }

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

    protected abstract void OnStartMoveObject();

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

    public void StartMoveObject()
    {
        PlayOnStartMove?.Invoke();
        OnStartMoveObject();
    }

    public void SetPreviewMat()
    {
        //CODE Review : Faire un autre event pour le changement visuel ?
        PlayOnMove?.Invoke();
    }

    public void SetNormalMat()
    {
        PlayOnReplace?.Invoke();
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is moved.
    /// </summary>
    public void MoveObject()
    {
        CloseStructure();
        PlayOnMove?.Invoke();
        OnMoveObject();
    }

    public void CancelMoveObject()
    {
        OpenStructure();
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
    public void MouseOver()
    {
        InfrastructureManager.SnapInfrastructure(this);
        InfrastructureOnMouseOver();
    }

    public void MouseExit()
    {
        InfrastructureManager.DesnapInfrastructure(this);
        InfrastructureOnMouseExit();
    }

    public void AskToDelete()
    {
        ConstructionManager.DestroyStructure(gameObject);
    }

    public void AskToInteract()
    {
        ConstructionManager.InteractWithStructure(gameObject);
    }

    [ContextMenu("Close")]
    public void CloseStructure()
    {
        isOpen = false;
        OnCloseStructure?.Invoke();
    }
    [ContextMenu("Open")]
    public void OpenStructure()
    {
        isOpen = true;
        OnOpenStructure?.Invoke();
    }

    /// <summary>
    /// Add 1 visitors to the Infrastructure and Update the UI if it's shown
    /// </summary>
    public void AddTotalVisitors()
    {
        infraDataRunTime.visitorsTotal += 1;
        UIManager.UpdateTotalNbVisitors();
        UIManager.UpdateCurrentNbVisitors();            //Add 1 
    }

    public void RemoveVisitors()
    {
        if (UIManager.GetInfrastructureInfo.savedInfrastructure == this)
        {
            UIManager.GetInfrastructureInfo.UpdateCurrentNbInfo(this);              //Remove 1
        }
    }
}
