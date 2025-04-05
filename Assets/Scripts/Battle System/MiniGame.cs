using UnityEngine;

namespace Battle_System
{
    public enum MiniGameStatus
    {
        Completed,
        Failed
    }
    public class MiniGame : MonoBehaviour
    {
        public virtual void StartMiniGame(System.Action<MiniGameStatus> onComplete)
        {
            // Implement the logic for starting the mini-game here
            // Call onComplete with the appropriate MiniGameStatus when done
        }
    }
}