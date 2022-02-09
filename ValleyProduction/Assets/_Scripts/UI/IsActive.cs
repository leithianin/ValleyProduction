using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActive : MonoBehaviour
{
    public void ChangeState()
    {
        if(gameObject.activeSelf) { gameObject.SetActive(false); }
        else { gameObject.SetActive(true); }
    }
}
