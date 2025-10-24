using System.Collections.Generic;
using UnityEngine;

namespace BlueStellar.Cor
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] List<GameObject> currencyLevels = new List<GameObject>();

        public void SpawnLevel(int indexLvl)
        {
            GameObject newLevel = Instantiate(currencyLevels[indexLvl], transform.position, transform.rotation);
        }
    }
}
