using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.AI
{
    [CustomEditor(typeof(AISightManager))]
    public class AISightManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AISightManager aiSightManager = (AISightManager)target;

            string infoBoxMessage = 
                $"Active AI sight count: {aiSightManager.ActiveAISightCount}\n" +
                $"Active stimulus count: {aiSightManager.ActiveStimulusCount}";

            EditorGUILayout.HelpBox(infoBoxMessage, MessageType.Info);
        }
    }
}
