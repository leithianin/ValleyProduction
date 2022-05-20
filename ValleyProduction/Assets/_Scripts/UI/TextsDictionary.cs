using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Texts", menuName = "Valley/Texts/TextsDictionary")]
public class TextsDictionary : ScriptableObject
{
    public static TextsDictionary instance;

    [SerializeField] private TextBase[] baseDialogue;
    [SerializeField] private TextBase[] advanceDialogue;
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
            case "PTD":
                foreach (TextBase txt in baseDialogue)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;

            case "ADD":
                foreach(TextBase txt in advanceDialogue)
                {
                    if(txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;

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
}
