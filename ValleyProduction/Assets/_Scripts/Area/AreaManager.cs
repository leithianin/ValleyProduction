using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : VLY_Singleton<AreaManager>
{
    /// Dimensions de la map
    [SerializeField] private Vector2 worldDimension;
    /// Taille de la grille
    [SerializeField] private float areaHeight;

    /// Dimensions de la grille
    private static Vector2Int gridDimension;
    /// Layer mask contenant les AreaDisplay
    [SerializeField] private LayerMask areaDisplayMask;

    /// Liste de toutes les zones de la map
    private static List<Area> areas = new List<Area>();

    /// Taille de la map (Getter)
    private static float AreaHeight => instance.areaHeight;

    private void Start()
    {
        CreateGrid();
    }

    /// <summary>
    /// Crée la grille, et assigne tous les AreaDisplay aux zones.
    /// </summary>
    [ContextMenu("Create Grid")]
    private void CreateGrid()
    {
        areas = new List<Area>();

        gridDimension = new Vector2Int(Mathf.RoundToInt(worldDimension.x / areaHeight), Mathf.RoundToInt(worldDimension.y / areaHeight));

        for (int i = 0; i < gridDimension.x; i++)
        {
            for(int j = 0; j < gridDimension.y; j++)
            {
                Area newArea = new Area();
                newArea.arrayPosition = new Vector2Int(i, j);
                newArea.worldPosition = new Vector2(areaHeight / 2f + areaHeight * i, areaHeight / 2f + areaHeight * j);

                newArea.datas.Add(new AD_Noise());

                newArea.SetAllDisplay(areaHeight, areaDisplayMask);

                areas.Add(newArea);
            }
        }
    }

    /// <summary>
    /// Permet de récupérer une Area depuis une position (x,z).
    /// </summary>
    /// <param name="position">Position de recherche (x,z).</param>
    /// <returns>L'Area dans laquelle la position se trouve.</returns>
    public static Area GetAreaAtPosition(Vector2 position)
    {
        int columnIndex = Mathf.RoundToInt((position.x - AreaHeight / 2) / AreaHeight);
        int lineIndex = Mathf.RoundToInt((position.y - AreaHeight / 2) / AreaHeight);

        int realIndex = columnIndex * gridDimension.y + lineIndex;

        if (realIndex >= 0 && realIndex < areas.Count)
        {
            return areas[realIndex];
        }

        return null;
    }

    /// <summary>
    /// Ajoute une data à l'Area voulue.
    /// </summary>
    /// <typeparam name="T">Type de la data à ajouter.</typeparam>
    /// <param name="toUpdate">L'Area à update.</param>
    /// <param name="dataToUpdate">La data à ajouter.</param>
    public static void AddDataToArea<T>(Area toUpdate, T dataToUpdate)
    {
        if (toUpdate != null)
        {
            toUpdate.GetData<T>().AddData(dataToUpdate);
        }
    }

    /// <summary>
    /// Retire une data à l'Area voulue.
    /// </summary>
    /// <typeparam name="T">Le type de data à retirer.</typeparam>
    /// <param name="toUpdate">L'Area à update.</param>
    /// <param name="dataToUpdate">La data à retirer.</param>
    public static void RemoveDataToArea<T>(Area toUpdate, T dataToUpdate)
    {
        if (toUpdate != null)
        {
            toUpdate.GetData<T>().RemoveData(dataToUpdate);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < areas.Count; i++)
        {
            Gizmos.DrawWireCube(areas[i].GetWorldPosition, Vector3.one * areaHeight);
        }
    }
}
