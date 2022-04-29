using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeContext : MonoBehaviour
{
    public void SetContext(int i)
    {
        VLY_ContextManager.ChangeContext(i);
    }
}
