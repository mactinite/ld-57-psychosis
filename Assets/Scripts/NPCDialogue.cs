using DefaultNamespace;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, Interactable
{
    [SerializeField]
    private string startNode = "Start";
    private string interactionVerb = "Talk";
    public void Interact()
    {
        DialogueManager.StartDialogue(startNode, () => GameManager.StopPlayerLookAt());
        GameManager.PlayerLookAt(transform);
    }

    public string Verb()
    {
        return interactionVerb;
    }
}
