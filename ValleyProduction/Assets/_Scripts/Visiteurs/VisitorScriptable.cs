using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Visitor/Create Visitor")]
public class VisitorScriptable : ScriptableObject
{
    [SerializeField, Tooltip("A list of all available skin for the visitor. We take a random one for each visitor.")] private List<AnimationHandler> display;
    [SerializeField, Tooltip("The speed of the visitor.")] private float speed;
    /*[SerializeField, Tooltip("The noise produced by the visitor.")] private float soundProduced;

    [SerializeField, Tooltip("The amount of satisfaction the visitor gain will just walking.")] private float satisfactionByTenSecond;
    [SerializeField, Tooltip("The amount of satisfaction the visitor gain when he sees an other visitor.")] private float satisfactionNextToOthers;*/

    /// <summary>
    /// Get a random skin for the visitor.
    /// </summary>
    public AnimationHandler Display => display[Random.Range(0, display.Count)];
    public float Speed => speed;
    /*public float SoundProduced => soundProduced;
    public float SatisfactionUpdate => satisfactionByTenSecond;
    public float SatisfactionNextToOther => satisfactionNextToOthers;*/
}
