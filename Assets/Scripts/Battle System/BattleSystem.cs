using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Battle_System;
using Framework.Singleton;
using Inventory_System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private PipCounterUI enemyHealthCounter;
        [SerializeField] private PipCounterUI playerHealthCounter;

        [SerializeField] private GameObject inventoryUI;

        private int enemyHealth = 4;
        private int playerHealth = 4; // Todo: load from gameManager and persist

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
            InventoryManager.ItemClicked += Instance.UseItem;

            Instance.currentEncounter = encounter;
            Instance.enemyHealth = encounter.enemyMaxHealth;
            Instance.enemyHealthCounter.SetHealth(Instance.enemyHealth, encounter.enemyMaxHealth);

            Instance.playerHealth = GameManager.Instance.playerHealth;
            Instance.playerHealthCounter.SetHealth(GameManager.instance.playerHealth,
                GameManager.instance.playerMaxHealth);

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
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.RemoveListener(OnBattleEnd);
            InventoryManager.ItemClicked -= UseItem;
            Instance.currentEncounter = null;
            Instance.battleCanvas.gameObject.SetActive(false);
            Instance.battleStage.SetActive(false);

            GameManager.UnfreezePlayer();
            InteractionManager.ActivateInteractions();

            Instance.battleCinemachineCamera.gameObject.SetActive(false);
            Instance.minigameCinemachineCamera.gameObject.SetActive(false);
            Instance.battleCamera.gameObject.SetActive(false);

            Instance.isRunning = false;
            
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
                GameManager.Instance.playerHealth -= 1;
                Instance.playerHealth = GameManager.Instance.playerHealth;
                Instance.playerHealthCounter.SetHealth(Instance.playerHealth, GameManager.instance.playerMaxHealth);
            }
            else if (status == MiniGameStatus.Completed)
            {
                Instance.enemyHealth -= 1;
                Instance.enemyHealthCounter.SetHealth(Instance.enemyHealth, currentEncounter.enemyMaxHealth);
            }

            if (Instance.enemyHealth <= 0)
            {
                OnEnemyDefeated();
                return;
            }

            if (Instance.playerHealth <= 0)
            {
                OnPlayerDefeated();
                return;
            }

            DialogueManager.Instance.dialogueRunner.StartDialogue(status == MiniGameStatus.Completed
                ? currentEncounter.successDialogueNode
                : currentEncounter.failedDialogueNode);
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.AddListener(PostMiniGameDialogueComplete);
        }

        private void OnPlayerDefeated()
        {
            DialogueManager.Instance.dialogueRunner.StartDialogue(currentEncounter.lossDialogueNode);
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.AddListener(OnBattleEnd);
        }

        public void OnEnemyDefeated()
        {
            DialogueManager.Instance.dialogueRunner.StartDialogue(currentEncounter.victoryDialogueNode);
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.AddListener(OnBattleEnd);
        }

        private void PostMiniGameDialogueComplete()
        {
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.RemoveListener(PostMiniGameDialogueComplete);
            Instance.battleCanvas.gameObject.SetActive(true);
            Instance.minigameCinemachineCamera.gameObject.SetActive(false);
        }


        public void OpenItems()
        {
            InventoryManager.Instance.ToggleInventory();
        }

        public void UseItem(string usedItemName)
        {
            // Try to use the item,
            // if the encounter has a reaction for the item, play it. 
            // If the item is used, remove it from the inventory
            Instance.battleCanvas.gameObject.SetActive(false);
            var item = InventoryManager.Instance.GetItemByName(usedItemName);
            if (item != null)
            {
                ItemReaction? itemReaction =
                    currentEncounter.GetItemReactionByItemName(usedItemName);

                if (itemReaction?.itemName == usedItemName)
                {
                    DialogueManager.Instance.dialogueRunner.StartDialogue(itemReaction?.reactionDialogueNode);
                    DialogueManager.Instance.dialogueRunner.onDialogueComplete.AddListener(AfterItemUsed);
                    return;
                }
            }

            DialogueManager.Instance.dialogueRunner.StartDialogue(currentEncounter.unkownItemReactionDialogueNode);
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.AddListener(AfterItemUsed);
        }

        public void AfterItemUsed()
        {
            // Remove the item from the inventory
            InventoryManager.HideInventory();
            DialogueManager.Instance.dialogueRunner.onDialogueComplete.RemoveListener(AfterItemUsed);
            Instance.battleCanvas.gameObject.SetActive(true);
        }
    }
}