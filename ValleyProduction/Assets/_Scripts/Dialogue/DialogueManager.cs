using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : VLY_Singleton<DialogueManager>
{
    public ELEMENTS elements;
    private Coroutine speaking = null;
    public static bool isSpeaking => instance.speaking != null;

    public float dialogueWaitingTime = 1f;
    public float textSpeed = 0.02f;
    //public float closeSpeed = 2f;
    private int index;
    private string currentId;

    public bool waitingInput = false;
    public bool wantToSkip = false;
    public bool speak = false;

    public UnityEvent OnEndDialogue;

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
        //PlayDialogue("PTD_000");
    }

    public void PlayDialogue(string id)
    {
        //StopCoroutine(CloseDialogue());
        StopAllCoroutines();
        if (!isSpeaking)
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
            Debug.Log("EndDialogue");
            CloseDialogue();
            StopSpeaking();
            OnEndDialogue?.Invoke();
        }
    }

    public static void Say(string text, string speaker)
    {
        instance.StopSpeaking();
        instance.speaking = instance.StartCoroutine(instance.Speaking(text, speaker));
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
        speak = true;

        while(dialogueText.text != dialogue)
        {
            if (wantToSkip)
            {
                dialogueText.text = dialogue;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                dialogueText.text += dialogue[dialogueText.text.Length];
                yield return new WaitForSeconds(textSpeed);
            }
        }

        speak = false;
        wantToSkip = false;
        waitingInput = true;
        //Dialogue Over
        yield return new WaitForSeconds(dialogueWaitingTime);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void SetWantToSkip()
    {
        if (speaking != null)
        {
            wantToSkip = true;
        }

        if (waitingInput)
        {
            waitingInput = false;
            NextDialogue();
        }
    }
}
