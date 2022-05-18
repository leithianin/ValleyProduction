using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    int index = 0;

    public string[] s = new string[]
    {
        "Hey yo",
        "dqdqdqsdqsdqd",
        "dd dd dd "
    };

    private void Start()
    {
        if (!DialogueManager.isSpeaking || DialogueManager.isWaitingForUserInput)
        {
            if (index >= s.Length)
            {
                return;
            }

            Say(TextsDictionary.instance.GetTextAsset("PTD_000").Texts[index]);
            //Say(s[index]);
            index++;
        }
    }

    void Say(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        DialogueManager.Say(speech, speaker);
    }
}
