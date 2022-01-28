using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_PlayShaderBoolean : MonoBehaviour, IFeedbackPlayer
{
    [SerializeField] private bool state;

    [SerializeField] private Renderer renderer;
    [SerializeField] private string shader_variable_name;

    private MaterialPropertyBlock materialBlock;

    public void Play()
    {
        materialBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(materialBlock);

        materialBlock.SetFloat(shader_variable_name, state?1f:0f);

        renderer.SetPropertyBlock(materialBlock);
    }

    public void SetShader(bool value)
    {
        state = value;
        Debug.Log(state);
        Play();
    }
}
