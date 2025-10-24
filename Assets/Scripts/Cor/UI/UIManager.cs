using UnityEngine;
using UnityEngine.SceneManagement;
namespace BlueStellar.Cor
{
    public class UIManager : MonoBehaviour
    {
        #region Singelton

        public static UIManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        [Header("Screens")]
        [SerializeField] GameObject joystickScreen;
        [SerializeField] GameObject tutorialScreen;
        [SerializeField] GameObject startScreen;
        [SerializeField] GameObject settingsButtonScreen;
        [SerializeField] GameObject settingsScreen;
        [SerializeField] GameObject moneyScreen;
        [SerializeField] GameObject loseScreen;
        [SerializeField] GameObject winScreen;

        public void JoystickScreen(bool isActive)
        {
            joystickScreen.SetActive(isActive);   
        }

        public void TutorialScreen(bool isActive)
        {
            tutorialScreen.SetActive(isActive);
        }

        public void StartScreen(bool isActive)
        {
            startScreen.SetActive(isActive);   
        }

        public void SettingsButtonScreen(bool isActive)
        {
            settingsButtonScreen.SetActive(isActive);
        }

        public void SettingsScreen(bool isActive)
        {
            settingsScreen.SetActive(isActive);
        }

        public void MoneyScreen(bool isActive)
        {
            moneyScreen.SetActive(isActive);
        }

        public void WinScreen(bool isActive)
        {
            winScreen.SetActive(isActive);
        }

        public void LoseScreen(bool isActive)
        {
            loseScreen.SetActive(isActive);   
        }

        public void OpenShop()
        {
            SceneManager.LoadScene("ShopScene");
        }
    }
}
