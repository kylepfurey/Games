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
    /// Decorators are nodes that alternate the behavior of a node.
    /// </summary>
    public class EDBTDecorator : EDBTNode
    {
        private EDBTNode childNode;
        public EDBTNode ChildNode
        {
            get => childNode;
            protected set => childNode = value;
        }

        public override EDBTNode Rootnode
        {
            get => base.Rootnode;
            set
            {
                base.Rootnode = value;

                if (value != null)
                {
                    childNode.Rootnode = value;
                }
            }
        }

        public EDBTDecorator(string name, EDBTNode childNode) : base(name)
        {
            this.childNode = childNode;
            childNode.ParentNode = this;
            childNode.ChildIndex = 0;
        }

        public override void OnStarted()
        {
            // By default, start the child.
            childNode.Start();
        }

        public override void OnInterrupted()
        {
            // By default, stop the child in case it is still active.
            childNode.Stop();
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            // By default, return the result of the child node.
            FinishExecution(success);
        }

        public override void OnBehaviorTreeDestroyed()
        {
            childNode.OnBehaviorTreeDestroyed();
            childNode = null;

            base.OnBehaviorTreeDestroyed();
        }
    }
}
