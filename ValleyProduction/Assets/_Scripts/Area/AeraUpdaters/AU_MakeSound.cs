using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_MakeSound : AreaUpdater<CPN_NoiseMaker>
{
    /// <summary>
    /// Ajoute NewData � la data actuelle.
    /// </summary>
    /// <param name="newData">Le Data � ajouter.</param>
    public override void SetData(CPN_NoiseMaker newData)
    {
        data = newData;
    }
}
 