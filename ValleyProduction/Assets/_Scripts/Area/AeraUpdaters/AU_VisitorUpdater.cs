using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_VisitorUpdater : AreaUpdater<int>
{
    /// <summary>
    /// Ajoute NewData à la data actuelle.
    /// </summary>
    /// <param name="newData">Le Data à ajouter.</param>
    public override void SetData(int newData)
    {
        data += newData;
    }
}
 