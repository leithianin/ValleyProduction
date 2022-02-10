using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionManager : VLY_Singleton<PollutionManager>
{
    [SerializeField] private PollutionTrash trashPrefab;
    [SerializeField] private Transform trashHandler;

    private List<PollutionTrash> availableTrashes = new List<PollutionTrash>();

    public static void PickUpTrash(PollutionTrash toPickUp)
    {
        instance.availableTrashes.Add(toPickUp);
    }

    public static void ThrowTrash(Vector3 wantedPosition)
    {
        PollutionTrash toThrow = instance.GetUsableTrash();
        instance.availableTrashes.Remove(toThrow);

        toThrow.Throw(wantedPosition);
    }

    private PollutionTrash GetUsableTrash()
    {
        if(availableTrashes.Count > 0)
        {
            return availableTrashes[0];
        }
        return CreateTrash();
    }

    private PollutionTrash CreateTrash()
    {
        PollutionTrash toReturn = Instantiate(trashPrefab, trashHandler);
        return toReturn;
    }
}
