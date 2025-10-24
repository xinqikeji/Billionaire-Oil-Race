using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlueStellar.Cor.Transports;
using BlueStellar.Cor.Characters;

namespace BlueStellar.Cor
{
    public class StackBarrels : MonoBehaviour
    {
        [SerializeField] List<CollectableBarrel> currencyBalls = new List<CollectableBarrel>();
        [SerializeField] List<Transform> currencyStackPoints = new List<Transform>();
        [SerializeField] ParticleSystem effect;
        [SerializeField] private float offsetStuffY;

        Character character;

        public int AmmountBalls()
        {
            return currencyBalls.Count;
        }

        private void Start()
        {
            character = GetComponent<Character>();
        }

        private void Update()
        {
            if (currencyBalls.Count > 0)
            {
                var startPos = currencyStackPoints[0].position;
                float index = 0;
                foreach (var item in currencyBalls)
                {
                    Vector3 newPosStuff = startPos + Vector3.up * offsetStuffY * currencyBalls.IndexOf(item);
                    item.transform.position = Vector3.Lerp(item.transform.position, newPosStuff, Time.deltaTime / index * 115);
                    item.transform.rotation =currencyStackPoints[0].transform.rotation;
                    index++;
                }
            }
        }

        public void AddCollectableBall(CollectableBarrel _ball)
        {
            foreach (var i in currencyBalls)
            {
                if (_ball == i)
                    return;
            }

            if (currencyBalls.Count > 0)
            {
                currencyStackPoints[1].transform.position = currencyBalls[currencyBalls.Count - 1].transform.position;
            }

            currencyBalls.Add(_ball);

            if (character.IsPlayer())
            {
                if(currencyBalls.Count < 18)
                {
                    CameraController.Instance.PlayerCamActive(true);
                    CameraController.Instance.HightCamActive(false);
                    CameraController.Instance.VeryHightCamActive(false);
                }
                if (currencyBalls.Count > 18 && currencyBalls.Count < 29)
                {
                    CameraController.Instance.PlayerCamActive(false);
                    CameraController.Instance.HightCamActive(true);
                    CameraController.Instance.VeryHightCamActive(false);
                }
                if(currencyBalls.Count > 29)
                {
                    CameraController.Instance.PlayerCamActive(false);
                    CameraController.Instance.HightCamActive(false);
                    CameraController.Instance.VeryHightCamActive(true);
                }
            }

            int indexBall = currencyBalls.IndexOf(_ball);
            _ball.transform.parent = currencyStackPoints[0];
            _ball.BallInStack();
            effect.Play();
        }

        public void UnstackCollectableBarrel(Transport transport)
        {
            for (int i = currencyBalls.Count - 1; i >= 0; i--)
            {
                currencyBalls[i].transform.SetParent(null);
                currencyBalls[i].transform.DOMove(transform.position + Vector3.up * 4f, 0.25f);
                currencyBalls[i].transform.DOJump(transport.transform.position, 4, 1, 0.3f).SetDelay(0.25f);
                currencyBalls[i].BarrelToTransport(transport);
                currencyBalls.Remove(currencyBalls[i]);
                if (currencyBalls.Count > 0)
                {
                    currencyStackPoints[1].transform.position = currencyBalls[currencyBalls.Count - 1].transform.position;
                }
                if (character.IsPlayer())
                {
                    if (currencyBalls.Count < 20)
                    {
                        CameraController.Instance.PlayerCamActive(true);
                        CameraController.Instance.HightCamActive(false);
                    }
                    if (currencyBalls.Count > 20)
                    {
                        CameraController.Instance.PlayerCamActive(false);
                        CameraController.Instance.HightCamActive(true);
                    }
                }
                return;
            }
        }

        public void DestroyedStack()
        {
            foreach (var i in currencyBalls)
            {
                i.BallNeutral();
            }

            currencyBalls.Clear();
        }

        public void ClearStack()
        {
            foreach(var i in currencyBalls)
            {
                Destroy(i.gameObject);
            }

            currencyBalls.Clear();
        }

        public void RemoveColleactbleBall()
        {
            if(currencyBalls.Count > 0)
            {
                Destroy(currencyBalls[currencyBalls.Count - 1].gameObject, 0.1f);
                currencyBalls.RemoveAt(currencyBalls.Count - 1);
            }
        }
    }
}
