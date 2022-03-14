using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Texts", menuName = "Valley/Texts/TextsDictionary")]
public class TextsDictionary : ScriptableObject
{
    [SerializeField] private TextBase[] pathTutorial;
    [SerializeField] private TextBase[] ecosystemTutorial;
    [SerializeField] private TextBase[] infrastructureTutorial;
    [SerializeField] private TextBase[] quests;
    [SerializeField] private TextBase blank;

    public TextBase GetTextAsset(string id)
    {
        var fragId = id.Substring(0, 3);

        switch (fragId)
        {
            case "PTH":
                foreach (TextBase txt in pathTutorial)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;

            case "STM":
                foreach (TextBase txt in ecosystemTutorial)
                {
                    if (txt.Id.Equals(id))
                    {
                        return txt;
                    }
                }
                break;

            case "INF":
                foreach(TextBase txt in infrastructureTutorial)
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
