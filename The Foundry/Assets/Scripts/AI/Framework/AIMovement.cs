using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    public abstract class AIMovement : MonoBehaviour
    {
        public abstract void SetMoveDestination(Vector3 destination);
        public abstract void SetMoveDestination(Transform transform);
        public abstract Transform SetLookTarget(Transform target);
    }
}
