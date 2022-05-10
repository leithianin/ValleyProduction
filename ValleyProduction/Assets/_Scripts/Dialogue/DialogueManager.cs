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
    private int index = 0;
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

    private void Start()
    {
        PlayDialogue("PTD_000");
    }

    private void PlayDialogue(string id)
    {
        if (!isSpeaking || isWaitingForUserInput)
        {
            currentId = id;

            if (index >= TextsDictionary.instance.GetTextAsset(id).Texts.Length)
            {
                dialoguePanel.SetActive(false);
                return;
            }

            string text = TextsDictionary.instance.GetTextAsset(id).Texts[index];
            string speaker = TextsDictionary.instance.GetTextAsset(id).Title;

            Say(text, speaker);
            index++;
        }
    }

    private void NextDialogue()
    {
        if (index <= TextsDictionary.instance.GetTextAsset(currentId).Texts.Length)
        {
            string text = TextsDictionary.instance.GetTextAsset(currentId).Texts[index];
            string speaker = TextsDictionary.instance.GetTextAsset(currentId).Title;

            Say(text, speaker);
            index++;
        }
        else
        {
            StopSpeaking();
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
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Over");
        yield return new WaitForSeconds(1f);
        NextDialogue();
    }
}
