using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : VLY_Singleton<GeneratorManager>
{
    public NameScriptable nameScript;
    public NameScriptable pathNameScript;

    public static List<string> namePathList = new List<string> { "Boulevard de Cailloux", "Route des Pigeons", "Route des des Érables", "Chemin des Baies", "Pierre qui roule", "Chemin des Étoiles",
                                                        "Les Champs aux Aigles Australs", "Le Territoire aux Ours Gris","Les Territoires aux Scarabées Pacifiques","Le Pâturage Fleuri","Le  Lagon Pétillant",
                                                        "Lac des Carcavers","Lac Vitreux","Creekplains Home","Test de nom super long pour voir jusqu'ou peut aller le nom","&é((àç@|(","The Sweet Spot"};

    public static string GetRandomPathName()
    {
        return instance.pathNameScript.nameList[Random.Range(0, instance.pathNameScript.nameList.Count)];
    }

    public static string GetRandomVisitorName()
    {
        return instance.nameScript.nameList[Random.Range(0, instance.nameScript.nameList.Count)];
    }
}
