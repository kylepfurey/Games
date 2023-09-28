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
    /// Composites are nodes that contain one or multiple children.
    /// </summary>
    public class EDBTComposite : EDBTNode
    {
        private EDBTNode[] childNodes;
        public EDBTNode[] ChildNodes => childNodes;

        public override EDBTNode Rootnode
        {
            get => base.Rootnode;
            set
            {
                base.Rootnode = value;

                if (value != null)
                {
                    for (int i = 0; i < childNodes.Length; ++i)
                        childNodes[i].Rootnode = base.Rootnode;
                }
            }
        }

        public EDBTComposite(string name, params EDBTNode[] childNodes) : base(name)
        {
            this.childNodes = childNodes;

            for (int i = 0; i < childNodes.Length; ++i)
            {
                childNodes[i].ParentNode = this;
                childNodes[i].ChildIndex = i;
            }
        }

        public override void OnInterrupted()
        {
            // Stop the child node that is still active.
            foreach (var child in childNodes)
            {
                if (child.CurrentState == State.Active)
                {
                    child.Stop();
                }
            }

            FinishExecution(false);
        }

        /// <summary>
        /// Stops the execution of the node in lower priority.
        /// </summary>
        /// <param name="abortIndex"> The index of the child that raises observer abort.</param>
        public void StopChildInLowerPriority(int abortIndex)
        {
            for (int i = abortIndex + 1; i < childNodes.Length; ++i)
            {
                if (childNodes[i].CurrentState == State.Active)
                {
                    childNodes[i].Stop();
                    break;
                }
            }
        }

        public override void OnBehaviorTreeDestroyed()
        {
            foreach (var child in childNodes)
            {
                child.OnBehaviorTreeDestroyed();
            }
            childNodes = null;

            base.OnBehaviorTreeDestroyed();
        }
    }
}
