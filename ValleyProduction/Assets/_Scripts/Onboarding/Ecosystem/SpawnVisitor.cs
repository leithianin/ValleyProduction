using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVisitor : MonoBehaviour
{
    public AnimationHandler visitorDisplay;
    public void PlayMovementAnimation()
    {
        visitorDisplay.PlayBodyAnim(BodyAnimationType.Walk);
    }
    public void StopMovementAnimation()
    {
        visitorDisplay.StopBodyAnim(BodyAnimationType.Walk);
    }
}
