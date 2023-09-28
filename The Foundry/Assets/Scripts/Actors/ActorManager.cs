using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class ActorManager : MonoBehaviour
    {
        private static ActorManager instance;
        public static ActorManager Instance
        {
            get
            {
                if (!instance)
                {
                    GameObject actorManagerObject = new GameObject("ActorManager");
                    instance = actorManagerObject.AddComponent<ActorManager>();
                }

                return instance;
            }
        }

        private HashSet<Actor> actors;
        public HashSet<Actor> Actors => actors;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            actors = new HashSet<Actor>();

            DontDestroyOnLoad(gameObject);
        }

        public void RegisterActor(Actor actor)
        {
            actors.Add(actor);
        }

        public void UnregisterActor(Actor actor)
        {
            actors.Remove(actor);
        }
    }
}
