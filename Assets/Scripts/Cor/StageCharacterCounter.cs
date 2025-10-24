using UnityEngine;
using BlueStellar.Cor.Characters;

namespace BlueStellar.Cor
{
    public class StageCharacterCounter : MonoBehaviour
    {
        [SerializeField] private int maxAmmount;
        [SerializeField] private int ammountCharacters;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Character"))
            {
                if (other.GetComponent<Character>().IsPlayer())
                {
                    return;
                }

                ammountCharacters++;

                if(ammountCharacters >= maxAmmount)
                {
                    LevelController.Instance.LevelFailed();
                }
            }
        }
    }
}
