using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnablePSPlay : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
