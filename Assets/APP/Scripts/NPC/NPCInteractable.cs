using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem; // ADDED: Required for the new Input System

[RequireComponent(typeof(Collider2D))]
public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private float pauseDuration = 3f; 
    [SerializeField] private TMP_Text dialogueText; 
    
    private NPCMovement movementScript;
    private NPCScheduleController scheduleController;
    private Collider2D npcCollider; 

    private void Awake()
    {
        movementScript = GetComponentInParent<NPCMovement>();
        scheduleController = GetComponentInParent<NPCScheduleController>();
        
        npcCollider = GetComponent<Collider2D>(); 

        if (dialogueText != null) dialogueText.text = ""; 
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            if (npcCollider.OverlapPoint(mouseWorldPosition))
            {
                TriggerInteraction();
            }
        }
    }

    private void TriggerInteraction()
    {
        if (movementScript != null && scheduleController != null)
        {
            movementScript.PauseMovement(pauseDuration);
            
            string npcAction = scheduleController.GetCurrentActionContext();
            
            StopAllCoroutines();
            StartCoroutine(ShowDialogue(npcAction));
        }
    }

    private IEnumerator ShowDialogue(string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text = text; 
            
            yield return new WaitForSeconds(pauseDuration); 
            
            dialogueText.text = ""; 
        }
    }
}