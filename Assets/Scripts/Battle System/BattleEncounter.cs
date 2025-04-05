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
    }
}