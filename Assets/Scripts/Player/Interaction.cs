using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public Transform rayOrigin;
    public float checkRate = 0.05f;
    private float laskCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    
    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;    
    }
    
    void Update()
    {
        if (Time.time - laskCheckTime > checkRate)
        {
            laskCheckTime = Time.time;
            
            Ray ray = new Ray(origin: rayOrigin.position, direction:rayOrigin.forward);
            
            Debug.DrawRay(ray.origin, ray.direction*maxCheckDistance, Color.red, 1f);
            
            RaycastHit hit;
    
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    
    private void SetPromptText()
    {
        if (curInteractable != null)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = curInteractable.GetInteractPrompt();    
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
        
    }
    
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
