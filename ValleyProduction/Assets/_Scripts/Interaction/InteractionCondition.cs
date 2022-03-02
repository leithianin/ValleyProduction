using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionCondition : MonoBehaviour
{
    public abstract bool IsConditionTrue(CPN_InteractionHandler interacter);
}
