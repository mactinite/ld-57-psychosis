using System;
using UnityEngine;

namespace Battle_System
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float seconds;
        private void Start()
        {
            Destroy(gameObject, seconds);
        }
    }
}