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
    /// Observer aborts are decorators that are capable of aborting a subtree immediately when the condition changes.
    /// </summary>
    public abstract class EDBTDecorator_ObserverAbort : EDBTDecorator
    {
        public enum ObserverAbortType
        {
            /// <summary>
            /// Never abort the current task even if the condition is no longer met.
            /// </summary>
            None = 0,

            /// <summary>
            /// Abort the subtree below this observer abort when the condition is not met.
            /// </summary>
            Self = 1,

            /// <summary>
            /// Abort the nodes in lower priority (i.e. nodes that are after this node if we traverse the behavior tree in pre-order) when the condition is met.
            /// </summary>
            LowerPriority = 2,

            /// <summary>
            /// Combine both Self and LowerPriority.
            /// </summary>
            Both = 3
        }

        private ObserverAbortType type = ObserverAbortType.None;
        public ObserverAbortType Type => type;

        private bool isObserving = false;
        public bool IsObserving => isObserving;

        public EDBTDecorator_ObserverAbort(string name, ObserverAbortType observerAbortType, EDBTNode childNode) : base(name, childNode)
        {
            type = observerAbortType;
        }

        public EDBTDecorator_ObserverAbort(string name, EDBTNode childNode) : base(name, childNode)
        {
        }

        public override void OnStarted()
        {
            // Start observing if it is not observing.
            if (type != ObserverAbortType.None && !isObserving)
            {
                isObserving = true;
                StartObserving();
            }

            if (!IsConditionMet())
            {
                FinishExecution(false);
            }
            else
            {
                ChildNode.Start();
            }
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            // Stop observing the child node if ONLY the child is observed.
            if (type == ObserverAbortType.Self && isObserving)
            {
                isObserving = false;
                StopObserving();
            }

            FinishExecution(success);
        }

        public abstract void StartObserving();

        public abstract void StopObserving();

        /// <summary>
        /// Called when the condition is changed.
        /// </summary>
        public virtual void EvaluateCondition()
        {
            // Stop the execution of the child node if:
            // + the condition is no longer valid, and
            // + the observer abort observes its child node.
            if (CurrentState == State.Active && !IsConditionMet())
            {
                if (type == ObserverAbortType.Self || type == ObserverAbortType.Both)
                {
                    Stop();
                }
            }

            // Stop the execution of the lower-priority node if:
            // + the condition is valid,
            // + the observer abort observes the lower-priority node.
            else if (CurrentState == State.Inactive && IsConditionMet())
            {
                if (type == ObserverAbortType.LowerPriority || type == ObserverAbortType.Both)
                {
                    EDBTNode currentNode = this;
                    EDBTNode parentNode = currentNode.ParentNode;

                    while (parentNode != null && parentNode is not EDBTComposite)
                    {
                        currentNode = parentNode;
                        parentNode = parentNode.ParentNode;
                    }

                    if (parentNode is EDBTComposite)
                    {
                        ((EDBTComposite)parentNode).StopChildInLowerPriority(currentNode.ChildIndex);
                    }
                }
            }
        }

        public abstract bool IsConditionMet();

        public override void OnBehaviorTreeDestroyed()
        {
            StopObserving();
            base.OnBehaviorTreeDestroyed();
        }
    }
}
