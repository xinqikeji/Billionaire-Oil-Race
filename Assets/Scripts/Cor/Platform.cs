using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueStellar.Cor
{
    public class Platform : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Barrel"))
            {
                if(collision.gameObject.GetComponent<CollectableBarrel>() != null)
                {
                    collision.gameObject.GetComponent<CollectableBarrel>().Normal();
                }
            }
        }
    }
}
