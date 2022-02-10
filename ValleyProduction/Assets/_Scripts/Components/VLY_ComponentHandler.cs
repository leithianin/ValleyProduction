using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_ComponentHandler : MonoBehaviour
{
    [SerializeField] private List<VLY_Component> components;

    public void GetComponentOfType<T>(ref T wantedComponent) where T : VLY_Component
    {
        for(int i = 0; i < components.Count; i++)
        {
            if(components[i] as T != null)
            {
                wantedComponent = components[i] as T;
                break;
            }
        }
    }

    [ContextMenu("Test 1")]
    public void Test1()
    {
        CPN_Stamina stam = null;
        GetComponentOfType<CPN_Stamina>(ref stam);

        if(stam != null)
        {
            Debug.Log(stam.GetStamina);
        }
    }

    [ContextMenu("Test 2")]
    public void Test2()
    {
        CPN_Movement stam = null;
        GetComponentOfType<CPN_Movement>(ref stam);

        Debug.Log(stam.gameObject.name);
    }
}
