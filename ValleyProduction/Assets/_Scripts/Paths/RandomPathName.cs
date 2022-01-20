using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathName
{
    public static List<string> nameList = new List<string> { "Boulevard de Cailloux", "Route des Pigeons", "Route des des �rables", "Chemin des Baies", "Pierre qui roule", "Chemin des �toiles", 
                                                        "Les Champs aux Aigles Australs", "Le Territoire aux Ours Gris","Les Territoires aux Scarab�es Pacifiques","Le P�turage Fleuri","Le  Lagon P�tillant",
                                                        "Lac des Carcavers","Lac Vitreux","Creekplains Home","Test de nom super long pour voir jusqu'ou peut aller le nom","&�((��@|(","The Sweet Spot"};

    public static string GetRandomName()
    {
        return nameList[Random.Range(0, nameList.Count)];
    }
}
