using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_System
{
    public class MiniGame_Boss : MiniGame
    {
        public ClickableSprite bossPrefab;
        public ClickableSprite bombPrefab;

        public Slider healthBar;
        public int bossMaxHealth = 100;
        [SerializeField] private float spawnOffset = 2f;
        [SerializeField] private float launchForceMin = 5f;
        [SerializeField] private float launchForceMax = 7f;

        [SerializeField] private float deviationAngle = 20f;
        [SerializeField] private float bombSpawnInterval = .6f;
        [SerializeField] private float timeToKillBoss = 5f;
        [SerializeField] private float bossDepthOffset = -1f;

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
            yield return new WaitForSeconds(2f);
            FullScreenText.ShowText("KILL THE BOSS");
            yield return new WaitForSeconds(2f);
            CursorManager.SetCursor(CursorManager.CursorType.Targeting);
            FullScreenText.HideText();
            yield return new WaitForSeconds(2f);
            int currentBossHealth = bossMaxHealth;
            int bombsHit = 0;
            // spawn boss
            var boss = Instantiate(bossPrefab, transform);
            boss.OnClick.AddListener(() =>
            {
                currentBossHealth--;
                healthBar.value = (float)currentBossHealth / bossMaxHealth;
            });
            
            boss.transform.localPosition = new Vector3(0f, 0f, bossDepthOffset);
            float bombTimer = 0f;
            float totalTimer = timeToKillBoss;
            while(currentBossHealth > 0)
            {
                bombTimer += Time.deltaTime;
                totalTimer -= Time.deltaTime;
                // spawn bombs
                if (bombTimer > bombSpawnInterval)
                {
                    var bomb = Instantiate(bombPrefab, transform);
                    Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
                    // launch upward and randomize the angle
                    float launchAngle = Random.Range(-deviationAngle, deviationAngle);
                    Vector2 launchDirection = Quaternion.Euler(0, 0, launchAngle) * Vector2.up;
                    float launchForce = Random.Range(launchForceMin, launchForceMax);
                    rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
                    bomb.transform.localPosition = GetRandomBombPosition();
                    bomb.OnClick.AddListener(() =>
                    {
                        OnBombClicked(bomb);
                        bombsHit++;
                    });
                    bombTimer = 0f;
                }

                // Failed if bomb is clicked
                if (bombsHit > 0)
                {
                    onComplete(MiniGameStatus.Failed);
                    Destroy(gameObject);
                    yield break;
                }
                
                if(totalTimer < 0f && currentBossHealth > 0)
                {
                    onComplete(MiniGameStatus.Failed);
                    Destroy(gameObject);
                    yield break;
                }
                
                // Wait for next frame
                yield return null;

            }
            
            onComplete(MiniGameStatus.Completed);
            Destroy(gameObject);
        }

        private Vector3 GetRandomBombPosition()
        {
            return new Vector3(Random.Range(-spawnOffset, spawnOffset), 0f, -1f);
        }

        private void OnBossClicked(ClickableSprite boss)
        {
            
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