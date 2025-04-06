using System;
using System.Collections.Generic;
using Battle_System;
using Framework.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory_System
{
    public class InventoryManager : SingletonBehaviour<InventoryManager>
    {
        public GameObject inventoryUI;
        public GameObject inventoryItemPrefab;
        private List<ItemAsset> _inventory = new List<ItemAsset>();
        [SerializeField]
        private List<ItemAsset> allItems = new List<ItemAsset>();
        public static Action<String> ItemClicked;
        
        private void Start()
        {
            // Hook up player input to show/hide inventory
            HideInventory();
            GameManager.GetPlayerInput().actions["Inventory"].performed += OnInventoryInput;
        }
        private void OnInventoryInput(InputAction.CallbackContext ctx)
        {
            ToggleInventory();
        }

        public void ToggleInventory()
        {
            bool unfreeze = !DialogueManager.Instance.dialogueRunner.IsDialogueRunning &&
                            !BattleSystem.Instance.isRunning;
            
            if (inventoryUI.activeSelf)
            {
                HideInventory();
                if(unfreeze)
                    GameManager.UnfreezePlayer();
            }
            else
            {
                ShowInventory();
                if(unfreeze)
                    GameManager.FreezePlayer();
            }
        }
        
        

        public static void AddItem(ItemAsset item)
        {
            if (Instance._inventory.Contains(item))
            {
                Debug.Log($"Item {item.itemName} already exists in inventory.");
                return;
            }
            
            Instance._inventory.Add(item);
            // Update Inventory UI
            var itemUI = Instantiate(Instance.inventoryItemPrefab, Instance.inventoryUI.transform);
            itemUI.GetComponent<ItemSelectable>().itemAsset = item;
            itemUI.GetComponentInChildren<Image>().sprite = item.itemSprite;
            itemUI.GetComponentInChildren<TMP_Text>().text = item.itemName;
            Debug.Log($"Item {item.itemName} added to inventory.");
        }
        
        public static void RemoveItem(ItemAsset item)
        {
            if (Instance._inventory.Contains(item))
            {
                Instance._inventory.Remove(item);
                // Update Inventory UI
                foreach (Transform child in Instance.inventoryUI.transform)
                {
                    if (child.GetComponentInChildren<TMP_Text>().text == item.itemName)
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
                
                Debug.Log($"Item {item.itemName} removed from inventory.");
            }
            else
            {
                Debug.Log($"Item {item.itemName} not found in inventory.");
            }
        }
        
        public static void ShowInventory()
        {
            if (Instance.inventoryUI != null)
            {
                Instance.inventoryUI.SetActive(true);
                Debug.Log("Inventory UI shown.");
            }
            else
            {
                Debug.LogError("Inventory UI is not assigned.");
            }
        }
        
        public static void HideInventory()
        {
            if (Instance.inventoryUI != null)
            {
                Instance.inventoryUI.SetActive(false);
                Debug.Log("Inventory UI hidden.");
            }
            else
            {
                Debug.LogError("Inventory UI is not assigned.");
            }
        }
        
        public List<ItemAsset> GetInventory()
        {
            return _inventory;
        }


        public ItemAsset GetItemByName(string s)
        {
            foreach (var item in allItems)
            {
                if (item.name == s)
                {
                    return item;
                }
            }

            Debug.LogError($"Item {s} not found in all items.");
            return null;
        }
    }
}