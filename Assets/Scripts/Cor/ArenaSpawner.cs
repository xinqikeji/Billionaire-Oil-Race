using System.Collections.Generic;
using UnityEngine;
using BlueStellar.SO;

namespace BlueStellar.Cor
{
    public class ArenaSpawner : MonoBehaviour
    {
        #region Singelton

        public static ArenaSpawner Instance;

        private void Awake()
        {
            Instance = this;
            SpawnNewLevel();
        }

        #endregion

        #region Variables

        [Header("Roads")]
        [SerializeField] List<GameObject> roads = new List<GameObject>();
        [SerializeField] Transform[] pointsRoads;

        [Header("Transports")]
        [SerializeField] List<GameObject> transports = new List<GameObject>();
        [SerializeField] Transform[] pointsTransports;

        [Header("LevelsArena")]
        [SerializeField] List<SO_LevelsArena> levelsArena = new List<SO_LevelsArena>();
        [SerializeField] private int indexLevel;

        #endregion

        private void SpawnNewLevel()
        {
            Load();
            transports = levelsArena[indexLevel].prefabsTransports;
            roads = levelsArena[indexLevel].prefabsRoads;
            for (int i = 0; i < roads.Count; i++)
            {
                GameObject newRoad = Instantiate(roads[i], pointsRoads[i].position, pointsRoads[i].rotation);
                newRoad.transform.parent = pointsRoads[i];
            }

            for (int i = 0; i < transports.Count; i++)
            {
                GameObject newTransport = Instantiate(transports[i], pointsTransports[i].position, pointsTransports[i].rotation);
                newTransport.transform.parent = pointsTransports[i];
            }
        }

        public void SetNext()
        {
            indexLevel++;
            if(indexLevel >= levelsArena.Count)
            {
                indexLevel = 0;
            }
            Save();
        }

        #region Load&Save

        private void Load()
        {
            indexLevel = ES3.Load("indexLevel", indexLevel);
        }

        private void Save()
        {
            ES3.Save("indexLevel", indexLevel);
        }

        #endregion
    }
}
