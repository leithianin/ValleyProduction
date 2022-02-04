using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : VLY_Singleton<GeneratorManager>
{
    public static List<string> namePathList = new List<string> { "Boulevard de Cailloux", "Route des Pigeons", "Route des des Érables", "Chemin des Baies", "Pierre qui roule", "Chemin des Étoiles",
                                                        "Les Champs aux Aigles Australs", "Le Territoire aux Ours Gris","Les Territoires aux Scarabées Pacifiques","Le Pâturage Fleuri","Le  Lagon Pétillant",
                                                        "Lac des Carcavers","Lac Vitreux","Creekplains Home","Test de nom super long pour voir jusqu'ou peut aller le nom","&é((àç@|(","The Sweet Spot"};

    public static List<string> nameVisitorsList = new List<string> { "Corto Pavie", "Ken Chi", "The running man", "Jodie Blease", "Lawrence Gordon", "Amos O'Sullivan", "Arielle Kinney",
                                                            "Mahima Cruz","Diesel Chavez","Subhaan Begum","Lennie Browning","Zohaib Gibbs","Ellie-Mae Duke","Jaime Ratliff",
                                                            "Sahib Bateman","Katy Cowan","Reiss Kinney","Christopher Short","Saffa Wharton","Lauren Byrne","Ayla Gibson",
                                                            "Janet Rhodes"};

    public static string GetRandomPathName()
    {
        return namePathList[Random.Range(0, namePathList.Count)];
    }

    public static string GetRandomVisitorName()
    {
        return nameVisitorsList[Random.Range(0, nameVisitorsList.Count)];
    }
}
