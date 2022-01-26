using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShader : MonoBehaviour, IFeedbackPlayer
{
    public Renderer renderer;
    public AnimationCurve curve;
    public string shader_variable_name;

    private MaterialPropertyBlock materialBlock;

    private float timer;
    private float base_value;

    void Update()
    {
        timer += Time.deltaTime;
        if (curve[curve.length - 1].time < timer)
        {
            enabled = false;
        }
        else
        {
            materialBlock.SetFloat(shader_variable_name, curve.Evaluate(timer));

            renderer.SetPropertyBlock(materialBlock);
        }
    }

    public void Play()
    {
        materialBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(materialBlock);
        base_value = materialBlock.GetFloat(shader_variable_name);
        timer = 0f;
        enabled = true;
    }
}
