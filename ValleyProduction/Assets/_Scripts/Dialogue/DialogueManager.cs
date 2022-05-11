using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : VLY_Singleton<DialogueManager>
{
    public ELEMENTS elements;
    private Coroutine speaking = null;
    public static bool isSpeaking => instance.speaking != null;
    public static bool isWaitingForUserInput = false;

    public float dialogueWaitingTime = 1f;
    public float textSpeed = 0.02f;
    public float closeSpeed = 2f;
    private int index;
    private string currentId;

    [System.Serializable]
    public class ELEMENTS
    {
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI nameText;
    }

    public static GameObject dialoguePanel => instance.elements.dialoguePanel;
    public static TextMeshProUGUI dialogueText => instance.elements.dialogueText;
    public static TextMeshProUGUI nameText => instance.elements.nameText;

    public void PlayDialogue(string id)
    {
        if (!isSpeaking || isWaitingForUserInput)
        {
            index = 0;
            currentId = id;

            string text = TextsDictionary.instance.GetTextAsset(id).Texts[index];
            string speaker = TextsDictionary.instance.GetTextAsset(id).Title;

            Say(text, speaker);
            index++;
        }
    }

    private void NextDialogue()
    {
        if (index < TextsDictionary.instance.GetTextAsset(currentId).Texts.Length)
        {
            string text = TextsDictionary.instance.GetTextAsset(currentId).Texts[index];
            string speaker = TextsDictionary.instance.GetTextAsset(currentId).Title;

            Say(text, speaker);
            index++;
        }
        else
        {
            StopSpeaking();
            StartCoroutine(CloseDialogue());
        }
    }

    public static void Say(string text, string speaker)
    {
        instance.StopSpeaking();
        instance.StartCoroutine(instance.Speaking(text, speaker));
    }

    public void StopSpeaking()
    {
        if(isSpeaking)
        {
            StopCoroutine(speaking);
        }
        speaking = null;
    }

    IEnumerator Speaking(string dialogue, string speaker)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        nameText.text = speaker;

        while(dialogueText.text != dialogue)
        {
            dialogueText.text += dialogue[dialogueText.text.Length];
            yield return new WaitForSeconds(textSpeed);
            //yield return new WaitForEndOfFrame();
        }

        //Dialogue Over
        yield return new WaitForSeconds(dialogueWaitingTime);
        NextDialogue();
    }

    IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(closeSpeed);
        dialoguePanel.SetActive(false);
    }
}
