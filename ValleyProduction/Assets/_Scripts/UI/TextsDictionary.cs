using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Texts", menuName = "Valley/Texts/TextsDictionary")]
public class TextsDictionary : ScriptableObject
{
    public static TextsDictionary instance;

    [SerializeField] private TextDialogue[] baseDialogue;
    [SerializeField] private TextDialogue[] advanceDialogue;
    [SerializeField] private TextDialogue[] questDialogue;
    [SerializeField] private TextBase[] quests;
    [SerializeField] private TextBase blank;
    public TextsDictionary()
    {
        instance = this;
    }
    public TextBase GetTextAsset(string id)
    {
        var fragId = id.Substring(0, 3);

        switch (fragId)
        {
            case "QST":
                foreach (TextBase txt in quests)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;
            case "BLK":
                return blank;
        }

        return null;
    }

    public TextDialogue GetDialogueAsset(string id)
    {
        var fragId = id.Substring(0, 3);

        switch (fragId)
        {
            case "PTD":
                foreach (TextDialogue txt in baseDialogue)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;

            case "ADD":
                foreach (TextDialogue txt in advanceDialogue)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;
            case "QST":
                foreach (TextDialogue txt in questDialogue)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;
        }

        return null;
    }
}
