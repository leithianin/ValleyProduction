using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VLY_ComponentHandler : MonoBehaviour
{
    [SerializeField] private List<VLY_Component> components;

    public T GetComponentOfType<T>() where T : VLY_Component
    {
        for(int i = 0; i < components.Count; i++)
        {
            if(components[i] != null && components[i] is T)
            {
                return components[i] as T;
            }
        }
        return null;
    }

    public void AddComponent(VLY_Component toAdd)
    {
        if(!components.Contains(toAdd))
        {
            components.Add(toAdd);
        }
    }

    public void RemoveComponent(VLY_Component toRemove)
    {
        if (components.Contains(toRemove))
        {
            components.Remove(toRemove);
        }
    }

    public void ActiveOutline()
    {
        CursorTextureManager.SetInteractionCursor();
        AnimationHandler test = GetComponentOfType<AnimationHandler>();
        test.outline.enabled = true;
    }

    public void DesactiveOutline()
    {
        CursorTextureManager.SetNormalCursor();
        AnimationHandler test = GetComponentOfType<AnimationHandler>();
        test.outline.enabled = false;
    }

    public void BlinkOnOutline()
    {
        AnimationHandler test = GetComponentOfType<AnimationHandler>();
        test.outline.enabled = true;
    }

    public void BlinkOffOutline()
    {
        AnimationHandler test = GetComponentOfType<AnimationHandler>();
        test.outline.enabled = false;
    }
}
