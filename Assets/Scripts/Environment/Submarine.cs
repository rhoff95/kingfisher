using System;
using UnityEngine;

namespace Environment
{
    public class Submarine : MonoBehaviour
    {
        public float descentSpeed;
        public float minHeight = -2f;
        public ParticleSystem pfx;
        
        private bool _descending;
        
        public void StartDecent()
        {
            _descending = true;
            pfx.Play();
        }

        private void Update()
        {
            if (_descending)
            {
                transform.position += Vector3.down * (descentSpeed * Time.deltaTime);
            }

            if (transform.position.y < minHeight)
            {
                Destroy(gameObject);
            }
        }
    }
}