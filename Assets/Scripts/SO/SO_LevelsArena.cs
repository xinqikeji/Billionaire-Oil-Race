using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BlueStellar.SO
{
    [CreateAssetMenu(fileName = "EnemyArmySettings", menuName = "BlueStellar/SO/SO_EnemyArmySettings")]
    public class SO_LevelsArena : SerializedScriptableObject
    {
        [Header("Transports")]
        public List<GameObject> prefabsTransports = new List<GameObject>();

        [Header("Roads")]
        public List<GameObject> prefabsRoads = new List<GameObject>();
    }
}
