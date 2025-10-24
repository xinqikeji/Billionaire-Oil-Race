using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;
using LiquidVolumeFX;
using BlueStellar.Cor.Helpers;
using BlueStellar.Cor.Characters;

namespace BlueStellar.Cor.Transports
{
    public class Transport : MonoBehaviour
    {
        [System.Serializable]
        public class ColorTransport
        {
            public CharacterColorType _colorType;
            public Transform debug;
            public MeshRenderer _transportMesh;
            public Material matFullTransport;
            public LiquidVolume _liquidVolume;
            public float maxProgressValue;
            public float progress;
            public string _name;
        }

        #region Variables

        [Header("TransportType")]
        [SerializeField] TransportType _transportType;

        [Space]
        [Header("ColorTransports")]
        [SerializeField] List<ColorTransport> colorTransports = new List<ColorTransport>();

        [Space]
        [Header("TransportDetails")]
        [SerializeField] GameObject transperentBody;
        [SerializeField] MeshRenderer[] details;
        [SerializeField] Material[] matDetails;

        [Space]
        [Header("EffectsTransport")]
        [SerializeField] ParticleSystem[] effects;
        [SerializeField] ParticleSystem effectOpen;

        [Space]
        [Header("AnimationTransport")]
        [SerializeField] Animator animTransport;
        [SerializeField] DOTweenAnimation openPunch;
        [SerializeField] DOTweenAnimation dopAnim;

        [Space]
        [Header("CanvasTransport")]
        [SerializeField] GameObject canvasTransport;
        [SerializeField] GameObject smallCanvasTransport;
        [SerializeField] TextMeshProUGUI textPercent;
        [SerializeField] TextMeshProUGUI textSecondPersent;
        [SerializeField] TextMeshProUGUI textNameCharacter;
        [SerializeField] TextMeshProUGUI textNameSecondCharacter;

        [Space]
        [Header("TransportField")]
        [SerializeField] GameObject field;

        [Space]
        [Header("TransportPoints")]
        [SerializeField] Transform point;
        [SerializeField] Transform pointSit;
        [SerializeField] Transform target;
        [SerializeField] Transform p;
        [SerializeField] Transform pointPlatform;

        [Space]
        [Header("TransportMovement")]
        [SerializeField] TransportMovement _transportMovement;

        public Transform dir;
        public bool isDebug;
        private int fillingPercent;
        private int fillingSecondPrecent;
        private bool isFullTransport;
        private string _characterName;
        private CharacterColorType _colorType;
        private Character _character;
        private ColorTransport _colorTransport;

        #endregion

        #region GetTransportVariables

        public bool IsFullTransport()
        {
            return isFullTransport;
        }

        public TransportType GetTransportType()
        {
            return _transportType;
        }

        public CharacterColorType GetColorType()
        {
            return _colorType;
        }

        public Transform PointSit()
        {
            return pointSit;
        }

        public Transform PointCharacter()
        {
            return point;
        }

        public Transform PointPlatform()
        {
            return pointPlatform;
        }

        public Transform PointLand()
        {
            return p;
        }

        #endregion

        private void Start()
        {
            if (isDebug)
            {
                foreach(var i in colorTransports)
                {
                    i.debug = dir;
                }
            }
        }

        public void SetCharacter(Character character)
        {
            _character = character;
        }

        public void FullTransport()
        {
            field.transform.parent = null;
            _transportMovement.ActiveMovement(true);
        }

        public void StopTransport()
        {
            animTransport.SetBool("Move", false);
            _character.CharacterToPlatform();
            foreach (var i in effects)
            {
                i.Stop();
            }
        }

        public void SetupNameCharacter(string characterName)
        {
            _characterName = characterName;
        }

        public void SetupMaterialSettings(CharacterColorType characterColorType)
        {
            foreach(var i in colorTransports)
            {
                if(i._colorType == characterColorType)
                {
                    i._name = _characterName;
                    _colorTransport = i;
                    break;
                }
            }

            _colorTransport._liquidVolume.level += _colorTransport.progress;
            FillingPercentageCalculation();
            CheckTransport();
        }

        private void CheckTransport()
        {
            if (_colorTransport._liquidVolume.level >= _colorTransport.maxProgressValue)
            {
                Destroy(_colorTransport._liquidVolume);
                var mats = _colorTransport._transportMesh.materials;
                for(int i = 0; i < mats.Length; i++)
                {
                    mats[i] = _colorTransport.matFullTransport;
                }
                _colorTransport._transportMesh.materials = mats;

                for(int i = 0; i < details.Length; i++)
                {
                    details[i].material = matDetails[i];
                }

                openPunch.DORestart();
                canvasTransport.transform.DOScale(0, 0.3f);
                transperentBody.SetActive(false);
                effectOpen.Play();

                if (effects.Length > 0)
                {
                    foreach (var i in effects)
                    {
                        if (i != null)
                            i.Play();
                    }
                }

                if(dopAnim != null)
                {
                    dopAnim.DOPlay();
                }
                animTransport.SetBool("Move", true);
                _colorType = _colorTransport._colorType;
                isFullTransport = true;
            }
        }

        private void FillingPercentageCalculation()
        {
            colorTransports = colorTransports.OrderBy(i => i._liquidVolume.level).ToList();
            foreach(var i in colorTransports) { i._transportMesh.gameObject.SetActive(false); }
            colorTransports[colorTransports.Count - 1]._transportMesh.gameObject.SetActive(true);
            fillingSecondPrecent = (int)(colorTransports[colorTransports.Count - 2]._liquidVolume.level / (colorTransports[colorTransports.Count - 2].maxProgressValue / 100));
            fillingPercent = (int)(colorTransports[colorTransports.Count - 1]._liquidVolume.level / (colorTransports[colorTransports.Count - 1].maxProgressValue / 100));
            textNameCharacter.text = colorTransports[colorTransports.Count - 1]._name;
            textNameSecondCharacter.text = colorTransports[colorTransports.Count - 2]._name;
            if (fillingPercent >= 1 && fillingPercent < 100) { canvasTransport.SetActive(true); }
            if(fillingSecondPrecent >= 1 && fillingSecondPrecent < 100) { smallCanvasTransport.SetActive(true); }

            if(fillingPercent > 100)
            {
                fillingPercent = 100;
            }

            textPercent.text = fillingPercent + "%";
            textSecondPersent.text = fillingSecondPrecent + "%";
        }
    }
}
