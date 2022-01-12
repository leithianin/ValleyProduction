using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private Vector2 worldDimension;
    [SerializeField] private float areaHeight;

    public List<Area> areas = new List<Area>();
    public Area areaTest = new Area();

    public CPN_Movement movement;
    public VisitorBehavior visitor;

    private void Start()
    {
        AD_Visitor visitorData = new AD_Visitor();
        AD_Noise noiseData = new AD_Noise();

        areaTest.datas.Add(visitorData);
        areaTest.datas.Add(noiseData);

        areaTest.GetData<CPN_Movement>().AddData(movement);
        areaTest.GetData<VisitorBehavior>().AddData(visitor);
    }

    [ContextMenu("Create Grid")]
    private void CreateGird()
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

                newArea.SetAllDisplay(areaHeight);

                areas.Add(newArea);
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
