using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    
    public ItemData itemData;
    public Action addItem;

    public Transform dropItem;

    public int score = 0;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UICondition.instance.UpdateScoreUI(score);
        
    }
}
