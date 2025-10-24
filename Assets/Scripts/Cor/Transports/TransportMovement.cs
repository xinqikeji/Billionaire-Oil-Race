using UnityEngine;
using DG.Tweening;

namespace BlueStellar.Cor.Transports
{
    public class TransportMovement : MonoBehaviour
    {
        #region Variables

        [Header("MovementSettings")]
        [SerializeField] Transport _transport;
        [SerializeField] Transform _target;
        [SerializeField] private float speed;
        [SerializeField] private bool canMovement;

        Vector3 movePos;

        #endregion

        private void Start()
        {
            movePos = transform.position;
            movePos.z = transform.position.z + 70f;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        public void ActiveMovement(bool isActive)
        {
            canMovement = isActive;
        }

        private void Movement()
        {
            if (!canMovement)
                return;

            speed += 0.007f;
            transform.position = Vector3.MoveTowards(transform.position, movePos, speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Stop")
            {
                _transport.StopTransport();
                ActiveMovement(false);
            }
        }
    }
}
