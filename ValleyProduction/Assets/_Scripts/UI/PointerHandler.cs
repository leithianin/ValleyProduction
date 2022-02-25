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

    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke(this);

        var message = $"<b>{ValleyText.Title}</b>\n{ValleyText.Description}";
        //Trasmettre au tooltip
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke(this);
    }
}
