using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueStellar.Cor.Helpers;

namespace BlueStellar.Cor
{
    public class GatesSpawner : MonoBehaviour
    {
        #region Variables

        [SerializeField] GameObject prefabGates;
        [SerializeField] List<Transform> pointsSpawn = new List<Transform>();
        [SerializeField] List<GateType> gatesTypes = new List<GateType>();
        [SerializeField] private int minGates;
        [SerializeField] private int maxGates;
        [SerializeField] private int ammountGates;

        List<Gate> currencyGates = new List<Gate>();

        #endregion

        private void Start()
        {
            LevelController.Instance.OnLevelStart.AddListener(SpawnGate);
        }

        public void RemoveGate(Gate gate)
        {
            currencyGates.Remove(gate);
            CheckGates();
        }

        private void SpawnGate()
        {
            if (currencyGates.Count > 0)
                return;

            ammountGates = Random.Range(minGates, maxGates);

            List<Transform> points = new List<Transform>();
            points.AddRange(pointsSpawn.ToArray());

            for (int i = 0; i < ammountGates; i++)
            {
                int randomPoint = Random.Range(0, points.Count);
                int randomType = Random.Range(0, gatesTypes.Count);

                GameObject newGate = Instantiate(prefabGates, points[randomPoint].position, points[randomPoint].rotation);
                points.Remove(points[randomPoint]);

                Gate gates = newGate.GetComponent<Gate>();
                gates.SetGatesSettings(this, gatesTypes[randomType]);
                currencyGates.Add(gates);
            }
        }

        private void CheckGates()
        {
            if (currencyGates.Count > 0)
                return;

            StopAllCoroutines();
            StartCoroutine(IE_GenerateNewGates());
        }

        private IEnumerator IE_GenerateNewGates()
        {
            yield return new WaitForSeconds(8f);

            SpawnGate();
        }
    }
}
