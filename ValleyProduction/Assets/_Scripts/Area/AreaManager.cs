using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private Vector2 worldDimension;
    [SerializeField] private float areaHeight;

    [SerializeField] private LayerMask areaDisplayMask;

    public List<Area> areas = new List<Area>();

    private void Start()
    {
        CreateGrid();
    }

    [ContextMenu("Create Grid")]
    private void CreateGrid()
    {
        areas = new List<Area>();

        int areaColumnNumber = Mathf.RoundToInt(worldDimension.x / areaHeight);
        int areaLineNumber = Mathf.RoundToInt(worldDimension.y / areaHeight);

        for(int i = 0; i < areaColumnNumber; i++)
        {
            for(int j = 0; j < areaLineNumber; j++)
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

    public List<VisitorBehavior> visitor;
    [ContextMenu("Add score")]
    private void TestScore()
    {
        for (int i = 0; i < areas.Count; i++)
        {
            for (int j = 0; j < visitor.Count; j++)
            {
                areas[i].GetData<VisitorBehavior>().AddData(visitor[j]);
            }
        }
    }

    [ContextMenu("Remove score")]
    private void TestScoreRemove()
    {
        for (int i = 0; i < areas.Count; i++)
        {
            for (int j = 0; j < visitor.Count; j++)
            {
                areas[i].GetData<VisitorBehavior>().RemoveData(visitor[j]);
            }
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
