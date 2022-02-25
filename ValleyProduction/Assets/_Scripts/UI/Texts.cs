using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Texts", menuName = "Valley/Texts/TextsDictionary")]
public class Texts : ScriptableObject
{
    [SerializeField] private TextBase[] pathTutorial;
    [SerializeField] private TextBase[] ecosystemTutorial;
    [SerializeField] private TextBase[] quests;
}
