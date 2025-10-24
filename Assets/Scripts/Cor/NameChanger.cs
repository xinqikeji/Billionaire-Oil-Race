using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlueStellar.Cor
{
    public class NameChanger : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputField;
        [SerializeField] PlayerName _playerName;

        private void Start()
        {
            _playerName = GameObject.FindObjectOfType<PlayerName>();
            if(_playerName.Name() != "Íæ¼Ò")
                inputField.text = _playerName.Name();
        }

        public void ChangeName()
        {
            _playerName.NewName(inputField.text);
        }
    }
}
