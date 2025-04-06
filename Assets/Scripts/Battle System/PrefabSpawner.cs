using UnityEngine;

namespace Battle_System
{
    public class PrefabSpawner : MonoBehaviour
    {
        public GameObject prefab;

        public void InstantiatePrefabAtPosition(Vector3 position)
        {
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab, position, Quaternion.identity);
                // Optionally, you can set the parent of the instantiated object
                // instance.transform.SetParent(transform);
            }
            else
            {
                Debug.LogError("Prefab is not assigned.");
            }

        } 
        public void InstantiatePrefab()
        {
            InstantiatePrefabAtPosition(transform.position);
        }
    }
}