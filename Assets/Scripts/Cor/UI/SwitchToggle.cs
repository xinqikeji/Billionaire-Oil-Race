using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlueStellar.Cor
{
    public class SwitchToggle : MonoBehaviour
    {
        #region Variables

        [Header("ToggleImg")]
        [SerializeField] RectTransform uiHandleRectTransform;
        [SerializeField] Image bgImg;
        [SerializeField] Image handleImg;
        [SerializeField] Sprite onBg;
        [SerializeField] Sprite onImg;
        [SerializeField] Sprite offBg;
        [SerializeField] Toggle toggle;

        [Space]
        [Header("VibrationToggle")]
        [SerializeField] VibrationController vibrationController;
        [SerializeField] private bool isVibrationToggle;

        [Space]
        [Header("SoundToggle")]
        [SerializeField] SoundManager soundManager;
        [SerializeField] private bool isSoundToggle;

        Vector2 handlePosition;

        #endregion

        private void Start()
        {
            handlePosition = uiHandleRectTransform.anchoredPosition;

            toggle.onValueChanged.AddListener(OnSwitch);

            if (isVibrationToggle)
            {
                if (!vibrationController.ISOffVibration())
                    toggle.isOn = true;
            }

            if (isSoundToggle)
            {
                if (!soundManager.IsOffSound())
                    toggle.isOn = true;
            }

            if (toggle.isOn)
                OnSwitch(true);
        }

        private void OnSwitch(bool on)
        {
            uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -0.1f : handlePosition, .4f).SetEase(Ease.InOutBack);

            if (on)
            {
                bgImg.sprite = onBg;
                handleImg.gameObject.SetActive(true);
                handleImg.sprite = onImg;
                if(isVibrationToggle) vibrationController.VibrationOffAndOn(false);
                if (isSoundToggle) soundManager.SoundOffAndOn(false);
                return;
            }

            bgImg.sprite = offBg;
            handleImg.gameObject.SetActive(false);
            if (isVibrationToggle) vibrationController.VibrationOffAndOn(true);
            if (isSoundToggle) soundManager.SoundOffAndOn(true);
        }
    }
}
