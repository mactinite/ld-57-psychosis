using System;
using Framework.Singleton;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueManager : SingletonBehaviour<DialogueManager>
{
    public DialogueRunner dialogueRunner;
    private PlayerInput playerInput;

    public static void StartDialogue(string startNode)
    {
        Instance.dialogueRunner.StartDialogue(startNode);
    }

    public static void StartDialogue(string startNode, Action onDialogueComplete)
    {
        Instance.dialogueRunner.onDialogueComplete.AddListener(() => onDialogueComplete?.Invoke());
        Instance.dialogueRunner.StartDialogue(startNode);
    }

    public static void StopDialogue()
    {
        Instance.dialogueRunner.Stop();
    }

    public void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        playerInput = GameManager.GetPlayerInput();
        if (dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner component not found on this GameObject.");
            return;
        }

        dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }


    private void OnDialogueStart()
    {
        // disable player input
        var inputs = GameManager.GetInputs();
        if (inputs != null)
        {
            inputs.allowMovement = false;
            inputs.SetCursorState(false);
        }

        InteractionManager.DeactivateInteractions();
    }

    private void OnDialogueComplete()
    {
        // re-enable player input
        var inputs = GameManager.GetInputs();
        if (inputs != null)
        {
            inputs.allowMovement = true;
            inputs.SetCursorState(true);
        }

        InteractionManager.ActivateInteractions();
    }
}