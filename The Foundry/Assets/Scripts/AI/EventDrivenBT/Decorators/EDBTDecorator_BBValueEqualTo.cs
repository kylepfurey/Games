using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Checks if the value of a blackboard key is equal to a specified value.
    /// </summary>
    public class EDBTDecorator_BBValueEqualTo<T> : EDBTDecorator_ObserverAbort
    {
        private string blackboardKey;
        private T value;

        public EDBTDecorator_BBValueEqualTo(string name, string blackboardKey, T value, ObserverAbortType observerAbortType, EDBTNode childNode) : base(name, observerAbortType, childNode)
        {
            this.blackboardKey = blackboardKey;
            this.value = value;
        }

        public EDBTDecorator_BBValueEqualTo(string blackboardKey, T value, ObserverAbortType observerAbortType, EDBTNode childNode) : base($"{blackboardKey} is equal to value", observerAbortType, childNode)
        {
            this.blackboardKey = blackboardKey;
            this.value = value;
        }

        public override bool IsConditionMet()
        {
            T blackboardValue;

            if (!Blackboard.TryGetValue(blackboardKey, out blackboardValue))
            {
                Debug.LogError($"{Name}: Cannot get the value \"{blackboardKey}\" from the blackboard.");
                return false;
            }

            if (!blackboardValue.Equals(value))
            {
                return false;
            }

            return true;
        }

        public override void StartObserving()
        {
            Blackboard.AddObserverAbort(blackboardKey, this);
        }

        public override void StopObserving()
        {
            Blackboard.RemoveObserverAbort(blackboardKey, this);
        }
    }
}
