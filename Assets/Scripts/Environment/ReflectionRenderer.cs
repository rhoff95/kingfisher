using UnityEngine;

namespace Environment
{
    public class ReflectionRenderer : MonoBehaviour
    {
        public Camera mainCamera;

        private void Update()
        {
            var mainCameraTransform = mainCamera.transform;
            var mainCameraPosition = mainCameraTransform.position;
            var newPosition = new Vector3(mainCameraPosition.x, 0 - transform.localScale.y / 2, -3);
            transform.position = newPosition;
        }
    }
}