using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory_System
{
    public class ItemSelectable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemAsset itemAsset;
        public Image itemImage;
        public Color32 hoverColor = new Color32(255, 255, 0, 255);
        public Color32 defaultColor = new Color32(255, 255, 255, 255);
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Item clicked: " + itemAsset.name);
            InventoryManager.ItemClicked?.Invoke(itemAsset.name);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            itemImage.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemImage.color = defaultColor;
        }
    }
}