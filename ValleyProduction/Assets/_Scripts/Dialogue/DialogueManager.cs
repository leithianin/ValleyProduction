using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : VLY_Singleton<DialogueManager>
{
    public ELEMENTS elements;

    public GameObject textBlock;                                            //Block input interaction 
    public static bool isSpeaking => instance.speaking != null;

    [Header("Value")]
    public float dialogueWaitingTime = 1f;
    public float textSpeed = 0.02f;
    public float vignetteValue = 100f;
    //public float closeSpeed = 2f;

    //Private variable
    private Coroutine speaking = null;
    private int index;
    private string currentId;

    private bool waitingInput = false;
    private bool wantToSkip = false;
    private bool speak = false;

    [Header("Events")]
    public UnityEvent OnEndDialogue;

    [System.Serializable]
    public class ELEMENTS
    {
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI indicationInputText;
    }

    public static GameObject dialoguePanel => instance.elements.dialoguePanel;
    public static TextMeshProUGUI dialogueText => instance.elements.dialogueText;
    public static TextMeshProUGUI nameText => instance.elements.nameText;
    public static TextMeshProUGUI indicationInputText => instance.elements.indicationInputText;

    private void Start()
    {
        //PlayDialogue("PTD_000");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetWantToSkip();
        }
    }

    public void PlayDialogue(string id)
    {
        indicationInputText.text = "<i> Click to speed up";
        CameraManager.SetVignettage(vignetteValue);
        textBlock.gameObject.SetActive(true);
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

            indicationInputText.text = "<i> Click to speed up";
            Say(text, speaker);
            index++;
        }
        else
        {
            TimerManager.CreateRealTimer(0.2f, () => textBlock.gameObject.SetActive(false));
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
                indicationInputText.text = "<i> Click to skip";
                yield return new WaitForEndOfFrame();
            }
            else
            {
                dialogueText.text += dialogue[dialogueText.text.Length];
                yield return new WaitForSeconds(textSpeed);
            }
        }

        indicationInputText.text = "<i> Click to skip";
        speak = false;
        wantToSkip = false;
        waitingInput = true;
        //Dialogue Over
        yield return new WaitForSeconds(dialogueWaitingTime);
    }

    public void CloseDialogue()
    {
        CameraManager.SetVignettage(0f);
        dialoguePanel.SetActive(false);
    }

    public void SetWantToSkip()
    {
        if (speak)
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
