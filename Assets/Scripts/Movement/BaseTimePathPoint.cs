using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    [System.Serializable]
    public class BaseTimePathPoint
    {
        public int Time;
        public BasePathPoint pathPoint;
        public string runBeforeDeparture;
        public string runUponArrival;
    }
}