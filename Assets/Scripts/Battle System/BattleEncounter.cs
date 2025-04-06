using System;
using System.Collections.Generic;
using Battle_System;
using UnityEngine;
using Yarn.Unity;

namespace DefaultNamespace.Battle_System
{
    [CreateAssetMenu(fileName = "Battle Encounter", menuName = "Battle System/Battle Encounter", order = 0)]
    public class BattleEncounter : ScriptableObject
    {
        public Sprite enemySpriteIdle;
        public MiniGameData miniGame;
        public string failedDialogueNode;
        public string successDialogueNode;
        public int enemyMaxHealth = 4;
        public string victoryDialogueNode;
        public string lossDialogueNode;
        public List<ItemReaction> itemReactions = new List<ItemReaction>();
        public string unkownItemReactionDialogueNode;


        public ItemReaction? GetItemReactionByItemName(string ItemName)
        {
            foreach (var itemReaction in itemReactions)
            {
                if (itemReaction.itemName == ItemName)
                {
                    return itemReaction;
                }
            }

            return null;
        }
    }
    
    [Serializable]
    public struct ItemReaction
    {
        public string itemName;
        public string reactionDialogueNode;
    }
}