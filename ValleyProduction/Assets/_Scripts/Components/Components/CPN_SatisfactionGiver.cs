using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SatisfactionGiver : VLY_Component<CPN_Data_SatisfactionGiver>
{
    [SerializeField] private float satisfactionGiven;

    [SerializeField] private SatisfactorType satisfactionType;

    public float SatisfactionGiven => satisfactionGiven;

    public SatisfactorType SatisfactionGiverType => satisfactionType;

    public override void SetData(CPN_Data_SatisfactionGiver dataToSet)
    {
        satisfactionGiven = dataToSet.SatisfactionGiven();

        satisfactionType = dataToSet.InteractionTypeUsed();
    }

    public bool IsSameInteractionType(SatisfactorType toCheck)
    {
        return satisfactionType == SatisfactorType.All || toCheck == SatisfactorType.All || satisfactionType == toCheck;
    }
}
