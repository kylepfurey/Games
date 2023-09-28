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
    /// Execute a sequence of child nodes until all of them return Success or until the first child node returns Failure.
    /// </summary>
    public class EDBTSequence : EDBTComposite
    {
        int currentChildIndex = -1;

        public EDBTSequence(string name, params EDBTNode[] childNodes) : base(name, childNodes)
        {
        }

        public EDBTSequence(params EDBTNode[] childNodes) : base("Sequence", childNodes)
        {
        }

        public override void OnStarted()
        {
            currentChildIndex = -1;

            EvaluateChild();
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            // If the child node fails, the sequence stops and returns failure.
            if (success == false)
            {
                FinishExecution(false);
                return;
            }

            // Evaluate the next child.
            EvaluateChild();
        }

        private void EvaluateChild()
        {
            ++currentChildIndex;

            // If there are still other children to execute, execute them.
            if (currentChildIndex < ChildNodes.Length)
            {
                ChildNodes[currentChildIndex].Start();
            }

            // If there is no child left to execute, stop and return success.
            else
            {
                FinishExecution(true);
            }
        }
    }
}
