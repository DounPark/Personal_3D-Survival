using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public static UICondition instance;
    public Condition health;
    public Condition stamina;
    
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if(instance==null)
            instance=this;
    }

    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"획득 코인 : {score}";
    }
}
