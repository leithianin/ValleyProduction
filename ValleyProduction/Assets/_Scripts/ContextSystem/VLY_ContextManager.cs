using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_ContextManager : VLY_Singleton<VLY_ContextManager>
{
    [SerializeField] private GameContext contexts;
    [SerializeField] private Camera usedCamera;

    private GameContext.Context currentContext;

    public static void ChangeContext(int ID)
    {
        instance.ChangeCurrentContext(ID);
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
                    InfrastructureManager.SetDisableTool(ToolType.Place, isEnable);
                    break;
                case FeatureLocker.Destruction:
                    InfrastructureManager.SetDisableTool(ToolType.Place, isEnable);
                    break;
                case FeatureLocker.Modification:
                    InfrastructureManager.SetDisableTool(ToolType.Place, isEnable);
                    break;
                case FeatureLocker.Ressource:
                    VLY_RessourceManager.EnableFeature(isEnable);
                    break;
                case FeatureLocker.Satisfaction:
                    AttractivityManager.EnableFeature(isEnable);
                    break;
            }
        }
    }
}
