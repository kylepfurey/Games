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
    /// The base class for all nodes in a behavior tree.
    /// </summary>
    public abstract class EDBTNode
    {
        /// <summary>
        /// The current state of the node.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// The node is not running.
            /// </summary>
            Inactive = 0,

            /// <summary>
            /// The node is running.
            /// </summary>
            Active = 1,

            /// <summary>
            /// This node is requested to stop (by the user, by the parent node, etc.).
            /// </summary>
            StopRequested = 2
        }
        private State currentState;
        public State CurrentState => currentState;

        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference to the parent of the node.
        /// </summary>
        protected EDBTNode parentNode;
        public EDBTNode ParentNode
        {
            get => parentNode;
            set => parentNode = value;
        }

        /// <summary>
        /// The child index for this node. Always -1 for the root node.
        /// </summary>
        protected int childIndex = -1;
        public int ChildIndex
        {
            get => childIndex;
            set => childIndex = value < -1 ? -1 : value;
        }

        /// <summary>
        /// Reference to the root node of the behavior tree.
        /// </summary>
        private EDBTNode rootNode;
        public virtual EDBTNode Rootnode
        {
            get => rootNode;
            set => rootNode = value;
        }

        /// <summary>
        /// Reference to the blackboard of the behavior tree.
        /// </summary>
        public virtual EDBTBlackboard Blackboard => rootNode.Blackboard;

        // Constructor
        public EDBTNode(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Starts executing the node.
        /// </summary>
        public void Start()
        {
            // Do not start the node if it is already active.
            if (currentState != State.Inactive)
                return;

            currentState = State.Active;
            OnStarted();
        }

        /// <summary>
        /// Requests the node to stop.
        /// </summary>
        public void Stop()
        {
            // Do not stop the node if it is not active.
            if (currentState != State.Active)
                return;

            currentState = State.StopRequested;
            OnInterrupted();
        }

        /// <summary>
        /// Executed when the node becomes active.
        /// </summary>
        public virtual void OnStarted() {}

        /// <summary>
        /// Executed when the node's state is switched to State.StopRequested.
        /// </summary>
        public virtual void OnInterrupted() {}

        /// <summary>
        /// Must be called to completely stop the node. Otherwise, the behavior tree can be stuck to this node.
        /// </summary>
        /// <param name="success"></param>
        public void FinishExecution(bool success)
        {
            currentState = State.Inactive;

            // Notify the parent node that this node has finished execution.
            if (parentNode != null)
                parentNode.OnChildFinishExecution(this, success);
        }

        /// <summary>
        /// Executed when a child finishes execution.
        /// </summary>
        /// <param name="child"> The child of the node.</param>
        /// <param name="success"> True if the child returns Success, false if the child returns Failure.</param>
        public virtual void OnChildFinishExecution(EDBTNode child, bool success)
        {
        }

        /// <summary>
        /// Invoked when the behavior tree is destroyed.
        /// </summary>
        public virtual void OnBehaviorTreeDestroyed()
        {
            parentNode = null;
            rootNode = null;
        }
    }
}