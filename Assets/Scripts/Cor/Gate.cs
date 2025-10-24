using UnityEngine;
using TMPro;
using DG.Tweening;
using BlueStellar.Cor.Helpers;

namespace BlueStellar.Cor
{
    public class Gate : MonoBehaviour
    {
        #region Variables

        [SerializeField] GateType _gatesType;
        [SerializeField] GameObject phon;
        [SerializeField] TextMeshProUGUI textGates;
        [SerializeField] private int numberGates;
        [SerializeField] CollectableBarrel[] collectableBarrels;
        private int index;
        private string symbolGate;

        CharacterColorType _characterColor;
        StackBarrels _stackBarrels;
        GatesSpawner _gatesSpawner;

        #endregion

        public GateType GetGateType()
        {
            return _gatesType;
        }

        public void SetGatesSettings(GatesSpawner gatesSpawner, GateType gatesType)
        {
            transform.DOScale(transform.localScale, 0.5f).From(0);
            _gatesSpawner = gatesSpawner;
            _gatesType = gatesType;
            numberGates = Random.Range(1, 10);
            SwitchGatesText();
        }

        public void ActivetedBonus(StackBarrels stackBarrels, CharacterColorType characterColorType)
        {
            _stackBarrels = stackBarrels;
            _characterColor = characterColorType;

            switch (_characterColor)
            {
                case CharacterColorType.Blue:
                    index = 0;
                    break;
                case CharacterColorType.Pink:
                    index = 1;
                    break;
                case CharacterColorType.Yellow:
                    index = 2;
                    break;
                case CharacterColorType.Red:
                    index = 3;
                    break;
                case CharacterColorType.Green:
                    index = 4;
                    break;
                case CharacterColorType.Purple:
                    index = 5;
                    break;
            }

            switch (_gatesType)
            {
                case GateType.Positive:
                    PositiveBonus();
                    break;
                case GateType.Negative:
                    NegativeBonus();
                    break;
            }

            phon.SetActive(false);
            _gatesSpawner.RemoveGate(this);
            transform.DOScale(0, 0.5f).OnComplete(() => Destroy(gameObject, 0.2f));
        }

        private void PositiveBonus()
        {
            for (int i = 0; i < numberGates; i++)
            {
                GameObject ball = Instantiate(collectableBarrels[index].gameObject, transform.position, transform.rotation);
                _stackBarrels.AddCollectableBall(ball.GetComponent<CollectableBarrel>());
            }
        }

        private void NegativeBonus()
        {
            for (int i = 0; i < numberGates; i++)
            {
               _stackBarrels.RemoveColleactbleBall();
            }
        }

        private void SwitchGatesText()
        {
            switch (_gatesType)
            {
                case GateType.Positive:
                    symbolGate = "+";
                    break;
                case GateType.Negative:
                    symbolGate = "-";
                    break;
            }

            textGates.text = symbolGate + numberGates;
        }
    }
}