using UnityEngine;
using DG.Tweening;
using BlueStellar.Cor.Helpers;
using BlueStellar.Cor.Transports;

namespace BlueStellar.Cor
{
    public class CollectableBarrel : MonoBehaviour
    {
        #region Variables

        [Header("BarrelsMaterials")]
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] Material[] mainMatsBarrel;
        [SerializeField] Material[] additionalMatsBarrel;

        [Header("BarrelPhysics")]
        [SerializeField] Rigidbody _rb;

        [Header("BarrelType")]
        [SerializeField] CharacterColorType _ballType;

        private bool isBallDestroyed;
        private bool cantStack;

        CharacterColorType newType;
        CollectableBarrelField _collectableBarrelField;

        #endregion

        public bool IsTrueCharacter(CharacterColorType characterColorType)
        {
            if (cantStack)
                return false;

            if (_ballType == characterColorType ||
                _ballType == CharacterColorType.Neutral)
            {
                newType = characterColorType;
                if (_ballType != newType)
                {
                    _ballType = newType;
                    SwitchBarrelColor();
                }
                return true;
            }

            return false;
        }

        public CharacterColorType Type()
        {
            return _ballType;
        }

        private void Start()
        {
            _collectableBarrelField = GetComponentInParent<CollectableBarrelField>();
        }

        public void BallInStack()
        {
            cantStack = true;
            _rb.isKinematic = true;
            if(_collectableBarrelField != null)
                _collectableBarrelField.RemoveCollectableBarrel(this);
            transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.15f, 1);
        }
       
        public void BallNeutral()
        {
            cantStack = true;
            transform.DOScale(0.4277498f, 0.1f);
            transform.SetParent(null);
            gameObject.GetComponent<Collider>().isTrigger = false;
            _rb.isKinematic = false;
            _rb.AddForce(new Vector3(0f, 5f, -2f), ForceMode.Impulse);
            _ballType = CharacterColorType.Neutral;
            SwitchBarrelColor();
        }

        public void Normal()
        {
            _ballType = CharacterColorType.Neutral;
            gameObject.GetComponent<Collider>().isTrigger = true;
            _rb.isKinematic = true;
            cantStack = false;
        }

        public void BarrelToTransport(Transport transport)
        {
            if (!isBallDestroyed)
            {
                transport.SetupMaterialSettings(_ballType);
                isBallDestroyed = true;
                Destroy(gameObject, 0.4f);
            }
        }

        private void SwitchBarrelColor()
        {
            int indexMat = 0;

            switch (_ballType)
            {
                case CharacterColorType.Neutral:
                    indexMat = 0;
                    break;
                case CharacterColorType.Blue:
                    indexMat = 1;
                    break;
                case CharacterColorType.Pink:
                    indexMat = 2;
                    break;
                case CharacterColorType.Yellow:
                    indexMat = 3;
                    break;
                case CharacterColorType.Red:
                    indexMat = 4;
                    break;
                case CharacterColorType.Green:
                    indexMat = 5;
                    break;
                case CharacterColorType.Purple:
                    indexMat = 6;
                    break;
            }

            var newMats = meshRenderer.materials;
            newMats[0] = mainMatsBarrel[indexMat];
            newMats[1] = additionalMatsBarrel[indexMat];
            meshRenderer.materials = newMats;
        }
    }
}
