using UnityEngine;

namespace Battle_System
{
    [CreateAssetMenu(fileName = "Mini Game", menuName = "Battle System/Mini Game", order = 0)]
    public class MiniGameData : ScriptableObject
    {
        public MiniGame miniGamePrefab;
    }
}