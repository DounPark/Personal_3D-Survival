using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName} \n {data.description}";
        return str;
    }

    public void OnInteract()
    {
        if (data.isInstantUse)
        {
            foreach (var effect in data.consumables)
            {
                switch (effect.type)
                {
                    case ConsumableType.Score:
                        CharacterManager.Instance.Player.AddScore((int)effect.value);
                        break;
                    case ConsumableType.Speed:
                        CharacterManager.Instance.Player.controller.Boost(effect.value, effect.duration);
                        break;
                }
            }
        }
        else
        {
            CharacterManager.Instance.Player.itemData = data;
            CharacterManager.Instance.Player.addItem?.Invoke();
        }
        
        Destroy(gameObject);
    }
}
