using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Match
{

    /// <summary>
    /// LEVEL SETTINGS - All of the information for a given level
    /// </summary>
    public class LevelSettings : MonoBehaviour
    {
        #region FIELDS
        [SerializeField]
        private Transform[] spawns;
        [SerializeField]
        private Transform start;
        [SerializeField]
        private Transform end;
        [SerializeField]
        private Transform boundsCenter;
        [SerializeField]
        private GameObject[] boundsObjects;
        [SerializeField]
        private float boundsMoveRate;
        [SerializeField]
        private float boundsEndDistThreshold;
        #endregion

        #region GETTERS AND SETTERS
        public Transform[] Spawns
        {
            get { return spawns; }
            set { spawns = value; }
        }

        public Transform Start
        {
            get { return start; }
            set { start = value; }
        }

        public Transform End {
            get { return end; }
            set { end = value; }
        }

        public Transform BoundsCenter
        {
            get { return boundsCenter; }
            set { boundsCenter = value; }
        }

        public GameObject[] BoundsObjects {
            get { return boundsObjects; }
            set { boundsObjects = value; }
        }

        public float BoundsMoveRate
        {
            get { return boundsMoveRate; }
            set { boundsMoveRate = value; }
        }

        public float BoundsEndDistThreshold
        {
            get { return boundsEndDistThreshold; }
            set { boundsEndDistThreshold = value; }
        }

        #endregion
    }
}
