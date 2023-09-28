using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    public abstract class EDBTBlackboardComponent : MonoBehaviour
    {
        protected EDBTBlackboard blackboard = new EDBTBlackboard();
        public EDBTBlackboard Blackboard => blackboard;

        private bool isInitialized = false;
        public bool IsInitialized => isInitialized;

        private void Awake()
        {
            InitializeBlackboard();
        }

        private void OnDestroy()
        {
            blackboard.DestroyBlackboard();
            blackboard = null;
        }

        public void InitializeBlackboard()
        {
            OnBlackboardInitialized();
            isInitialized = true;
        }

        protected abstract void OnBlackboardInitialized();

        public static implicit operator EDBTBlackboard(EDBTBlackboardComponent blackboardComponent)
        {
            return blackboardComponent.blackboard;
        }
    }
}
