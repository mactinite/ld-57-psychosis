using UnityEngine;

public class PipCounterUI : MonoBehaviour
{
    
    public GameObject healthCounterPrefab;
    public int maxHealth = 4;
    public int currentHealth = 4;
    
    private GameObject[] healthCounters;
    private void Start()
    {

    }
    
    public void UpdateHealthUI()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                healthCounters[i].transform.Find("Fill").gameObject.SetActive(true);
            }
            else
            {
                healthCounters[i].transform.Find("Fill").gameObject.SetActive(false);
            }
        }
    }
    
    public void SetHealth(int health, int maxHealth)
    {
        currentHealth = health;
        this.maxHealth = maxHealth;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        ;
        healthCounters = new GameObject[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            healthCounters[i] = Instantiate(healthCounterPrefab, transform);
        }
        UpdateHealthUI();
    }
}