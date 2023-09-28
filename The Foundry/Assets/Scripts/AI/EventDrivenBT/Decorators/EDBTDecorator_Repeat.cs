namespace GameFramework.AI
{
    /// <summary>
    /// Executes the child node a specified number of times or until the child node returns Failure.
    /// </summary>
    public class EDBTDecorator_Repeat : EDBTDecorator
    {
        private float numOfLoops;
        private float currentNumOfLoops;

        public EDBTDecorator_Repeat(string name, float numOfLoops, EDBTNode childNode) : base(name, childNode)
        {
            this.numOfLoops = numOfLoops < 1 ? 1 : numOfLoops;
        }

        public EDBTDecorator_Repeat(float numOfLoops, EDBTNode childNode) : base($"Repeat {numOfLoops} time(s)", childNode)
        {
            this.numOfLoops = numOfLoops < 1 ? 1 : numOfLoops;
        }

        public override void OnStarted()
        {
            currentNumOfLoops = 0;

            ChildNode.Start();
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            if (success)
            {
                ++currentNumOfLoops;

                if (currentNumOfLoops == numOfLoops)
                    FinishExecution(true);
                else
                    child.Start();
            }
            else
            {
                FinishExecution(false);
            }
        }
    }
}
