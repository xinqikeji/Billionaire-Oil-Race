using UnityEngine;

namespace BlueStellar.Cor.Characters
{
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] Animator _anim;

        public void RunAnimation(float speed)
        {
            _anim.SetFloat("Run", speed);
        }

        public void JumpAnimation()
        {
            _anim.SetTrigger("Jump");
        }

        public void KnockAnimation()
        {
            _anim.SetTrigger("Knock");
        }

        public void CryingAnimation()
        {
            _anim.SetTrigger("Crying");
        }

        public void VictoryAnimation()
        {
            _anim.SetTrigger("Victory");
        }

        public void EnterTheTransport()
        {
            _anim.SetTrigger("EnterTransport");
        }

        public void ExitTransport()
        {
            _anim.SetTrigger("ExitTransport");
        }
    }
}
