using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<PointerHandler> onEnter;
    public event Action<PointerHandler> onExit;

    public TextBase ValleyText => relay.Value;
    [SerializeField] private TextHolder relay;

    [SerializeField] private RectTransform tooltipPos;
    [SerializeField] private int index;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke(this);
        var message = "";
        if (ValleyText.Title != "") { message = $"<b>{ValleyText.Title}</b>\n{ValleyText.Description}"; }
        else                                  { message = $"{ValleyText.Description}"; }
        UIManager.GetTooltip.ShowTooltip(message, tooltipPos, index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke(this);
        UIManager.GetTooltip.HideTooltip();
    }
}
