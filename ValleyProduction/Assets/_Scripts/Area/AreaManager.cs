using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
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
}
