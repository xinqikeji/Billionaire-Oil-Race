using UnityEngine;

namespace BlueStellar.Cor.Characters
{
    public class CharacterCanvas : MonoBehaviour
    {
        #region Variables

        [SerializeField] Transform targetCharacter;

        Transform _target;
        Transform _transform;

        #endregion

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _transform.parent = null;
            _target = targetCharacter;
        }

        private void LateUpdate()
        {
            if(_target != null)
                transform.position = _target.position;
        }
    }
}
