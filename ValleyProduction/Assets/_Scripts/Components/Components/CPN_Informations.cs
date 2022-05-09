using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeVisitor { Hiker, Tourist}
public class CPN_Informations : VLY_Component<VisitorScriptable>
{
    public VisitorScriptable scriptable = null;
    public VLY_ComponentHandler components;

    public TypeVisitor visitorType;
    private string name = "Robert";
    private string goal = "Je veux atteindre PlaceHolder";
    private string note = "J'aime bien PlaceHolder";

    public string GetName => name;
    public string GetGoal => goal;
    public string GetNote => note;

    //Genere aléatoirement les infos
    public void OnEnable()
    {
        name = GeneratorManager.GetRandomVisitorName();
    }

    public void DisplayInformation()
    {
        UIManager.InteractWithVisitor(this);
    }

    public override void SetData(VisitorScriptable dataToSet)
    {
        scriptable = dataToSet;
    }
}
