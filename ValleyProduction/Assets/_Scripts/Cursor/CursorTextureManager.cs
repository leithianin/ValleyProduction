using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTextureManager: VLY_Singleton<CursorTextureManager>
{
    [Header("Normal Cursor")]
    public Texture2D T_normal;
    public CursorMode normalCursorMode = CursorMode.Auto;
    public Vector2 normalHotSpot;


    [Header("Interaction Cursor")]
    public Texture2D T_interaction;
    public Texture2D T_interactionPressed;
    public CursorMode interactionCursorMode = CursorMode.Auto;
    public Vector2 interactionHotSpot;

    private bool isInteraction = false;

    public static void SetNormalCursor()
    {
        instance.isInteraction = false;
        Cursor.SetCursor(instance.T_normal, instance.normalHotSpot, instance.normalCursorMode);
    }

    public static void SetInteractionCursor()
    {
        instance.isInteraction = true;
        Cursor.SetCursor(instance.T_interaction, instance.interactionHotSpot, instance.interactionCursorMode);
    }

    public static void SetPressedCursor()
    {
        if(instance.isInteraction)
        {
            Cursor.SetCursor(instance.T_interactionPressed, instance.interactionHotSpot, instance.interactionCursorMode);
        }
    }

    public static void SetReleaseCursor()
    {
        if(instance.isInteraction)
        {
            SetInteractionCursor();
        }
        else
        {
            SetNormalCursor();
        }
    }
}
