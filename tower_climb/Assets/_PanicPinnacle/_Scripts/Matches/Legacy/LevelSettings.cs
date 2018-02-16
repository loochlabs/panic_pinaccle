using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Matches.Legacy
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
        private float boundsMoveRate = 0.6f;
        [SerializeField]
        private float boundsEndDistThreshold = 0.5f;
        [SerializeField]
        private float boundEndScaleRate = 0.5f;

        //direction of movement during round.playing
        private Vector3 boundsDirection = Vector3.zero;
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

        public float BoundsEndScaleRate
        {
            get { return boundEndScaleRate; }
            set { boundEndScaleRate = value; }
        }

        /// <summary>
        /// Direction of movement from start to end of level.
        /// </summary>
        //@TODO find a way to only calculate this at Start()
        //      maybe during RoundManager.Start()
        public Vector3 BoundsDirection
        {
            get
            {
                boundsDirection.x = End.position.x - Start.position.x;
                boundsDirection.y = End.position.y - Start.position.y;
                boundsDirection.z = End.position.z - Start.position.z;
                return boundsDirection.normalized;
            }
        }
        #endregion
    }
}
