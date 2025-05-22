using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    
    Condition health {get {return uiCondition.health;}}
    Condition stamina {get {return uiCondition.stamina;}}
    
    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Drink(float amount)
    {
        stamina.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }
        
        stamina.Subtract(amount);
        return true;
    }
}
