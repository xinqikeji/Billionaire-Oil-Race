using System.Collections;
using UnityEngine;
using DG.Tweening;
using BlueStellar.Cor.Characters;

namespace BlueStellar.Cor
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("MovementSettings")]
        [SerializeField] Transform finishPoint;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundMask;
        [SerializeField] private float gravityMultyplier;
        [SerializeField] private float groundDistance;
        [SerializeField] private float speedMovement;
        [SerializeField] private float speedRotate;
        [SerializeField] private float speedFinish;
        [SerializeField] private bool isLockControll;
        [SerializeField] CharacterAnimations characterAnimations;
        [SerializeField] Transform characterPoint;
        [SerializeField] Character ch;

        private bool isFinish;
        private bool canFinishRun;

        Vector3 gravityVelocity;
        Transform _transformPlayer;
        CharacterController _characterController;
        FloatingJoystick _joystick;

        private void Start()
        {
            _transformPlayer = GetComponent<Transform>();
            _characterController = GetComponent<CharacterController>();
            _joystick = GameObject.FindObjectOfType<FloatingJoystick>();
            LockControll(true);
            LevelController.Instance.OnLevelStart.AddListener(Move);
            LevelController.Instance.OnLevelFailed.AddListener(Stop);
        }

        private void FixedUpdate()
        {
            FinishMovement();
        }

        private void Update()
        {
            if (isLockControll || isFinish)
                return;

            MovementControll();
        }

        public void LockControll(bool lockControll)
        {
            isLockControll = lockControll;
        }

        public void Move()
        {
            LockControll(false);
        }

        public void Stop()
        {
            LockControll(true);
        }

        public void MovementToTarget(Transform target, bool isParent)
        {
            _transformPlayer.DOMove(new Vector3(target.position.x,
                _transformPlayer.position.y, target.position.z), 0.1f).OnComplete(() => SetPos());
            if (isParent)
            {
                _transformPlayer.transform.parent = target;
                LockControll(true);
            }
            if (!isParent)
            {
                _transformPlayer.transform.parent = null;
                StartCoroutine(IE_ReturnMovement());
            }
        }

        public void MoveToFinish()
        {
            ch.transform.position = characterPoint.position;
            characterAnimations.RunAnimation(1);
            isFinish = true;
            StartCoroutine(IE_CanRun());
        }

        private void SetPos()
        {
            ch.transform.position = characterPoint.position;
            ch.transform.rotation = characterPoint.rotation;
        }

        private void FinishMovement()
        {
            if (!isFinish || !canFinishRun)
                return;

            if (_transformPlayer.position == finishPoint.position)
            {
                LevelController.Instance.LevelCompleted();
                isFinish = true;
                return;
            }

            _transformPlayer.position = Vector3.MoveTowards(_transformPlayer.position, finishPoint.position, speedFinish);
            Vector3 look = finishPoint.position;
            look.y = _transformPlayer.position.y;
            _transformPlayer.DOLookAt(look, 0.3f);
        }

        private void MovementControll()
        {
            if (Input.GetMouseButton(0))
            {
                if (_joystick != null)
                {
                    var xInput = _joystick.Horizontal;
                    var yInput = _joystick.Vertical;

                    _characterController.Move((Vector3.right * xInput + Vector3.forward * yInput) * speedMovement * Time.deltaTime);

                    _transformPlayer.LookAt(transform.position + (Vector3.right * xInput + Vector3.forward * yInput) * speedRotate * Time.deltaTime);

                    var isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                    if (isGrounded && gravityVelocity.y < 0 && isGrounded && gravityVelocity.y > 1)
                    {
                        gravityVelocity.y = -2f;
                    }

                    gravityVelocity += Vector3.up * gravityMultyplier * Time.deltaTime;
                    _characterController.Move(gravityVelocity);

                    if (xInput >= 0.1f || xInput <= -0.1f ||
                       yInput >= 0.1f || yInput <= -0.1f)
                    {
                        characterAnimations.RunAnimation(1);
                    }
                    else { characterAnimations.RunAnimation(0); }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                characterAnimations.RunAnimation(0);
            }
        }

        private IEnumerator IE_ReturnMovement()
        {
            yield return new WaitForSeconds(2f);

            LockControll(false);
        }

        private IEnumerator IE_CanRun()
        {
            yield return new WaitForSeconds(1f);

            canFinishRun = true;
        }
    }
}
