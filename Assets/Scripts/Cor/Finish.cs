using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueStellar.Cor
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] effects;

        public void Active()
        {
            StartCoroutine(IE_Effects());
        }

        private IEnumerator IE_Effects()
        {
            foreach(var i in effects)
            {
                i.Play();
                yield return new WaitForSeconds(0.35f);
            }
        }
    }
}
