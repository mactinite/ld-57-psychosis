using System.Collections;
using UnityEngine;

namespace Battle_System
{
    public class MiniGame_Minions : MiniGame
    {
        public ClickableSprite minionPrefab;
        public GameObject darknessOverlay;
        public int minionCount = 3;
        public float minionLifetime = 5f;
        [SerializeField] private float maxSpawnDistance = 2f;

        public override void StartMiniGame(System.Action<MiniGameStatus> onComplete)
        {
            // Implement the logic for starting the target practice mini-game here
            // Call onComplete with the appropriate MiniGameStatus when done
            Debug.Log("Starting Minions Mini Game");
            StartCoroutine(Run(onComplete));
        }


        public IEnumerator Run(System.Action<MiniGameStatus> onComplete)
        {
            CursorManager.SetCursor(CursorManager.CursorType.Point);
            yield return new WaitForSeconds(1f);
            // Lights out!
            darknessOverlay.SetActive(true);

            int targetsHit = 0;
            // spawn all minions at random positions
            for (int i = 0; i < minionCount; i++)
            {
                var minion = Instantiate(minionPrefab, transform);
                minion.transform.localPosition = GetRandomPosition();
                minion.OnClick.AddListener(() =>
                {
                    OnMinionClicked(minion);
                    targetsHit++;
                });
                
                minion.SetLifetime(3f+minionLifetime);
                yield return null;
            }
            yield return new WaitForSeconds(3f);

            //lights on!
            darknessOverlay.SetActive(false);
            CursorManager.SetCursor(CursorManager.CursorType.Targeting);
            yield return new WaitForSeconds(minionLifetime);
            bool pass = targetsHit >= minionCount;
            onComplete?.Invoke(pass ? MiniGameStatus.Completed : MiniGameStatus.Failed);
            Destroy(gameObject);
        }

        private void OnMinionClicked(ClickableSprite minion)
        {
            // Handle the minion being clicked
            Destroy(minion.gameObject);
        }

        private Vector3 GetRandomPosition()
        {
            // Generate a random position within a certain range
            float x = Random.Range(-maxSpawnDistance, maxSpawnDistance);
            float y = Random.Range(-maxSpawnDistance, maxSpawnDistance);
            return new Vector3(x, -.5f, y);
        }

        private void OnTargetClicked(ClickableSprite target)
        {
            // Handle the target being clicked
            Destroy(target.gameObject);
        }

        private void OnBombClicked(ClickableSprite target)
        {
            // Handle the target being clicked
            Destroy(target.gameObject);
        }
    }
}