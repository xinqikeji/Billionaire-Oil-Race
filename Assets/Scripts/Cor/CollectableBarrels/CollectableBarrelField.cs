using System.Collections.Generic;
using UnityEngine;
using BlueStellar.Cor.Helpers;

namespace BlueStellar.Cor
{
    public class CollectableBarrelField : MonoBehaviour
    {
        [System.Serializable]
        public class BarrelType
        {
            public GameObject ballPrefab;
            public CharacterColorType type;
        }

        #region Variables

        [Header("BallTypes")]
        [SerializeField] List<BarrelType> ballTypes = new List<BarrelType>();

        [Space]
        [Header("FieldPlacement")]
        [SerializeField] private int length;
        [SerializeField] private int line;
        [SerializeField] private float xOrder;
        [SerializeField] private float zF;
        [SerializeField] private float xPosition;
        [SerializeField] private float zPosition;
        [SerializeField] private float form;

        [Space]
        [Header("TimerResetBall")]
        [SerializeField] private float timeToResetBall;
        [SerializeField] private float timer;

        [Space]
        [Header("NextFields")]
        [SerializeField] CollectableBarrelField collectableBarrelField;
        [SerializeField] private bool isFirstField;

        public int testSum;

        private Vector3 startPoint;
        private Vector3 position;

        private List<SpawnedBarrel> _spawnedBarrels = new List<SpawnedBarrel>();
        private List<SpawnedBarrel> _respawnBarrels = new List<SpawnedBarrel>();

        #endregion

        public List<Vector3> ListTypeBarrels(CharacterColorType colorType)
        {
            List<Vector3> barrels = new List<Vector3>();
            switch (colorType)
            {
                case CharacterColorType.Blue:
                    foreach (var i in _spawnedBarrels)
                    {
                        if (i.GetSpawnedBallType() == colorType)
                            barrels.Add(i.SpawnPosition());
                    }
                    return barrels;
                case CharacterColorType.Yellow:
                    foreach (var i in _spawnedBarrels)
                    {
                        if (i.GetSpawnedBallType() == colorType)
                            barrels.Add(i.SpawnPosition());
                    }
                    return barrels;
                case CharacterColorType.Pink:
                    foreach (var i in _spawnedBarrels)
                    {
                        if (i.GetSpawnedBallType() == colorType)
                            barrels.Add(i.SpawnPosition());
                    }
                    return barrels;
                case CharacterColorType.Green:
                    foreach (var i in _spawnedBarrels)
                    {
                        if (i.GetSpawnedBallType() == colorType)
                            barrels.Add(i.SpawnPosition());
                    }
                    return barrels;
                case CharacterColorType.Purple:
                    foreach (var i in _spawnedBarrels)
                    {
                        if (i.GetSpawnedBallType() == colorType)
                            barrels.Add(i.SpawnPosition());
                    }
                    return barrels;
                case CharacterColorType.Red:
                    foreach (var i in _spawnedBarrels)
                    {
                        if (i.GetSpawnedBallType() == colorType)
                            barrels.Add(i.SpawnPosition());
                    }
                    return barrels;
            }
            return null;
        }

        private void Start()
        { 
            SetStartPos();
            BarrelsPlacement();
        }

        private void Update()
        {
            ResetBarrels();
        }

        #region GenerateBarrels

        public void AddBarrelType(BarrelType ballType)
        {
            ballTypes.Add(ballType);
        }

        public void RemoveBarrelType(CharacterColorType type, int number)
        {
            if (_spawnedBarrels.Count == 0)
                return;

            BarrelType barrelType = new BarrelType();
            foreach (var i in ballTypes)
            {
                if (i.type == type)
                {
                    barrelType = i;
                    break;
                }
            }

            testSum += number;

            for (int i = 0; i < number; i++)
            {
                int random = Random.Range(0, _spawnedBarrels.Count);
                if (_spawnedBarrels[random].GetCollectableBall() != null)
                {
                    Destroy(_spawnedBarrels[random].GetCollectableBall().gameObject);
                    _spawnedBarrels[random].ClearSpawnedBall();
                    GenerateTypeRemovedBarrel(_spawnedBarrels[random], barrelType);
                }

                if (_spawnedBarrels[random].GetCollectableBall() == null)
                {
                    GenerateTypeRemovedBarrel(_spawnedBarrels[random], barrelType);
                }
            }
        }

