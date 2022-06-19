using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<PointerHandler> onEnter;
    public event Action<PointerHandler> onExit;

    public TextTooltip ValleyText => relay.Value;
    [SerializeField] private TextHolder relay;

    [SerializeField] private RectTransform tooltipPos;
    [SerializeField] private int index;

    private bool isEnter = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TimerManager.CreateGameTimer(0.4f, SetupTooltip);
        isEnter = true;
    }

    public void SetupTooltip()
    {
        if (isEnter)
        {
            Debug.Log("Enter Tooltip");
            onEnter?.Invoke(this);
            var message = "";

            switch (UIManager.GetData.lang)
            {
                case Language.en:
                    if (ValleyText.Titleen != string.Empty) { message = $"<b>{ValleyText.Titleen}</b>\n{ValleyText.Texten}"; }
                    else { message = $"{ValleyText.Texten}"; }
                    break;
                case Language.fr:
                    if (ValleyText.Titlefr != string.Empty) { message = $"<b>{ValleyText.Titlefr}</b>\n{ValleyText.Textfr}"; }
                    else { message = $"{ValleyText.Textfr}"; }
                    break;
            }

            UIManager.GetTooltip.ShowTooltip(message, tooltipPos, index);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        onExit?.Invoke(this);
        UIManager.GetTooltip.HideTooltip();
    }

    private void OnDisable()
    {
        onExit?.Invoke(this);
        UIManager.GetTooltip.HideTooltip();
    }
}
