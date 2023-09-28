using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// A blackboard is the memory of a behavior tree. It contains data that the behavior tree uses for executing some tasks.
    /// </summary>
    public class EDBTBlackboard
    {
        private abstract class BlackboardKey
        {
            /// <summary>
            /// The name of the key.
            /// </summary>
            private string name;
            public string Name => name;

            /// <summary>
            /// Is the value of the key set? Always returns false when 1) the value is not set when the key is added to the blackboard, or 2) the value of the key is NULL.
            /// </summary>
            private bool isSet = false;
            public bool IsSet
            {
                get => isSet;
                set => isSet = value;
            }

            public BlackboardKey(string name)
            {
                this.name = name;
            }
        }

        private class BlackboardKeyConcrete<T> : BlackboardKey
        {
            T value;
            public T Value
            {
                get => value;
                set => this.value = value;
            }

            public BlackboardKeyConcrete(string name) : base(name)
            {
            }

            public BlackboardKeyConcrete(string name, T value) : base(name)
            {
                this.value = value;
            }
        }

        /// <summary>
        /// All keys in the blackboard.
        /// </summary>
        private Dictionary<string, BlackboardKey> blackboardKeys = new Dictionary<string, BlackboardKey>();

        /// <summary>
        /// Observer abort decorators that observe this blackboard. When a key's value is updated, the blackboard notifies the observe aborts that observe the key.
        /// </summary>
        Dictionary<string, HashSet<EDBTDecorator_ObserverAbort>> observerAborts = new Dictionary<string, HashSet<EDBTDecorator_ObserverAbort>>();

        /// <summary>
        /// Does this blackboard contain a key?
        /// </summary>
        public bool ContainsKey(string key)
        {
            return blackboardKeys.ContainsKey(key);
        }

        /// <summary>
        /// Adds a key to the blackboard. Returns false if a key with the same name already exists in the blackboard.
        /// </summary>
        public bool AddKey<T>(string key)
        {
            BlackboardKeyConcrete<T> newBlackboardKey = new BlackboardKeyConcrete<T>(key, default);

            bool canAddKey = blackboardKeys.TryAdd(key, newBlackboardKey);

            if (canAddKey)
            {
                observerAborts[key] = new HashSet<EDBTDecorator_ObserverAbort>();
            }

            return canAddKey;
        }

        /// <summary>
        /// Adds a key with a specified value to the blackboard. Returns false if a key with the same name already exists in the blackboard.
        /// </summary>
        public bool AddKey<T>(string key, T value)
        {
            BlackboardKeyConcrete<T> newBlackboardKey = new BlackboardKeyConcrete<T>(key, value);
            newBlackboardKey.IsSet = true;

            bool canAddKey = blackboardKeys.TryAdd(key, newBlackboardKey);

            if (canAddKey)
            {
                observerAborts[key] = new HashSet<EDBTDecorator_ObserverAbort>();
            }

            return canAddKey;
        }

        /// <summary>
        /// Is the value of a key set?
        /// </summary>
        public bool IsSet<T>(string key)
        {
            BlackboardKey blackboardKey;
            if (!blackboardKeys.TryGetValue(key, out blackboardKey))
            {
                Debug.LogError($"Cannot get the value of \"{blackboardKey}\". The key does not exist.");
                return false;
            }

            if (blackboardKey is not BlackboardKeyConcrete<T>)
            {
                Debug.LogError($"Cannot get the value of \"{blackboardKey}\". The key does not match the type.");
                return false;
            }

            return blackboardKey.IsSet && ((BlackboardKeyConcrete<T>)blackboardKey).Value != null;
        }

        /// <summary>
        /// Remove a key from the blackboard. Returns true if the key had been in the blackboard before it was removed.
        /// </summary>
        public bool RemoveKey(string key)
        {
            // Remove the observer aborts that observe this key.
            observerAborts.Remove(key);

            return blackboardKeys.Remove(key);
        }

        /// <summary>
        /// Returns the value of a key.
        /// </summary>
        public T GetValue<T>(string key)
        {
            return ((BlackboardKeyConcrete<T>)blackboardKeys[key]).Value;
        }

        /// <summary>
        /// Safe way to get the value of a key.
        /// </summary>
        public bool TryGetValue<T>(string key, out T value)
        {
            BlackboardKey blackboardKey;

            if (!blackboardKeys.TryGetValue(key, out blackboardKey))
            {
                value = default;
                return false;
            }

            if (blackboardKey is BlackboardKeyConcrete<T>)
            {
                value = GetValue<T>(key);
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Sets the value of a key.
        /// </summary>
        public void SetValue<T>(string key, T value)
        {
            ((BlackboardKeyConcrete<T>)blackboardKeys[key]).Value = value;
            ((BlackboardKeyConcrete<T>)blackboardKeys[key]).IsSet = true;

            foreach (var observerAbort in observerAborts[key].ToList())
            {
                observerAbort.EvaluateCondition();
            }
        }

        /// <summary>
        /// Safe way to set the value of a key.
        /// </summary>
        public bool TrySetValue<T>(string key, T value)
        {
            if (!ContainsKey(key))
                return false;

            if (blackboardKeys[key] is not BlackboardKeyConcrete<T>)
                return false;

            SetValue(key, value);
            return true;
        }

        /// <summary>
        /// Returns the position from a blackboard key.
        /// </summary>
        public bool TryGetPosition(string key, out Vector3 position)
        {
            // Case 1: The key stores a vector.
            Vector3 vector;
            if (TryGetValue(key, out vector))
            {
                position = vector;
                return true;
            }

            // Case 2: The key stores a transform.
            Transform transform;
            if (TryGetValue(key, out transform) && transform != null)
            {
                position = transform.position;
                return true;
            }

            // Case 3: The key stores a game object.
            GameObject gameObject;
            if (TryGetValue(key, out gameObject) && gameObject != null)
            {
                position = gameObject.transform.position;
                return true;
            }

            // Case 4: The key stores a component.
            Component component;
            if (TryGetValue(key, out component) && component != null)
            {
                position = component.transform.position;
                return true;
            }

            position = default;
            return false;
        }

        /// <summary>
        /// Returns the transform from a blackboard key.
        /// </summary>
        public bool TryGetTransform(string key, out Transform transform)
        {
            // Case 1: The key stores a transform.
            if (TryGetValue(key, out transform))
            {
                return true;
            }

            // Case 2: The key stores a game object.
            GameObject gameObject;
            if (TryGetValue(key, out gameObject) && gameObject != null)
            {
                transform = gameObject.transform;
                return true;
            }

            // Case 3: The key stores a component.
            Component component;
            if (TryGetValue(key, out component) && component != null)
            {
                transform = component.transform;
                return true;
            }

            transform = default;
            return false;
        }

        /// <summary>
        /// Makes an observer abort observe a blackboard key.
        /// </summary>
        public void AddObserverAbort(string blackboardKey, EDBTDecorator_ObserverAbort observerAbort)
        {
            if (observerAbort == null)
                return;

            if (observerAbort.Type == EDBTDecorator_ObserverAbort.ObserverAbortType.None)
                return;

            observerAborts[blackboardKey].Add(observerAbort);

            //Debug.Log($"Added observer for key \"{blackboardKey}\".");
        }

        /// <summary>
        /// Removes an observer abort from this blackboard.
        /// </summary>
        public void RemoveObserverAbort(string blackboardKey, EDBTDecorator_ObserverAbort observerAbort)
        {
            if (observerAbort == null)
                return;

            observerAborts[blackboardKey].Remove(observerAbort);

            //Debug.Log($"Removed observer from key \"{blackboardKey}\".");
        }

        /// <summary>
        /// Destroys the blackboard. Particularly, removes all observer aborts and removes all blackboard keys in the blackboard.
        /// </summary>
        public void DestroyBlackboard()
        {
            blackboardKeys = null;
            observerAborts = null;
        }
    }
}