        public void GenerateRemovedBarrel(SpawnedBarrel spawnedBarrel)
        {
            if (ballTypes.Count == 0)
                return;

            BarrelType ballType = ballTypes[Random.Range(0, ballTypes.Count)];
            GameObject createdBall = Instantiate(ballType.ballPrefab, spawnedBarrel.SpawnPosition(),
                ballType.ballPrefab.transform.rotation);

            createdBall.transform.parent = transform;
            createdBall.transform.position = spawnedBarrel.SpawnPosition();
            spawnedBarrel.SetNewSpawnedBall(createdBall.GetComponent<CollectableBarrel>());
            _respawnBarrels.Remove(spawnedBarrel);
        }

        public void GenerateTypeRemovedBarrel(SpawnedBarrel spawnedBall, BarrelType ballType)
        {
            if (ballTypes.Count == 0)
                return;

            GameObject createdBall = Instantiate(ballType.ballPrefab, spawnedBall.SpawnPosition(),
                ballType.ballPrefab.transform.rotation);

            createdBall.transform.parent = transform;
            createdBall.transform.position = spawnedBall.SpawnPosition();
            spawnedBall.SetNewSpawnedBall(createdBall.GetComponent<CollectableBarrel>());
            _respawnBarrels.Remove(spawnedBall);
        }

        public void RemoveCollectableBarrel(CollectableBarrel collectableBall)
        {
            foreach (var i in _spawnedBarrels)
            {
                if (collectableBall == i.GetCollectableBall())
                {
                    i.ClearSpawnedBall();
                    _respawnBarrels.Add(i);
                }
            }
        }

        public void RemoveSpawnedBarrel(CharacterColorType _characterColorType)
        {
            BarrelType barrelType = new BarrelType();

            foreach(var i in ballTypes)
            {
                if(i.type == _characterColorType)
                {
                    barrelType = i;
                    break;
                }
            }

            if (collectableBarrelField != null)
            {
                collectableBarrelField.gameObject.SetActive(true);
                collectableBarrelField.AddBarrelType(barrelType);
            }

            ballTypes.Remove(barrelType);

            for (int i = 0; i < _spawnedBarrels.Count; i++)
            {
                if (_spawnedBarrels[i].GetSpawnedBallType() == _characterColorType)
                {
                    if (_spawnedBarrels[i].GetCollectableBall() != null)
                    {
                        Destroy(_spawnedBarrels[i].GetCollectableBall().gameObject);
                        _spawnedBarrels[i].ClearSpawnedBall();
                        if(ballTypes.Count == 0)
                            _respawnBarrels.Add(_spawnedBarrels[i]);
                    }
                }
            }
        }

        private void ResetBarrels()
        {
            timer += Time.deltaTime;

            if (timer >= timeToResetBall)
            {
                
                if (_respawnBarrels.Count == 0)
                {
                    timer = 0f;
                    return;
                }

                timer = 0f;
                GenerateRemovedBarrel(_respawnBarrels[0]);
            }
        }

        #endregion

        #region Placement

        private void SetStartPos()
        {
            startPoint = transform.position;
            zPosition = transform.position.z;
            xPosition = transform.position.x;
        }

        private void BarrelsPlacement()
        {
            for (int i = 0; i < length; i++)
            {
                xOrder += form;
                
                if (i % line == 0)
                {
                    zPosition -= zF;
                    xOrder = 0;
                    position = new Vector3(xPosition, startPoint.y, zPosition);
                }
                else
                {
                    position = new Vector3(xPosition + xOrder, startPoint.y, zPosition);
                }

                if (isFirstField)
                {
                    BarrelType ballType = ballTypes[Random.Range(0, ballTypes.Count)];

                    GameObject newCollectableBarrel = Instantiate(ballType.ballPrefab,
                     position, ballType.ballPrefab.transform.rotation);

                    newCollectableBarrel.transform.parent = transform;

                    SpawnedBarrel spawnedBarrel = new SpawnedBarrel();
                    spawnedBarrel.SetSpawnedBallPosition(position);
                    spawnedBarrel.SetSpawnedBall(newCollectableBarrel.GetComponent<CollectableBarrel>());
                    _spawnedBarrels.Add(spawnedBarrel);
                }
                if (!isFirstField)
                {
                    SpawnedBarrel spawnedBarrel = new SpawnedBarrel();
                    spawnedBarrel.SetSpawnedBallPosition(position);
                    _spawnedBarrels.Add(spawnedBarrel);
                }
            }
        }

        #endregion
    }
}
