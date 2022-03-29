using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChoosePathButtons : MonoBehaviour
{
    private Camera camera;

    public List<ButtonPathData> pathButtonList = new List<ButtonPathData>();
    [SerializeField] private GameObject pathChoiceMenu;

    public void Start()
    {
        camera = PlayerInputManager.GetCamera;
    }

    //Position des boutons en jeu par rapport au Pathpoint selectionné
    public void ButtonsOffset(IST_PathPoint pathpoint)
    {
        Vector3 positionButtons = pathpoint.transform.position;

        float offsetPosY = positionButtons.y;
        float offsetPosX = positionButtons.x;

        Vector3 offsetPos = new Vector3(offsetPosX, offsetPosY, positionButtons.z);

        Vector2 canvasPos;
        Vector2 screenPoint = camera.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(pathButtonList[0].transform.parent.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        pathButtonList[0].transform.parent.transform.localPosition = canvasPos;
    }

    //Range les PathData dans la liste de boutons 
    public void ArrangePathButton(IST_PathPoint pathpoint)
    {
        pathChoiceMenu.SetActive(true);

        foreach (ButtonPathData go in pathButtonList)
        {
            go.gameObject.SetActive(false);
        }

        foreach (PathData pd in PathManager.GetAllPath)
        {
            if (pd.ContainsPoint(pathpoint))
            {
                for (int i = 0; i < pathButtonList.Count - 1; i++)
                {
                    if (!pathButtonList[i].gameObject.activeSelf)
                    {
                        pathButtonList[i].pathData = pd;
                        pathButtonList[i].buttonPathpoint = pathpoint;
                        pathButtonList[i].transform.GetChild(0).GetComponent<Text>().text = pd.name;
                        pathButtonList[i].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        ButtonsOffset(pathpoint);
    }

    public void HidePathButton()
    {
        pathChoiceMenu.SetActive(false);

        foreach (ButtonPathData go in pathButtonList)
        {
            go.gameObject.SetActive(false);
        }
    }
}
