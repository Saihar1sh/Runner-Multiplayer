using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arixen.ScriptSmith
{
    public class PoolableObject : MonoBehaviour
    {
        public bool isInUse;
        public bool InitialInit { get; private set; }

        public void OnDisable()
        {
            isInUse = false;
        }

        public void Init()
        {
            InitialInit = true;
        }
    }
}