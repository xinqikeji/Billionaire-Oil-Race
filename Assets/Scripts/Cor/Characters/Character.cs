using System.Collections;
using UnityEngine;
using DG.Tweening;
using BlueStellar.Cor.Helpers;
using BlueStellar.Cor.Transports;

namespace BlueStellar.Cor.Characters
{
    public class Character : MonoBehaviour
    {
        #region Variables

        [Header("CharacterColorTyep")]
        [SerializeField] CharacterColorType _characterColorType;

        [Space]
        [Header("CharacterSkin")]
        [SerializeField] GameObject skin;
        
        [Space]
        [Header("CharacterEffects")]
        [SerializeField] ParticleSystem effectDamage;

        [Space]
        [Header("CharacterCanvas")]
        [SerializeField] GameObject canvas;

        [SerializeField] CollectableBarrelField[] collectableBarrelField;
        [SerializeField] CharacterAnimations characterAnimations;
        [SerializeField] StackBarrels _stackBarrels;
        [SerializeField] PlayerMovement playerMovement;
        [SerializeField] BotMovement botMovement;
        [SerializeField] ParticleSystem effectLand;
        [SerializeField] private string _name;
        [SerializeField] private int indexField;
        [SerializeField] private bool isPlayer;
        [SerializeField] private bool isDeactiveCharacter;
        [SerializeField] private bool isKnock;
        [SerializeField] private bool isTransport;
        [SerializeField] private bool isFinish;

        private TransportType _transportType;
        private Transport _transport;
        
        #endregion

        public bool IsPlayer()
        {
            return isPlayer;
        }

        public string SetupName(string newName)
        {
            return _name = newName;
        }

        private void Start()
        {
            if (isPlayer)
            {
                LevelController.Instance.OnLevelFailed.AddListener(CryingReaction);
                LevelController.Instance.OnLevelCompleted.AddListener(VictoryReaction);
            }
            if (!isPlayer)
            {
                LevelController.Instance.OnLevelFailed.AddListener(VictoryReaction);
                LevelController.Instance.OnLevelCompleted.AddListener(CryingReaction);
            }
        }

        public void SetCharacterSettings(CharacterColorType characterColorType)
        {
            _characterColorType = characterColorType;
        }

        public void ActiveCharacter(bool isActive)
        {
            isDeactiveCharacter = isActive;
        }

        #region CharacterTransport

        public void JumpToTransport()
        {
            transform.DOJump(new Vector3(_transport.PointCharacter().position.x,
                _transport.PointCharacter().position.y, _transport.PointCharacter().position.z), 1f, 0, 0.8f);
            StartCoroutine(IE_CharacterTransport(1f));
        }

        public void CharacterToPlatform()
        {
            isTransport = false;
            skin.SetActive(true);
            characterAnimations.JumpAnimation();
            StartCoroutine(IE_JumpToPlatform());
        }

        private void SwitchTransportType()
        {
            switch (_transportType)
            {
                case TransportType.Car:
                    characterAnimations.RunAnimation(1);
                    transform.DOMove(_transport.PointSit().position, 1f).OnComplete(() => EnterTransport());
                    break;
                case TransportType.Ship:
                    characterAnimations.JumpAnimation();
                    StartCoroutine(IE_ReadyToJump());
                    break;
                case TransportType.Helicopter:
                    characterAnimations.JumpAnimation();
                    StartCoroutine(IE_ReadyToJump());
                    break;
            }
        }

        private void EnterTransport()
        {
            transform.DORotate(new Vector3(0f, 90f, 0f), 0.5f).OnComplete(() => characterAnimations.EnterTheTransport());
            StartCoroutine(IE_CharacterTransport(3.5f));
        }

