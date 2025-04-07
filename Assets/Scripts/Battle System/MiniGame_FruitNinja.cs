using System.Collections;
using UnityEngine;

namespace Battle_System
{
    public class MiniGame_FruitNinja : MiniGame
    {
        public ClickableSprite targetPrefab;
        public ClickableSprite bombPrefab;
        public int targetCount = 5;
        public float targetSpawnInterval = 1f;
        public float targetLifetime = 2f;
        
        public float launchForceMin = 0.5f;
        public float launchForceMax = 1f;
        public float bombSpawnProbability = 0.2f;
        [SerializeField] private float deviationAngle = 20f;
        [SerializeField] private float verticalOffset = -1f;
        [SerializeField] private float maxSpawnDistance = 1f;

        public override void StartMiniGame(System.Action<MiniGameStatus> onComplete)
        {
            // Implement the logic for starting the target practice mini-game here
            // Call onComplete with the appropriate MiniGameStatus when done
            Debug.Log("Starting Fruit Ninja Mini Game");
            StartCoroutine(Run(onComplete));
        }
        

        public IEnumerator Run(System.Action<MiniGameStatus> onComplete)
        {
            CursorManager.SetCursor(CursorManager.CursorType.Point);
            yield return new WaitForSeconds(2f);
            FullScreenText.ShowText("HIT TARGETS AVOID BOMBS");
            yield return new WaitForSeconds(2f);
            CursorManager.SetCursor(CursorManager.CursorType.Targeting);
            FullScreenText.HideText();
            
            int targetsHit = 0;
            int bombsHit = 0;
            int targetsSpawned = 0;
            // Instantiate targets from transform position
            Vector3 targetSpawnPosition = transform.position + Vector3.up * verticalOffset +
                                          Vector3.right * Random.Range(-maxSpawnDistance, maxSpawnDistance);
            for (int i = 0; i < targetCount; i++)
            {
                bool isBomb = Random.value < bombSpawnProbability;
                ClickableSprite target;
                if (isBomb)
                {
                    target = Instantiate(bombPrefab, targetSpawnPosition, Quaternion.identity);
                    target.OnClick.AddListener(() =>
                    {
                        OnTargetClicked(target);
                        bombsHit++;
                    });
                    target.SetLifetime(targetLifetime);
                }
                else
                {
                    targetsSpawned++;
                    target = Instantiate(targetPrefab, targetSpawnPosition, Quaternion.identity);
                    target.OnClick.AddListener(() =>
                    {
                        OnTargetClicked(target);
                        targetsHit++;
                    });
                    target.SetLifetime(targetLifetime);
                }

                
                Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
                // launch upward and randomize the angle
                float launchAngle = Random.Range(-deviationAngle, deviationAngle);
                Vector2 launchDirection = Quaternion.Euler(0, 0, launchAngle) * Vector2.up;
                float launchForce = Random.Range(launchForceMin, launchForceMax);
                rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(targetSpawnInterval);
                if(bombsHit > 0)
                {
                    // If a bomb is hit, end the game immediately
                    onComplete?.Invoke(MiniGameStatus.Failed);
                    Destroy(gameObject);
                    yield break;
                }
            }
            
            yield return new WaitForSeconds(2f);

            if(targetsHit >= targetsSpawned)
            {
                onComplete?.Invoke(MiniGameStatus.Completed);
            }
            else
            {
                onComplete?.Invoke(MiniGameStatus.Failed);
            }
            
            Destroy(gameObject);

            
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