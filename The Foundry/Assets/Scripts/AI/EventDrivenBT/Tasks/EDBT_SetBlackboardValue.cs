namespace GameFramework.AI
{
    /// <summary>
    /// Set the value of a blackboard key.
    /// </summary>
    /// <typeparam name="T">The type of the blackboard key.</typeparam>
    public class EDBTTask_SetBlackboardValue<T> : EDBTTask
    {
        private string blackboardKey;
        T value;

        public EDBTTask_SetBlackboardValue(string name, string BlackboardKey, T value) : base(name)
        {
            blackboardKey = BlackboardKey;
            this.value = value;
        }

        public override void OnStarted()
        {
            if (Blackboard.TrySetValue(blackboardKey, value))
            {
                FinishExecution(true);
            }
            else
            {
                FinishExecution(false);
            }
        }
    }
}
