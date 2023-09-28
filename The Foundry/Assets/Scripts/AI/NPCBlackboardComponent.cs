using GameFramework.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class NPCBlackboardComponent : EDBTBlackboardComponent
    {
        public const string KEY_SEEN_ENEMY = "seenEnemy";

        protected override void OnBlackboardInitialized()
        {
            blackboard.AddKey<Component>(KEY_SEEN_ENEMY);
        }
    }
}
