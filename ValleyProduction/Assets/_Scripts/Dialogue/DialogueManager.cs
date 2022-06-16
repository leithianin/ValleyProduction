using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : VLY_Singleton<DialogueManager>
{
    public ELEMENTS elements;

    public GameObject textBlock;                                            //Block input interaction 
    public static bool isSpeaking => instance.speaking != null;

    public Image woodyImage;

    private List<string> texts = new List<string>();

    [Header("Value")]
    public float dialogueWaitingTime = 1f;
    public float textSpeed = 0.02f;

    [Header("Vignette")]
    public float timeTransition;
    private float currentTimeTransition = 0f;
    public float vignetteValue = 100f;
    private bool startVignettage = false;
    private bool endVignettage   = false;
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
    public UnityEvent OnStartDialogue;

    private TimerManager.Timer currentDialogTimer;

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

    private void Update()
    {
        if(startVignettage)
        {
            currentTimeTransition += Time.deltaTime;
            //CameraManager.SetVignettage(Mathf.Lerp(0f, vignetteValue, currentTimeTransition / timeTransition));

            if(currentTimeTransition > timeTransition) 
            {
                currentTimeTransition = 0f;
                startVignettage = false; 
            }
        }

        if(endVignettage)
        {
            currentTimeTransition += Time.deltaTime;

            //CameraManager.SetVignettage(Mathf.Lerp(vignetteValue, 0f, currentTimeTransition / timeTransition));

            if (currentTimeTransition > timeTransition) 
            {
                currentTimeTransition = 0f;
                endVignettage = false; 
            }
        }
    }

    public static void PlayDialogueForTime(string id, float dialogTime)
    {
        instance.DisplayDialogueForTime(id, dialogTime);
    }

    public static void PlayDialogue(string id)
    {
        instance.DisplayDialogue(id);
    }

    public void DisplayDialogueForTime(string id, float dialogTime)
    {
        if(currentDialogTimer != null)
        {
            currentDialogTimer.Stop();
            currentDialogTimer = null;
        }

        currentDialogTimer = TimerManager.CreateRealTimer(dialogTime, NextDialogue);

        DisplayDialogue(id);
    }

    public void DisplayDialogue(string id)
    {
        indicationInputText.text = "<i> Click to speed up";
        startVignettage = true;
        if (textBlock != null)
        {
            textBlock.gameObject.SetActive(true);
        }
        OnStartDialogue?.Invoke();

        StopSpeaking();

        switch (UIManager.GetData.lang)
        {
            case Language.en:
                Debug.Log(TextsDictionary.instance);
                Debug.Log(TextsDictionary.instance.GetDialogueAsset(id));


                foreach (string str in TextsDictionary.instance.GetDialogueAsset(id).Textsen)
                {
                    texts.Add(str);
                }
                break;

            case Language.fr:
                if (TextsDictionary.instance.GetDialogueAsset(id).Textsfr[0] != string.Empty)
                {
                    foreach (string str in TextsDictionary.instance.GetDialogueAsset(id).Textsfr)
                    {
                        texts.Add(str);
                    }
                }
                else
                {
                    foreach (string str in TextsDictionary.instance.GetDialogueAsset(id).Textsen)
                    {
                        texts.Add(str);
                    }
                }
                break;
        }

        if (!isSpeaking)
        {
            index = 0;
            currentId = id;

            if (TextsDictionary.instance.GetDialogueAsset(id).Behavior != null)
            {
                woodyImage.sprite = TextsDictionary.instance.GetDialogueAsset(id).Behavior;
            }

            string text = texts[index];
            string speaker = TextsDictionary.instance.GetDialogueAsset(id).Title;

            Say(text, speaker);
            index++;
        }     
    }

    private void NextDialogue()
    {
        if (index < texts.Count)
        {
            string text = texts[index];
            string speaker = TextsDictionary.instance.GetDialogueAsset(currentId).Title;

            indicationInputText.text = "<i> Click to speed up";
            Say(text, speaker);
            index++;
        }
        else
        {
            EndDialog();
        }
    }

    private void EndSpeak()
    {
        speak = false;
        wantToSkip = false;
        waitingInput = true;
    }

    private void EndDialog()
    {
        if (textBlock != null)
        {
            TimerManager.CreateRealTimer(0.2f, () => textBlock.gameObject.SetActive(false));
        }
        CloseDialogue();
        texts.Clear();
        StopSpeaking();
        OnEndDialogue?.Invoke();
    }

    public static void Say(string text, string speaker)
    {
        instance.StopSpeaking();
        instance.speaking = instance.StartCoroutine(instance.Speaking(text, speaker));
    }

    public void StopSpeaking()
    {
        EndSpeak();

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
        EndSpeak();
        //Dialogue Over
        yield return new WaitForSeconds(dialogueWaitingTime);
    }

    public void CloseDialogue()
    {
        endVignettage = true;
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
