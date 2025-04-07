using System;
using Battle_System;
using Framework.Singleton;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueManager : SingletonBehaviour<DialogueManager>
{
    public DialogueRunner dialogueRunner;
    private PlayerInput playerInput;
    
    private void OnContinueInput(InputAction.CallbackContext ctx)
    {
        Instance.dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
    }

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
        GameManager.GetPlayerInput().actions["Continue"].performed +=  OnContinueInput;

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
        GameManager.FreezePlayer();
    }

    private void OnDialogueComplete()
    {
        if (BattleSystem.instance.isRunning) return;
        GameManager.UnfreezePlayer();
        InteractionManager.ActivateInteractions();
    }
}