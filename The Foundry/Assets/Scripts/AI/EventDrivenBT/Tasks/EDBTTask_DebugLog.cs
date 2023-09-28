namespace GameFramework.AI
{
    /// <summary>
    /// Print a debug log in the debug window.
    /// </summary>
    public class EDBTTask_DebugLog : EDBTTask
    {
        private string message;
        private bool success;

        public EDBTTask_DebugLog(string name, string message, bool success = true) : base(name)
        {
            this.message = message;
            this.success = success;
        }

        public EDBTTask_DebugLog(string message, bool success = true) : base($"Debug Log: {message}")
        {
            this.message = message;
            this.success = success;
        }

        public override void OnStarted()
        {
            UnityEngine.Debug.Log(message);
            FinishExecution(success);
        }
    }
}
