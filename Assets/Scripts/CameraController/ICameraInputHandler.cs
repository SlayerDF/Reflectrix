using UnityEngine;

namespace Reflectrix.CameraController
{
    public interface ICameraInputHandler
    {
        /// <summary>
        /// Initialize camera input handler.
        /// </summary>
        void Initialize(CameraController cameraController, PlayerControls controls);
    }
}