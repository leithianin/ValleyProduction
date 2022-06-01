using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeBehavior : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshes;

    [SerializeField] private float lerpCoef;

    [SerializeField] private UnityEvent OnTreeAlive;
    [SerializeField] private UnityEvent OnTreeDead;

    private int currentPhase;
    private float lastLerpValue;

    private float currentInterpolate = 0;
    private float lerpValue = 0;

    public int CurrentPhase => currentPhase;

    [ContextMenu("Set Mesh List")]
    private void GetAllMeshes()
    {
        meshes = new List<MeshRenderer>(transform.GetComponentsInChildren<MeshRenderer>());
    }

    public void SetTreePhase(int phase)
    {
        if (phase != currentPhase)
        {
            if(phase < currentPhase)
            {
                foreach(MeshRenderer mesh in meshes)
                {
                    //mesh.material.EnableKeyword("_BLOSSOM");
                }
            }
            else
            {
                foreach (MeshRenderer mesh in meshes)
                {
                    //mesh.material.DisableKeyword("_BLOSSOM");
                }
            }

            if(phase >= 3)
            {
                OnTreeDead?.Invoke();
            }
            else if(currentPhase >= 3)
            {
                OnTreeAlive?.Invoke();
            }

            lastLerpValue = lerpValue;

            currentInterpolate = 0;

            currentPhase = phase;

            enabled = true;
        }
    }

    private void Update()
    {
        currentInterpolate += Time.deltaTime * lerpCoef;
        lerpValue = Mathf.Lerp(lastLerpValue, currentPhase, currentInterpolate);

        if (currentInterpolate >= 1)
        {
            enabled = false;
        }

        foreach (MeshRenderer mesh in meshes)
        {
            SetTransitionState(mesh);
        }
    }

    private void SetBlossom(Renderer toSet, bool value)
    {

    }

    private void SetTransitionState(Renderer toSet)
    {
         MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();

        toSet.GetPropertyBlock(materialBlock);

        materialBlock.SetFloat("TRANSITION_STATE", lerpValue);

        //materialBlock.SetFloat(shader_variable_name, state ? 1f : 0f);

        toSet.SetPropertyBlock(materialBlock);
    }
}
