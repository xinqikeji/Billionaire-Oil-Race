using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using BlueStellar.Cor.Helpers;
using BlueStellar.Cor.Transports;
using BlueStellar.Cor.Characters;

namespace BlueStellar.Cor
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotMovement : MonoBehaviour
    {
        #region Variables

        [SerializeField] Transform[] monsterPointsStage1;
        [SerializeField] Transform[] monsterPStage2;
        [SerializeField] Transform[] mPSgar3;
        [SerializeField] private float timeToMonster;
        [SerializeField] private float timer;
        [SerializeField] private bool isStopMovement;
        [SerializeField] private bool toTransport;
        [SerializeField] CharacterColorType colorType;
        [SerializeField] List<Vector3> points = new List<Vector3>();
        [SerializeField] private int min;
        [SerializeField] private int max;
        [SerializeField] private int indexMonsterPoint;
        private bool inMonster;

        [SerializeField] Rigidbody _rb;
        [SerializeField] NavMeshAgent _agent;
        [SerializeField] StackBarrels _stackBalls;
        [SerializeField] CollectableBarrelField[] _collectableBallsField;
        [SerializeField] private int indexM;
        Vector3 ball;
        [SerializeField] Character character;
        [SerializeField] CharacterAnimations characterAnimations;
        [SerializeField] Transform ch;
        [SerializeField] Transform characterPoint;
        public int index;

        #endregion

        private void Start()
        {
            isStopMovement = true;
            LevelController.Instance.OnLevelStart.AddListener(Move);
            LevelController.Instance.OnLevelFailed.AddListener(Stop);
        }

        private void Update()
        {
            if (isStopMovement)
                return;

            if (!toTransport)
            {
                if (timer >= timeToMonster)
                {
                    int random = Random.Range(min, max);

                    if (_stackBalls.AmmountBalls() >= random)
                    {
                        if (indexM == 0)
                        {
                            _agent.SetDestination(monsterPointsStage1[indexMonsterPoint].position);
                        }
                        if (indexM == 1)
                        {
                            int _monsterPStage2 = monsterPStage2.Length;
                            int _PStage2 = Random.Range(0, (_monsterPStage2-1));
                            _agent.SetDestination(monsterPStage2[_PStage2].position);
                        }
                        if (indexM == 2)
                        {
                            int _monsterPStage3 = mPSgar3.Length;
                            int _PStage3 = Random.Range(0, (_monsterPStage3 - 1));
                            _agent.SetDestination(mPSgar3[_PStage3].position);
                        }
                        timer = 0f;
                        toTransport = true;
                        return;
                    }

                    timer = 0f;
                }
            }

            if (toTransport)
                return;

            timer += Time.deltaTime;

            NewMove();
        }

        #region PlatformMovement

        private void FixedUpdate()
        {
            if(points.Count == 0 && !isStopMovement)
            {
                SetPoints();
                NewPoint();
                UpdateMove();
            }
        }

        private void SetPoints()
        {
            points = _collectableBallsField[indexM].ListTypeBarrels(colorType);
        }

        private void NewMove()
        {
            if (Vector3.Distance(transform.position, ball) < 1)
            {
                NewPoint();
                UpdateMove();
            }
            else
            {
                characterAnimations.RunAnimation(1);
            }
        }

        private void UpdateMove()
        {
            ball = points[index];
            if (_agent.enabled)
                _agent.SetDestination(ball);
            characterAnimations.RunAnimation(1);
        }

        private void NewPoint()
        {
            index = Random.Range(0, points.Count - 1);
        }

        #endregion

        public void Move()
        {
            SetPoints();
            index = Random.Range(0, points.Count - 1);
            UpdateMove();
            StopMovement(false);
        }

        public void Stop()
        {
            StopMovement(true);
        }

        public void StopMovement(bool isActive)
        {
            if (isActive)
            {
                isStopMovement = isActive;
                _agent.enabled = false;
                return;
            }

            _rb.isKinematic = true;
            isStopMovement = false;
            _agent.enabled = true;
            toTransport = false;
        }

        public void PushBot(Transform pushTarget)
        {
            _agent.enabled = false;
            _rb.isKinematic = false;
            StopMovement(true);
            Vector3 pushDirection = new Vector3(transform.position.x - pushTarget.position.x,
                transform.position.y, transform.position.z - pushTarget.position.z);
            _rb.AddForce(pushDirection * 5f, ForceMode.Impulse);
        }

        public void RestartMovement()
        {
            _agent.enabled = true;
            SetPoints();
            NewPoint();
            UpdateMove();
        }

        public void ReturnMove()
        {
            StopMovement(false);
            NewPoint();
            UpdateMove();
        }

        public void ToTransport(Transform point, bool isParent)
        {
            StopMovement(true);
            transform.DOMove(new Vector3(point.position.x,
                transform.position.y, point.position.z), 0.1f).OnComplete(() => SetPos());
            if (isParent)
            {
                indexM++;
                transform.transform.parent = point;
            }
            if (!isParent)
                transform.transform.parent = null;
        }

        private void SetPos()
        {
            ch.transform.position = characterPoint.position;
            ch.transform.rotation = characterPoint.rotation;
            SetPoints();
            StopMovement(false);
            NewPoint();
            UpdateMove();
            toTransport = false;
        }

        #region BotCollisions

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "TransportField")
            {
                if (toTransport)
                {
                    if (inMonster)
                        return;

                    if (other.GetComponentInParent<Transport>() == null)
                    {
                        indexMonsterPoint++;

                        if (indexM == 0)
                        {
                            if (indexMonsterPoint >= monsterPointsStage1.Length)
                            {
                                //characterAnimations.RunAnimation(0);
                                // StopMovement(true);
                                indexMonsterPoint = 0;
                            }
                        }
                        if (indexM == 1)
                        {
                            if (indexMonsterPoint >= monsterPStage2.Length)
                            {
                                //characterAnimations.RunAnimation(0);
                                //StopMovement(true);
                                indexMonsterPoint = 0;
                            }
                        }
                        if (indexM == 2)
                        {
                            if (indexMonsterPoint >= mPSgar3.Length)
                            {
                                characterAnimations.RunAnimation(0);
                                StopMovement(true);
                            }
                        }
                        return;
                    }

                    Transport transport = other.GetComponentInParent<Transport>();
                    if (transport.IsFullTransport())
                    {
                        indexMonsterPoint++;
                        if (indexM == 0)
                        {
                            if (indexMonsterPoint >= monsterPointsStage1.Length)
                            {
                                indexMonsterPoint = 0;
                                //characterAnimations.RunAnimation(0);
                                //StopMovement(true);
                            }
                        }
                        if (indexM == 1)
                        {
                            if (indexMonsterPoint >= monsterPStage2.Length)
                            {
                                indexMonsterPoint = 0;
                                //characterAnimations.RunAnimation(0);
                                //StopMovement(true);
                            }
                        }
                        if (indexM == 2)
                        {
                            if (indexMonsterPoint >= mPSgar3.Length)
                            {
                                indexMonsterPoint = 0;
                                //characterAnimations.RunAnimation(0);
                                //StopMovement(true);
                            }
                        }
                        return;
                    }

                    points.Clear();
                    SetPoints();
                    NewPoint();
                    UpdateMove();
                    toTransport = false;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "TransportField")
            {
                if (toTransport)
                {
                    if (inMonster)
                        return;

                    //if (other.GetComponentInParent<Transport>() == null)
                    //{
                    //    indexMonsterPoint++;

                    //    if (indexM == 0)
                    //    {
                    //        if (indexMonsterPoint >= monsterPointsStage1.Length)
                    //        {
                    //            //characterAnimations.RunAnimation(0);
                    //            // StopMovement(true);
                    //            indexMonsterPoint = 0;
                    //        }
                    //    }
                    //    if (indexM == 1)
                    //    {
                    //        if (indexMonsterPoint >= monsterPStage2.Length)
                    //        {
                    //            //characterAnimations.RunAnimation(0);
                    //            //StopMovement(true);
                    //            indexMonsterPoint = 0;
                    //        }
                    //    }
                    //    if (indexM == 2)
                    //    {
                    //        if (indexMonsterPoint >= mPSgar3.Length)
                    //        {
                    //            characterAnimations.RunAnimation(0);
                    //            StopMovement(true);
                    //        }
                    //    }
                    //    return;
                    //}

                    //Transport transport = other.GetComponentInParent<Transport>();
                    //if (transport.IsFullTransport())
                    //{
                    //    indexMonsterPoint++;
                    //    if (indexM == 0)
                    //    {
                    //        if (indexMonsterPoint >= monsterPointsStage1.Length)
                    //        {
                    //            indexMonsterPoint = 0;
                    //            //characterAnimations.RunAnimation(0);
                    //            //StopMovement(true);
                    //        }
                    //    }
                    //    if (indexM == 1)
                    //    {
                    //        if (indexMonsterPoint >= monsterPStage2.Length)
                    //        {
                    //            indexMonsterPoint = 0;
                    //            //characterAnimations.RunAnimation(0);
                    //            //StopMovement(true);
                    //        }
                    //    }
                    //    if (indexM == 2)
                    //    {
                    //        if (indexMonsterPoint >= mPSgar3.Length)
                    //        {
                    //            indexMonsterPoint = 0;
                    //            //characterAnimations.RunAnimation(0);
                    //            //StopMovement(true);
                    //        }
                    //    }
                    //    return;
                    //}

                    points.Clear();
                    SetPoints();
                    NewPoint();
                    UpdateMove();
                    toTransport = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            inMonster = false;
        }

        #endregion
    }
}
