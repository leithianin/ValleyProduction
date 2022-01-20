using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathName
{
    public static List<string> nameList = new List<string> { "Boulevard de Cailloux", "Route des Pigeons", "Route des des Érables", "Chemin des Baies", "Pierre qui roule", "Chemin des Étoiles", 
                                                        "Les Champs aux Aigles Australs", "Le Territoire aux Ours Gris","Les Territoires aux Scarabées Pacifiques","Le Pâturage Fleuri","Le  Lagon Pétillant",
                                                        "Lac des Carcavers","Lac Vitreux","Creekplains Home","Test de nom super long pour voir jusqu'ou peut aller le nom","&é((àç@|(","The Sweet Spot"};

    public static string GetRandomName()
    {
        return nameList[Random.Range(0, nameList.Count)];
    }
}
