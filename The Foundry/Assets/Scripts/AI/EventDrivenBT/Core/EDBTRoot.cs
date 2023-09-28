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
    /// The entry point of a behavior tree. Every behavior tree must have one.
    /// DO NOT use this node as a child node.
    /// </summary>
    public class EDBTRoot : EDBTDecorator
    {
        private EDBTBlackboard blackboard;
        public override EDBTBlackboard Blackboard => blackboard;

        bool treeDestroyed = false;

        public EDBTRoot(EDBTBlackboard blackboard, EDBTNode childNode) : base("Root", childNode)
        {
            this.blackboard = blackboard;
            Rootnode = this;
        }

        public EDBTRoot(EDBTNode childNode) : this(new EDBTBlackboard(), childNode)
        {
        }

        public override void OnStarted()
        {
            // This node has already started. Don't repeat it.
            TimerManager.Instance.RemoveTimer(Start);

            if (!treeDestroyed)
            {
                ChildNode.Start();
            }
        }

        public override void OnInterrupted()
        {
            UnityEngine.Debug.Log("Stopping behavior tree...");

            if (ChildNode.CurrentState == State.Active)
            {
                ChildNode.Stop();
            }

            TimerManager.Instance.RemoveTimer(Start);
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            if (CurrentState == State.StopRequested)
            {
                FinishExecution(success);

                UnityEngine.Debug.Log("Behavior tree stopped completely");
            }

            else
            {
                FinishExecution(success);

                if (!treeDestroyed)
                {
                    // Restart the root node.
                    // 1) Why isn't Start() called directly? Stack overflow.
                    // 2) Why -1 instead of 1 for repeatedTimes? To guarantee that Start() will run again. When Start() is called, it is automatically removed from the timer manager.
                    TimerManager.Instance.AddTimer(Start, 0.0f, -1);
                }
            }
        }

        /// <summary>
        /// Destroy the behavior tree. Call this to avoid memory leaks.
        /// </summary>
        public void DestroyBehaviorTree()
        {
            Stop();
            treeDestroyed = true;

            OnBehaviorTreeDestroyed();

            ChildNode = null;
            parentNode = null;
            Rootnode = null;

            UnityEngine.Debug.Log("Behavior tree destroyed");
        }
    }
}
