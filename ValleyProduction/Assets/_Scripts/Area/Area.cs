using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area
{
    /// Liste des diff�rents types de datas prit en compte dans l'Area.
    public List<AreaData> datas = new List<AreaData>();
    /// Position dans la grille.
    public Vector2Int arrayPosition;
    /// Position dans le monde.
    public Vector2 worldPosition;
    /// Position dans le monde, convertie en Vecteur 3.
    public Vector3 GetWorldPosition => new Vector3(worldPosition.x, 0, worldPosition.y);

    /// <summary>
    /// R�cup�re le gestionaire de data du type voulut.
    /// </summary>
    /// <typeparam name="T">Type de la data voulue.</typeparam>
    /// <returns>Le gestionaire de data du type voulut.</returns>
    public AreaData<T> GetData<T>()
    {
        for(int i = 0; i < datas.Count;i++)
        {
            AreaData<T> toTest = datas[i] as AreaData<T>;
            if (toTest != null)
            {
                return toTest;
            }
        }
        return null;
    }

    /// <summary>
    /// D�tecte tous les AreaDisplay dans la zone et affecte l'Area � ces derniers.
    /// </summary>
    /// <param name="zoneLength">Taille d'une Area.</param>
    /// <param name="mask">Layer mask des AreaDisplay.</param>
    public void SetAllDisplay(float zoneLength, LayerMask mask)
    {
        Collider[] hitColliders = Physics.OverlapBox(GetWorldPosition, Vector3.one * zoneLength / 2f, Quaternion.identity, mask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject obj = hitColliders[i].gameObject;

            AreaDisplay[] displays = obj.GetComponents<AreaDisplay>();

            for (int j = 0; j < displays.Length; j++)
            {
                displays[j].AffectToArea(datas);
            }
        }
    }
}
