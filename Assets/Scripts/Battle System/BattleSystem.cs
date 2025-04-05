using System.Collections.Generic;
using DefaultNamespace.Battle_System;
using Framework.Singleton;
using Unity.Cinemachine;
using UnityEngine;

namespace Battle_System
{
    public class BattleSystem : SingletonBehaviour<BattleSystem>
    {
        [SerializeField] private Canvas battleCanvas;
        [SerializeField] private GameObject battleStage;
        [SerializeField] private SpriteRenderer enemySpriteRenderer;
        [SerializeField] private Camera battleCamera;
        [SerializeField] private CinemachineCamera battleCinemachineCamera;
        [SerializeField] private CinemachineCamera minigameCinemachineCamera;
    
        [SerializeField] private List<BattleEncounter> battleEncounters = new List<BattleEncounter>();
        [SerializeField] private Transform miniGameParent;

        private BattleEncounter currentEncounter;
        private DialogueManager _dialogueManager;
    
        public bool isRunning;
        private void Start()
        {
            _dialogueManager = DialogueManager.instance;
            _dialogueManager.dialogueRunner.AddCommandHandler<string>("StartBattle", StartBattleCommand);
        }
    
        private static void StartBattleCommand(string encounterName)
        {
            var encounter = Instance.GetBattleEncounter(encounterName);

            if (encounter != null)
            {
                StartBattle(encounter);
            }
            else
            {
                Debug.LogError($"Encounter {encounterName} not found.");
            }
        }

        private BattleEncounter GetBattleEncounter(string name)
        {
            foreach (var encounter in battleEncounters)
            {
                if (encounter.name == name)
                {
                    return encounter;
                }
            }

            Debug.LogError($"Battle Encounter with name {name} not found.");
            return null;
        }

        public static void StartBattle(BattleEncounter encounter)
        {
            Instance.currentEncounter = encounter;
            
            GameManager.FreezePlayer();
            InteractionManager.DeactivateInteractions();
        
            Instance.battleCinemachineCamera.gameObject.SetActive(true);
            Instance.minigameCinemachineCamera.gameObject.SetActive(false);
            Instance.battleCamera.gameObject.SetActive(true);

            Instance.isRunning = true;
            Instance.battleCanvas.gameObject.SetActive(true);
            Instance.battleStage.SetActive(true);
            Instance.enemySpriteRenderer.sprite = encounter.enemySpriteIdle;
        }

        public void OnBattleEnd()
        {
            Instance.currentEncounter = null;
            
            GameManager.UnfreezePlayer();
            InteractionManager.ActivateInteractions();
        
            Instance.battleCinemachineCamera.gameObject.SetActive(false);
            Instance.minigameCinemachineCamera.gameObject.SetActive(false);
            Instance.battleCamera.gameObject.SetActive(false);
        
            Instance.isRunning = false;
            Instance.battleCanvas.gameObject.SetActive(false);
            Instance.battleStage.SetActive(false);
        }
    
        public void OnFight()
        {
            // Run A Combat Game
            Instance.minigameCinemachineCamera.gameObject.SetActive(true);
            Instance.battleCanvas.gameObject.SetActive(false);

            // Insantiate the minigame from the encounter and wait for the onComplete callback
            var miniGame = Instantiate(currentEncounter.miniGame.miniGamePrefab, miniGameParent);
            miniGame.StartMiniGame(OnEndMinigame);
        }

        public void OnEndMinigame(MiniGameStatus status)
        {
            if (status == MiniGameStatus.Failed)
            {
                DialogueManager.Instance.dialogueRunner.StartDialogue(currentEncounter.failedDialogueNode);
            }
            else
            {
                DialogueManager.Instance.dialogueRunner.StartDialogue(currentEncounter.successDialogueNode);
            }
            
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.AddListener(PostGameDialogueComplete);
        }
        
        private void PostGameDialogueComplete()
        {
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.RemoveListener(PostGameDialogueComplete);
            Instance.battleCanvas.gameObject.SetActive(true);
            Instance.minigameCinemachineCamera.gameObject.SetActive(false);
        }

        public void OpenItems()
        {
            // Open items menu
        }

        public void UseItem(string name)
        {
            // use a specific item with name
        }
    
    }
}
