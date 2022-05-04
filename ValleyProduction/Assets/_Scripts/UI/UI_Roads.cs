using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Roads : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public void UpdateName(string name)
    {
        nameText.text = name;
    }
}
