using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Area
{
    /// Liste des différents types de datas prit en compte dans l'Area.
    public List<AreaData> datas = new List<AreaData>();
    /// Position dans la grille.
    public Vector2Int arrayPosition;
    /// Position dans le monde.
    public Vector2 worldPosition;
    /// Position dans le monde, convertie en Vecteur 3.
    public Vector3 GetWorldPosition => new Vector3(worldPosition.x, 0, worldPosition.y);

    /// <summary>
    /// Récupère le gestionaire de data du type voulut.
    /// </summary>
    /// <typeparam name="T">Type de la data voulue.</typeparam>
    /// <returns>Le gestionaire de data du type voulut.</returns>
    public List<AreaData<T>> GetData<T>()
    {
        List<AreaData<T>> toReturn = new List<AreaData<T>>();
        for (int i = 0; i < datas.Count;i++)
        {
            AreaData<T> toTest = datas[i] as AreaData<T>;
            if (toTest != null)
            {
                toReturn.Add(toTest);
            }
        }
        return toReturn;
    }

    /// <summary>
    /// Détecte tous les AreaDisplay dans la zone et affecte l'Area à ces derniers.
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
                //Debug.Log(AreaManager.GetAreaAtPosition(displays[j].transform.position).worldPosition.ToString("F4"));
                if (AreaManager.GetAreaAtPosition(displays[j].Position) == this)
                {
                    displays[j].AffectToArea(datas);
                    //hitColliders[i].enabled = false;
                }
            }
        }
    }
}
