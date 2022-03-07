using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SatisfactionGiver : VLY_Component<CPN_Data_SatisfactionGiver>
{
    [SerializeField] private float satisfactionGiven;

    [SerializeField] private BuildTypes satisfactionType;

    public float SatisfactionGiven => satisfactionGiven;

    public BuildTypes SatisfactionGiverType => satisfactionType;

    public override void SetData(CPN_Data_SatisfactionGiver dataToSet)
    {
        satisfactionGiven = dataToSet.SatisfactionGiven();

        satisfactionType = dataToSet.InteractionTypeUsed();
    }

    public bool IsSameInteractionType(BuildTypes toCheck)
    {
        return satisfactionType == toCheck;
    }


}
