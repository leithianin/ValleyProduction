using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceGiver : MonoBehaviour
{

    public void GiveRessource(float amount)
    {
        VLY_RessourceManager.GainRessource(amount);
    }

    public void RemoveRessource(float amount)
    {
        VLY_RessourceManager.LoseRessource(amount);
    }
}
