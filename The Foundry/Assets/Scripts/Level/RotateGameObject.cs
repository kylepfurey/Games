using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class RotateGameObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 eulers;

        private void Update()
        {
            transform.Rotate(eulers * Time.deltaTime);
        }
    }
}
