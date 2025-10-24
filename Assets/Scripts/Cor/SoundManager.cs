using UnityEngine;

namespace BlueStellar.Cor
{
    public class SoundManager : MonoBehaviour
    {
        #region Singelton

        public static SoundManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        #region Variables

        [Header("Sounds")]
        [SerializeField] AudioSource soundClaim;
        [SerializeField] AudioSource soundNegativeBonus;
        [SerializeField] AudioSource soundHit;

        private bool isOffSound;

        #endregion

        public bool IsOffSound()
        {
            Load();
            return isOffSound;
        }

        private void Start()
        {
            Load();
        }

        public void SoundOffAndOn(bool isOff)
        {
            isOffSound = isOff;
            Save();
        }

        public void SoundClaimActive()
        {
            if (isOffSound)
                return;

            soundClaim.Play();
        }

        public void SoundNegativeBonusActive()
        {
            if (isOffSound)
                return;

            soundNegativeBonus.Play();
        }

        public void SoundHitActive()
        {
            if (isOffSound)
                return;

            soundHit.Play();
        }

        #region Load&Save

        private void Load()
        {
            isOffSound = ES3.Load("isOffSound", isOffSound);
        }

        private void Save()
        {
            ES3.Save("isOffSound", isOffSound);
        }

        #endregion
    }
}
