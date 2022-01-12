using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area
{
    public List<AreaData> datas = new List<AreaData>();

    public Vector2Int arrayPosition;

    public Vector2 worldPosition;

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

    public void GetAllData(float zoneLength)
    {
        Collider[] hitColliders = Physics.OverlapBox(worldPosition, Vector3.one * zoneLength, Quaternion.identity, LayerMask.NameToLayer("BaseData"));


    }
}
