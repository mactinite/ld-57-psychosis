using UnityEngine;

namespace Inventory_System
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory System/Item", order = 0)]
    public class ItemAsset: ScriptableObject
    {
        public Sprite itemSprite;
        public string itemName;
        public string itemDescription;
    }
    
}