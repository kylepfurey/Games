/*
    MIT License

    Copyright (c) 2016 Nils K�bler

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
    /// Always returns Success regardless of the child's result.
    /// </summary>
    public class EDBTDecorator_ForceSuccess : EDBTDecorator
    {
        public EDBTDecorator_ForceSuccess(string name, EDBTNode childNode) : base(name, childNode)
        {
        }

        public EDBTDecorator_ForceSuccess(EDBTNode childNode) : base("Force Success", childNode)
        {
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            FinishExecution(true);
        }
    }
}
