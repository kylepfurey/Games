using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Compares a blackboard key with a value.
    /// </summary>
    public class EDBTDecorator_BBKeyCompare<T> : EDBTDecorator_ObserverAbort
    {
        public enum CompareType
        {
            Equal = 0,
            NotEqual = 1,
            LessThan = 2,
            LessThanOrEqual = 3,
            GreaterThan = 4,
            GreaterThanOrEqual = 5,
        }

        private string blackboardKey;
        private T value;
        private CompareType compareType;

        public EDBTDecorator_BBKeyCompare(string name, string blackboardKey, T value, CompareType compareType, ObserverAbortType observerAbortType, EDBTNode childNode) : base(name, observerAbortType, childNode)
        {
            this.blackboardKey = blackboardKey;
            this.value = value;
            this.compareType = compareType;
        }

        public override bool IsConditionMet()
        {
            T blackboardValue;

            if (!Blackboard.TryGetValue(blackboardKey, out blackboardValue))
            {
                Debug.LogError($"{Name}: Cannot get the value \"{blackboardKey}\" from the blackboard.");
                return false;
            }

            if (!Blackboard.IsSet<T>(blackboardKey))
            {
                Debug.LogError($"{Name}: The key \"{blackboardKey}\" is not set.");
                return false;
            }

            switch (compareType)
            {
                case CompareType.Equal:
                    return blackboardValue.Equals(value);

                case CompareType.NotEqual:
                    return blackboardValue.Equals(value);

                case CompareType.LessThan:
                    if (blackboardValue is not IComparable)
                    {
                        return false;
                    }
                    return ((IComparable)blackboardValue).CompareTo(value) < 0;

                case CompareType.LessThanOrEqual:
                    if (blackboardValue is not IComparable)
                    {
                        return false;
                    }
                    return ((IComparable)blackboardValue).CompareTo(value) <= 0;

                case CompareType.GreaterThan:
                    if (blackboardValue is not IComparable)
                    {
                        return false;
                    }
                    return ((IComparable)blackboardValue).CompareTo(value) > 0;

                case CompareType.GreaterThanOrEqual:
                    if (blackboardValue is not IComparable)
                    {
                        return false;
                    }
                    return ((IComparable)blackboardValue).CompareTo(value) >= 0;
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
