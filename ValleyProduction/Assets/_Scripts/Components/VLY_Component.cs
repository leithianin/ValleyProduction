using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VLY_Component : MonoBehaviour
{

}

public abstract class VLY_Component<T> : VLY_Component
{
    public abstract void SetData(T dataToSet);
}
