using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_MakeSound : AreaUpdater<int>
{
    /// <summary>
    /// Ajoute NewData � la data actuelle.
    /// </summary>
    /// <param name="newData">Le Data � ajouter.</param>
    public override void SetData(int newData)
    {
        data = newData;
    }
}
 