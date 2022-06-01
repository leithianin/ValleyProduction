using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashStretchAnimation : MonoBehaviour
{
    public AnimationCurve squashStretchCurve;
    public Renderer renderer;

    public float animLength;
    private float currentLength = 0;

    private MaterialPropertyBlock matBlock;

    private void Update()
    {
        if (currentLength < animLength)
        {
            currentLength += Time.unscaledDeltaTime;
            PlayAnimation();
        }
        else
        {
            enabled = false;
        }
    }

    private void PlayAnimation()
    {
        matBlock.SetFloat("SquashAnim", squashStretchCurve.Evaluate(currentLength / animLength));

        renderer.SetPropertyBlock(matBlock);
    }

    public void StartAnimation()
    {
        matBlock = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(matBlock);

        enabled = true;
    }
}
