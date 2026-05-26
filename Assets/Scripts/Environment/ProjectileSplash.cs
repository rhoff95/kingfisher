using UnityEngine;

namespace Environment
{
    public class DelayedDestroy : MonoBehaviour
    {
        public float delay = 1f;

        private void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}