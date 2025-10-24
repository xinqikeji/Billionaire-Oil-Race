using UnityEngine;
using Cinemachine;

namespace BlueStellar.Cor
{
    public class CameraController : MonoBehaviour
    {
        #region Singleton

        public static CameraController Instance;

        private void Awake()
        {
            Instance = this;    
        }

        #endregion

        #region Variables

        [SerializeField] CinemachineVirtualCamera playerCam;
        [SerializeField] CinemachineVirtualCamera hightCam;
        [SerializeField] CinemachineVirtualCamera veryHightCam;
        [SerializeField] CinemachineVirtualCamera transportCam;
        [SerializeField] CinemachineVirtualCamera finishcam;

        Transform player;

        #endregion

        private void Start()
        {
            SetTargetCameras();
        }

        public void PlayerCamActive(bool isActive)
        {
            playerCam.gameObject.SetActive(isActive);
        }

        public void HightCamActive(bool isActive)
        {
            hightCam.gameObject.SetActive(isActive);
        }

        public void VeryHightCamActive(bool isActive)
        {
            veryHightCam.gameObject.SetActive(isActive);
        }

        public void TransportCamActive(Transform point, bool isActive)
        {
            transportCam.gameObject.SetActive(isActive);
            if (isActive)
            {
                transportCam.Follow = point;
                transportCam.LookAt = point;
            }
        }

        public void FinishCamActive(bool isAcitve)
        {
            finishcam.gameObject.SetActive(isAcitve);
        }

        private void SetTargetCameras()
        {
            player = GameObject.FindObjectOfType<PlayerMovement>().transform;
            playerCam.Follow = player;
            playerCam.LookAt = player;
            hightCam.Follow = player;
            hightCam.LookAt = player;
            veryHightCam.Follow = player;
            veryHightCam.LookAt = player;
            finishcam.Follow = player;
            finishcam.LookAt = player;
        }
    }
}
