using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Texts", menuName = "Valley/Texts/TextsDictionary")]
public class TextsDictionary : ScriptableObject
{
    [SerializeField] private TextBase[] pathTutorial;
    [SerializeField] private TextBase[] ecosystemTutorial;
    [SerializeField] private TextBase[] quests;

    public TextBase GetTextAsset(string id)
    {
        var fragId = id.Substring(0, 2);

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

            case "QST":
                foreach (TextBase txt in quests)
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
