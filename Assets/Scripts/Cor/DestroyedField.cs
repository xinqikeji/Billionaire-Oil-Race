using UnityEngine;

namespace BlueStellar.Cor
{
    public class DestroyedField : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Barrel"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
