using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_MakeSound : AreaUpdater<NoiseMaker>
{
    /// <summary>
    /// Ajoute NewData à la data actuelle.
    /// </summary>
    /// <param name="newData">Le Data à ajouter.</param>
    public override void SetData(NoiseMaker newData)
    {
        data = newData;
    }
}
 