using System;
using UnityEngine;

public class BounceAroundScreen : MonoBehaviour
{
    [SerializeField] Vector3 screenBounds = Vector2.one;
    public float speed = 5f;
    private Vector3 direction;

    private void Start()
    {
        direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
            0f).normalized;
    }

    private void Update()
    {
        // move in random direction, when you cross a boundary, reflect the direction
        Vector3 position = transform.localPosition;

        position += direction * Time.deltaTime * speed;
        if (position.x > screenBounds.x || position.x < -screenBounds.x)
        {
            direction = new Vector3(-direction.x, direction.y, 0f);
        }
        else if (position.y > screenBounds.y || position.y < -screenBounds.y)
        {
            direction = new Vector3(direction.x, -direction.y, 0f);
        }

        transform.localPosition = position;
    }
}