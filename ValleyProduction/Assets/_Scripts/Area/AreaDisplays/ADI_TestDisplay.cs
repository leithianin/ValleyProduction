using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADI_TestDisplay : AreaDisplay
{
    public int scoreLow, scoreHigh;

    public override void OnUpdateScore(int newScore)
    {
        if(newScore < scoreLow)
        {
            Debug.Log("Score Low");
        }
        else if(newScore > scoreHigh)
        {
            Debug.Log("Score High");
        }
        else
        {
            Debug.Log("Score Middle");
        }
    }
}
