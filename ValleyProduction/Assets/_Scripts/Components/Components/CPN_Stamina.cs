using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Stamina : VLY_Component<CPN_Data_Stamina>
{
    private Vector3 lastPosition;

    [SerializeField] private float currentStamina;
    [SerializeField] private float lastStaminaState;
    [SerializeField] private float maxStamina;

    private float slopeCoef = 0.1f;
    private float staminaRegenCoef = 5f;

    [SerializeField, Tooltip("Appelé quand le personnage arrive à 0 de stamina.")] private UnityEvent OnIsExhausted;
    [SerializeField, Tooltip("Appelé quand le personnage repasse au dessus de 0 de stamina.")] private UnityEvent OnIsNotExhausted;
    [SerializeField, Tooltip("Appelé à chaque fois que le personnage se déplace et qu'il n'a plus de stamina.")] private UnityEvent OnMoveWithoutStamina;

    private void Start()
    {
        lastPosition = transform.position;
        currentStamina = maxStamina;
        lastStaminaState = currentStamina;
    }

    private void Update()
    {
        if(Vector3.Distance(lastPosition, transform.position) > 0)
        {
            currentStamina -= Vector3.Distance(lastPosition, transform.position) * CalculateSlope();
            UIManager.UpdateCurrentStamina();

            if(currentStamina <= 0)
            {
                if(lastStaminaState > 0)
                {
                    OnIsExhausted?.Invoke();
                }
                else
                {
                    OnMoveWithoutStamina?.Invoke();
                }
                currentStamina = 0;
            }
        }
        else if(currentStamina < maxStamina)
        {
            if(lastStaminaState <= 0)
            {
                OnIsNotExhausted?.Invoke();
            }

            currentStamina += Time.deltaTime * staminaRegenCoef;

            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }

        lastStaminaState = currentStamina;
        lastPosition = transform.position;
    }

    private float CalculateSlope()
    {
        float toReturn = (transform.position.y - lastPosition.y) *slopeCoef + 1;
        if(toReturn > 1)
        {
            return toReturn;
        }
        return 1;
    }

    public override void SetData(CPN_Data_Stamina dataToSet)
    {
        maxStamina = dataToSet.MaxStamina();

        slopeCoef = dataToSet.SlopeCoef();

        staminaRegenCoef = dataToSet.RegenCoef();

        SetStaminaPercentage(1);
    }

    public float GetStamina => currentStamina;

    public void SetStaminaPercentage(float percent)
    {
        currentStamina = maxStamina * percent;
    }
}
