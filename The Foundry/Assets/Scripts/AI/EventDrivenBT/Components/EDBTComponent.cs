using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    public abstract class EDBTComponent : MonoBehaviour
    {
        [SerializeField]
        protected EDBTBlackboardComponent blackboardComponent;
        public EDBTBlackboard Blackboard => blackboardComponent;

        protected EDBTRoot root;

        private bool isTreeInitialized = false;
        private bool shouldRestartBehaviorTree = false;

        protected virtual void OnValidate()
        {
            if (!blackboardComponent)
            {
                blackboardComponent = GetComponent<EDBTBlackboardComponent>();
            }
        }

        private void OnDisable()
        {
            PauseBehaviorTree();
        }

        private void OnEnable()
        {
            if (isTreeInitialized && shouldRestartBehaviorTree)
            {
                StartBehaviorTree();
            }
        }

        private void Awake()
        {
            if (!blackboardComponent)
            {
                blackboardComponent = GetComponent<EDBTBlackboardComponent>();
            }

            InitializeBehaviorTree();
        }

        private void Start()
        {
            StartBehaviorTree();
        }

        private void OnDestroy()
        {
            root.DestroyBehaviorTree();
        }

        public void InitializeBehaviorTree()
        {
            OnBehaviorTreeInitialized();
            isTreeInitialized = true;
        }

        protected abstract void OnBehaviorTreeInitialized();

        public void StartBehaviorTree()
        {
            if (!isTreeInitialized)
            {
                Debug.LogError("Cannot start the behavior tree: The tree has not been initialized.");
                return;
            }

            shouldRestartBehaviorTree = true;
            root.Start();
        }

        public void PauseBehaviorTree()
        {
            if (!isTreeInitialized)
            {
                Debug.LogError("Cannot pause the behavior tree: The tree has not been initialized.");
                return;
            }

            shouldRestartBehaviorTree = true;
            root.Stop();
        }

        public void StopBehaviorTree()
        {
            if (!isTreeInitialized)
            {
                Debug.LogError("Cannot stop the behavior tree: The tree has not been initialized.");
                return;
            }

            shouldRestartBehaviorTree = false;
            root.Stop();
        }
    }
}
