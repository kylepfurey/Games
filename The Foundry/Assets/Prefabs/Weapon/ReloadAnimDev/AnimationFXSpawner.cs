using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ANIMATION FX SPAWNER
//This is a script to enable GameObject instantiation through animation timings.
//Useful for dialogue audio, special fx, and pretty much anything that needs to happen during animations that can't be set to bones.
//WRITTEN BY LIAM SHELTON
public class AnimationFXSpawner : StateMachineBehaviour
{
    public GameObject prefabToSpawn;
    public float normalizedTimeToSpawnAt = 0.0f; //Use this to control the precise moment to spawn the GameObject
    public string subObjectToSpawnAt = "null"; //Specify the name of the sub-object here. Has to be a string due to inability to specify GameObject references in StateMachineBehaviors
    public bool instantiateInWorldSpace = false; //does what it says.

    private bool hasActivated = false;



    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasActivated = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > normalizedTimeToSpawnAt && !hasActivated)
        {
            hasActivated = true;

            //figure out where the object will spawn at
            GameObject theObject = findTheGameObject(animator);
            //GameObject theNewFX = GameObject.Instantiate(prefabToSpawn, theObject.transform, instantiateInWorldSpace);  

            GameObject theNewFX = GameObject.Instantiate(prefabToSpawn, theObject.transform.position, theObject.transform.rotation);
            theNewFX.transform.parent = theObject.transform;
        }
    }



    //Encapsulation of some logic for specifying where to instantiate the GameObject.
    private GameObject findTheGameObject(Animator animator)
    {
        GameObject theObject = null;
        if (subObjectToSpawnAt == "null")
        {
            theObject = animator.gameObject;
        }
        else
        {
            //find the object to spawn at (POSSIBLY SLOW but probably fine on modern hardware)
            foreach (Transform subTransform in animator.GetComponentsInChildren<Transform>())
            {
                if (subTransform.gameObject.name == subObjectToSpawnAt)
                {
                    theObject = subTransform.gameObject;
                }
            }

            //if still null, then we couldn't find the object! fallback to animator gameobject
            if (theObject == null)
            {
                Debug.LogError($"AnimationFXSpawner could not locate sub-object of name {subObjectToSpawnAt}. Reverting to animator object {animator.gameObject.name}.");
                theObject = animator.gameObject;
            }
        }
        return theObject;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
