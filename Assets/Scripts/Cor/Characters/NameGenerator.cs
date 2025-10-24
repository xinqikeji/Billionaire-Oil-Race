using UnityEngine;
using TMPro;
using BlueStellar.Cor.Characters;

namespace BlueStellar.Cor
{
    public class NameGenerator : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textName;
        [SerializeField] string[] names;
        [SerializeField] Character _character;

        private string _name;

        public string Name()
        {
            return _name;
        }

        private void Start()
        {
            int randomIndex = Random.Range(0, names.Length);
            textName.text = names[randomIndex];
            _name = names[randomIndex];
            _character.SetupName(_name);
        }
    }
}
