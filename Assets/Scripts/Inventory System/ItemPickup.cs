using System;
using DefaultNamespace;
using UnityEngine;

namespace Inventory_System
{
    public class ItemPickup: MonoBehaviour, Interactable
    {
        public string interactionVerb = "Grab";
        public ItemAsset itemAsset;
        public SpriteRenderer spriteRenderer;
        public void Interact()
        {
            InventoryManager.AddItem(itemAsset);
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            spriteRenderer.sprite = itemAsset.itemSprite;
        }

        public string Verb() => interactionVerb;
    }
}