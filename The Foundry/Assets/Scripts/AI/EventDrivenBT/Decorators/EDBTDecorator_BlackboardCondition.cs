/*
    MIT License

    Copyright (c) 2016 Nils Kübler

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

namespace GameFramework.AI
{
    /// <summary>
    /// Checks whether the blackboard key is set or not set.
    /// </summary>
    public class EDBTDecorator_BlackboardCondition<T> : EDBTDecorator_ObserverAbort
    {
        public enum BlackboardConditionType
        {
            IsSet = 0,
            IsNotSet = 1
        }

        private BlackboardConditionType type = BlackboardConditionType.IsSet;
        private string blackboardKey;

        public EDBTDecorator_BlackboardCondition(string name, string blackboardKey, BlackboardConditionType type, EDBTNode childNode) : base(name, childNode)
        {
            this.blackboardKey = blackboardKey;
            this.type = type;
        }

        public EDBTDecorator_BlackboardCondition(string name, string blackboardKey, BlackboardConditionType type, ObserverAbortType observerAbortType, EDBTNode childNode) : base(name, observerAbortType, childNode)
        {
            this.blackboardKey = blackboardKey;
            this.type = type;
        }

        public EDBTDecorator_BlackboardCondition(string name, string blackboardKey, EDBTNode childNode) : base(name, childNode)
        {
            this.blackboardKey = blackboardKey;
            type = BlackboardConditionType.IsSet;
        }

        public EDBTDecorator_BlackboardCondition(string name, string blackboardKey, ObserverAbortType observerAbortType, EDBTNode childNode) : base(name, observerAbortType, childNode)
        {
            this.blackboardKey = blackboardKey;
            type = BlackboardConditionType.IsSet;
        }

        public EDBTDecorator_BlackboardCondition(string blackboardKey, ObserverAbortType observerAbortType, EDBTNode childNode) : base($"Blackboard condition: {blackboardKey} is Set", observerAbortType, childNode)
        {
            this.blackboardKey = blackboardKey;
            type = BlackboardConditionType.IsSet;
        }

        public override bool IsConditionMet()
        {
            //UnityEngine.Debug.Log($"{Name}: Checking condition...");
            if (Blackboard.IsSet<T>(blackboardKey))
            {
                //UnityEngine.Debug.Log($"{Name}: Value \"{blackboardKey}\" is set.");
                if (type == BlackboardConditionType.IsSet)
                {
                    //UnityEngine.Debug.Log($"{Name}: Return true.");
                    return true;
                }
                else
                {
                    //UnityEngine.Debug.Log($"{Name}: Return false.");
                    return false;
                }
            }
            else
            {
                //UnityEngine.Debug.Log($"{Name}: Value \"{blackboardKey}\" is not set.");
                if (type == BlackboardConditionType.IsNotSet)
                {
                    //UnityEngine.Debug.Log($"{Name}: Return true.");
                    return true;
                }
                else
                {
                    //UnityEngine.Debug.Log($"{Name}: Return false.");
                    return false;
                }
            }
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
