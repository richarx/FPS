using UnityEngine;

namespace Tools_and_Scripts
{
    public class CameraScreenPosition : MonoBehaviour
    {
        public static CameraScreenPosition instance;

        private Camera mainCamera;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        public Vector3 WorldToScreen(Vector3 position)
        {
            return mainCamera.WorldToScreenPoint(position);
        }
        
        public Vector3 ScreenToWorld(Vector2 position)
        {
            return mainCamera.ScreenToWorldPoint(position.ToVector3(mainCamera.nearClipPlane));
        }

        public Vector2 GetScreenCenterPosition()
        {
            return new Vector2(mainCamera.pixelWidth / 2.0f, mainCamera.pixelHeight / 2.0f);
        }
    }
}
