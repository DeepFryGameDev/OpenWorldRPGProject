using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    [System.Serializable]
    public class BasePathPoint
    {
        public GameObject pathPoint;
        public float minSecondsToWait = 1, maxSecondsToWait = 3;
    }
}
