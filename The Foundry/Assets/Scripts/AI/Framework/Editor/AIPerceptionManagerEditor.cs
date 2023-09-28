using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.AI
{
    [CustomEditor(typeof(AIPerceptionManager))]
    public class AIPerceptionManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AIPerceptionManager manager = (AIPerceptionManager)target;

            DrawDefaultInspector();

            EditorGUILayout.HelpBox($"Active AI perception count: {manager.ActiveAIPerceptionCount}", MessageType.Info);
        }
    }
}
