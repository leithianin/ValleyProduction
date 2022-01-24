using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShader : MonoBehaviour, IFeedbackPlayer
{
    public Material shaderMaterial;
    public AnimationCurve curve;
    public string shader_variable_name;

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
            shaderMaterial.SetFloat(shader_variable_name, curve.Evaluate(timer)); 
        }
    }

    public void Play()
    {
       
        base_value = shaderMaterial.GetFloat(shader_variable_name);
        timer = 0f;
        enabled = true;
    }
}
