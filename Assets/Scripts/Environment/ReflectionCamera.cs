using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Camera))]
    public class ReflectionCamera : MonoBehaviour
    {
        public Camera mainCamera;
        private Camera _reflectionCamera;

        private void Awake()
        {
            _reflectionCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            var mainCameraTransform = mainCamera.transform;
            var mainCameraPosition = mainCameraTransform.position;
            var newPosition = new Vector3(mainCameraPosition.x, 0 + _reflectionCamera.orthographicSize, _reflectionCamera.transform.position.z);
            _reflectionCamera.transform.position = newPosition;
        }
    }
}
