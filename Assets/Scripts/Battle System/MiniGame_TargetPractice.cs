using System.Collections;
using UnityEngine;

namespace Battle_System
{
    public class MiniGameTargetPractice : MiniGame
    {
        public ClickableSprite targetPrefab;
        public int targetCount = 5;
        public float targetSpawnInterval = 1f;
        public float targetLifetime = 1f;
        public float maxSpawnDistance = .2f;
        public override void StartMiniGame(System.Action<MiniGameStatus> onComplete)
        {
            // Implement the logic for starting the target practice mini-game here
            // Call onComplete with the appropriate MiniGameStatus when done
            Debug.Log("Starting Target Practice Mini Game");
            StartCoroutine(Run(onComplete));
        }
        
        public IEnumerator Run(System.Action<MiniGameStatus> onComplete)
        {
            CursorManager.SetCursor(CursorManager.CursorType.Point);
            yield return new WaitForSeconds(2f);
            FullScreenText.ShowText("HIT THE TARGETS");
            yield return new WaitForSeconds(2f);
            FullScreenText.HideText();

            CursorManager.SetCursor(CursorManager.CursorType.Targeting);
            int targetsHit = 0;
            int lastTargetsHit = 0;
            CursorManager.SetCursor(CursorManager.CursorType.Targeting);
            yield return new WaitForSeconds(2f);
            
            for (int i = 0; i < targetCount; i++)
            {
                
                var target = Instantiate(targetPrefab, transform);
                target.transform.localPosition = GetRandomPosition();
                lastTargetsHit = targetsHit;
                target.OnClick.AddListener(() =>
                {
                    OnTargetClicked(target);
                    targetsHit++;
                });
                
                yield return new WaitForSeconds(targetLifetime);
                if (!(target == null) && lastTargetsHit == targetsHit)
                {
                    Destroy(target.gameObject);
                }

                yield return new WaitForSeconds(Random.Range(0.1f, targetSpawnInterval));
            }
            CursorManager.SetCursor(CursorManager.CursorType.Point);

            onComplete?.Invoke(targetsHit >= targetCount ? MiniGameStatus.Completed : MiniGameStatus.Failed);
            Destroy(gameObject);
        }
        
        private void OnTargetClicked(ClickableSprite target)
        {
            // Handle the target being clicked
            Destroy(target.gameObject);
        }

        private Vector3 GetRandomPosition()
        {
            // Generate a random position within the bounds of the game area
            float x = Random.Range(-maxSpawnDistance, maxSpawnDistance);
            float y = Random.Range(-maxSpawnDistance, maxSpawnDistance);
            return new Vector3(x, y, 0);
        }
    }
}