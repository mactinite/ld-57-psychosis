using DefaultNamespace;
using Framework.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : SingletonBehaviour<InteractionManager>
{
    private Transform cameraTransform;
    private Interactable currentInteractable;
    public TMP_Text interactionPrompt;
    public PlayerInput playerInput;

    private bool active = true;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        playerInput = GameManager.GetPlayerInput();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found.");
            return;
        }
        
        playerInput.actions["Interact"].performed += ctx => Interact();
    }

    // Update is called once per frame
    void Update()
    {
        if(!active) return;
        // sphere cast and if hit, check if the object has an Interactable component
        RaycastHit hit;
        if (Physics.SphereCast(cameraTransform.position, .1f, cameraTransform.forward, out hit, 2f))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            currentInteractable = interactable;
        }
        else
        {
            currentInteractable = null;
        }

        if (currentInteractable != null)
        {
            // show interaction prompt
            interactionPrompt.gameObject.SetActive(true);
            interactionPrompt.text = "F - " + currentInteractable.Verb();
        }
        else
        {
            interactionPrompt.gameObject.SetActive(false);
            interactionPrompt.text = "";
        }
    }
    
    public static void DeactivateInteractions()
    {
        // deactivate the interaction prompt
        Instance.interactionPrompt.gameObject.SetActive(false);
        Instance.interactionPrompt.text = "";
        Instance.active = false;
    }

    public static void ActivateInteractions()
    {
        Instance.active = true;

    }


    private void Interact()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            currentInteractable = null; // reset current interactable after interaction
        }
    }
}