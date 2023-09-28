using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class TestLockingFrameRate : MonoBehaviour
    {
        public int targetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
            Destroy(gameObject);
        }
    }
}