        private IEnumerator IE_CharacterTransport(float time)
        {
            yield return new WaitForSeconds(time);

            collectableBarrelField[indexField].RemoveSpawnedBarrel(_characterColorType);
            indexField++;
            if (indexField == 1)
            {
                collectableBarrelField[indexField].RemoveBarrelType(_characterColorType, 25);
            }
            if (indexField == 2)
            {
                collectableBarrelField[indexField].RemoveBarrelType(_characterColorType, 10);
            }
            if (playerMovement != null) { playerMovement.MovementToTarget(_transport.PointCharacter(), true); }
            if(botMovement != null) { botMovement.ToTransport(_transport.PointCharacter(), true); }
            if (isPlayer)
            {
                CameraController.Instance.PlayerCamActive(false);
                CameraController.Instance.HightCamActive(false);
                CameraController.Instance.TransportCamActive(_transport.PointCharacter(), true);
            }
            _transport.FullTransport();
            canvas.SetActive(false);
            skin.SetActive(false);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void CharacterPlatform()
        {
            if (isPlayer)
            {
                CameraController.Instance.PlayerCamActive(true);
                CameraController.Instance.TransportCamActive(_transport.PointCharacter(), false);
            }
            if (!isPlayer)
                botMovement.ToTransport(_transport.PointLand(), false);
            canvas.SetActive(true);
            effectLand.Play();
        }

        private IEnumerator IE_ReadyToJump()
        {
            yield return new WaitForSeconds(0.6f);

            JumpToTransport();
        }

        private IEnumerator IE_JumpToPlatform()
        {
            yield return new WaitForSeconds(0.5f);

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (isPlayer)
            {
                transform.DOJump(new Vector3(_transport.PointLand().position.x,
                    _transport.PointLand().position.y, _transport.PointLand().position.z), 1f, 0, 0.8f).OnComplete(() => CharacterPlatform());

                if (isPlayer)
                    playerMovement.MovementToTarget(_transport.PointLand(), false);
               
            }
            if (!isPlayer)
            {
                transform.DOJump(new Vector3(_transport.PointLand().position.x,
                    transform.position.y, _transport.PointLand().position.z), 1f, 0, 0.8f).OnComplete(() => CharacterPlatform());
            }
        }

        #endregion

        #region CharacterKnock

        public void KnockCharacter(Transform m)
        {
            if (isKnock)
                return;

            if (isPlayer)
            {
                playerMovement.LockControll(true);
            }
            if (!isPlayer)
            {
                botMovement.PushBot(m);
            }
            effectDamage.Play();
            _stackBarrels.DestroyedStack();
            characterAnimations.KnockAnimation();
            isKnock = true;
            StartCoroutine(IE_Return());
        }

        private IEnumerator IE_Return()
        {
            yield return new WaitForSeconds(2.1f);

            if (isPlayer)
            {
                playerMovement.LockControll(false);
            }
            if (!isPlayer)
            {
                botMovement.ReturnMove();
            }
            isKnock = false;
        }

        #endregion

        #region CharacterReactions

        private void VictoryReaction()
        {
            characterAnimations.VictoryAnimation();
            skin.transform.DORotate(new Vector3(0f, 180f, 0f), 0.3f);
            if (isPlayer)
            {
                playerMovement.LockControll(true);
                CameraController.Instance.PlayerCamActive(false);
                CameraController.Instance.FinishCamActive(true);
                return;
            }
            if (!isPlayer)
            {
                botMovement.StopMovement(true);
            }
        }

        private void CryingReaction()
        {
            characterAnimations.CryingAnimation();
            skin.transform.DORotate(new Vector3(0f, 180f, 0f), 0.3f);
            if (isPlayer)
            {
                playerMovement.LockControll(true);
                CameraController.Instance.PlayerCamActive(false);
                CameraController.Instance.FinishCamActive(true);
                return;
            }
            if (!isPlayer)
            {
                botMovement.StopMovement(true);
            }
        }

        #endregion

        #region CharacterCollisions

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Barrel")
            {
                CollectableBarrel _ball = other.GetComponent<CollectableBarrel>();

                if (!_ball.IsTrueCharacter(_characterColorType))
                    return;

                _stackBarrels.AddCollectableBall(_ball);
                if (isPlayer)
                {
                    SoundManager.Instance.SoundClaimActive();
                    VibrationController.Instance.ClaimVibration();
                }
            }

            if (other.gameObject.tag == "Character")
            {
                Character character = other.GetComponent<Character>();
                StackBarrels stackBalls = other.GetComponent<StackBarrels>();

                if (isDeactiveCharacter)
                    return;

                if (_stackBarrels.AmmountBalls() == stackBalls.AmmountBalls())
                    return;

                if (isPlayer)
                {
                    SoundManager.Instance.SoundHitActive();
                    VibrationController.Instance.KnockVibration();
                }

                if (_stackBarrels.AmmountBalls() >= stackBalls.AmmountBalls())
                {
                    character.KnockCharacter(transform);
                    return;
                }

                KnockCharacter(other.transform);
            }

            if(other.gameObject.tag == "Gate")
            {
                Gate gate = other.GetComponent<Gate>();
                gate.ActivetedBonus(_stackBarrels, _characterColorType);
                if (isPlayer)
                {
                    switch (gate.GetGateType())
                    {
                        case GateType.Positive:
                            SoundManager.Instance.SoundClaimActive();
                            break;
                        case GateType.Negative:
                            SoundManager.Instance.SoundNegativeBonusActive();
                            break;
                    }
                }
            }

            if (other.gameObject.tag == "Finish")
            {
                if (isFinish)
                    return;

                if (isPlayer) 
                {
                    other.GetComponent<Finish>().Active();
                    _stackBarrels.ClearStack();
                    playerMovement.LockControll(true);
                    playerMovement.MoveToFinish();
                    CameraController.Instance.PlayerCamActive(false);
                    CameraController.Instance.HightCamActive(false);
                    CameraController.Instance.VeryHightCamActive(false);
                    CameraController.Instance.FinishCamActive(true);
                }
                isFinish = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "TransportField")
            {
                if (isTransport)
                    return;

                if(other.GetComponentInParent<Transport>() == null)
                {
                    return;
                }

                _transport = other.GetComponentInParent<Transport>();

                if (_transport.IsFullTransport())
                {
                    if (_characterColorType == _transport.GetColorType())
                    {
                        _transport.SetCharacter(this);
                        _transportType = _transport.GetTransportType();
                        if (isPlayer) { playerMovement.LockControll(true); }
                        if (!isPlayer) { botMovement.StopMovement(true); }
                        SwitchTransportType();
                        isTransport = true;
                    }
                    return;
                }

                _transport.SetupNameCharacter(_name);
                _stackBarrels.UnstackCollectableBarrel(_transport);

                if (_stackBarrels.AmmountBalls() == 0)
                    return;

                if (isPlayer)
                {
                    VibrationController.Instance.UnstackVibration();
                }
            }
        }
        
        #endregion
    }
}
