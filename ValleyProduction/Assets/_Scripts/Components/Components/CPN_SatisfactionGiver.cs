using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SatisfactionGiver : VLY_Component<CPN_Data_SatisfactionGiver>
{
    [SerializeField] private float satisfactionGiven;

    [SerializeField] private InteractionType interactionType;

    public float SatisfactionGiven => satisfactionGiven;

    public override void SetData(CPN_Data_SatisfactionGiver dataToSet)
    {
        satisfactionGiven = dataToSet.SatisfactionGiven();

        interactionType = dataToSet.InteractionTypeUsed();
    }

    public bool IsSameInteractionType(InteractionType toCheck)
    {
        return interactionType == toCheck;
    }


}
