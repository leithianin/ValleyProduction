using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_ContextManager : VLY_Singleton<VLY_ContextManager>
{
    [SerializeField] private GameContext contexts;
    [SerializeField] private Camera usedCamera;
    [SerializeField] private int startingContext = 0;

    private GameContext.Context currentContext;

    private void Start()
    {
        ChangeContext(startingContext);
    }

    public static void ChangeContext(int ID)
    {
        instance.ChangeCurrentContext(ID);
    }

    public static void ChangeContext(string str)
    {
        int security = instance.contexts.GetContextID(str);

        if (security >= 0)
        {
            instance.ChangeCurrentContext(security);
        }
    }

    private void ChangeCurrentContext(int ID)
    {
        if (currentContext != contexts.GetContext(ID))
        {
            if (currentContext != null)
            {
                DisableOrEnableContext(currentContext, true);
            }

            currentContext = contexts.GetContext(ID);
            usedCamera.eventMask = contexts.GetContextLayers(ID);
            DisableOrEnableContext(currentContext, false);
        }
    }

    private void DisableOrEnableContext(GameContext.Context context, bool isEnable)
    {
        foreach (FeatureLocker lockedFeature in context.lockedFeatures)
        {
            switch (lockedFeature)
            {
                case FeatureLocker.CameraMovement:
                    throw new NotImplementedException();
                    break;
                case FeatureLocker.CameraRotation:
                    throw new NotImplementedException();
                    break;
                case FeatureLocker.Construction:
                    InfrastructureManager.EnableOrDisableTool(ToolType.Place, isEnable);
                    break;
                case FeatureLocker.Destruction:
                    InfrastructureManager.EnableOrDisableTool(ToolType.Delete, isEnable);
                    break;
                case FeatureLocker.Modification:
                    InfrastructureManager.EnableOrDisableTool(ToolType.Move, isEnable);
                    break;
                case FeatureLocker.Ressource:
                    VLY_RessourceManager.EnableFeature(isEnable);
                    break;
                case FeatureLocker.Satisfaction:
                    AttractivityManager.EnableFeature(isEnable);
                    break;
                case FeatureLocker.BlockAllKeyboardInput:
                    PlayerInputManager.EnableOrDisableKeyboard(isEnable);
                    break;
                case FeatureLocker.BlockMouseInput:
                    Debug.Log("IsEnable BlockMouse change to : " + isEnable);
                    PlayerInputManager.blockMouse = isEnable;
                    break;
            }
        }
    }
}
