using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    public class TestEventDrivenBT : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            /*
            var rootNode = new EDBTRoot
            (
                new EDBTSequence
                ( "Print and Wait",
                    new EDBTTask_DebugLog("Print hello world", "Hello World!"),
                    new EDBTTask_Wait("Wait for seconds", 3.0f)
                )
            );
            */


            /*
            var rootNode = new EDBTRoot
            (
                new EDBTSequence
                (
                    "Print and Wait",
                    new EDBTDecorator_Repeat(3, new EDBTTask_DebugLog("Print hello world", "Hello World!")),
                    new EDBTTask_Wait("Wait for seconds", 3.0f)
                )
            );
            */


            var rootNode = new EDBTRoot
            (
                new EDBTSelector
                (
                    "Choose which debug log to print",
                    new EDBTTask_DebugLog("Print A", "Hello A!"),
                    new EDBTTask_DebugLog("Print B", "Hello B!")
                )
            );

            rootNode.Start();
        }
    }
}
