using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Animator animator;

    #region Facial Objects
    [SerializeField] private GameObject leftEye;
    [SerializeField] private GameObject rightEye;
    #endregion

    #region Emotions
    public Material Neutral;
    public Material Happy;
    public Material Sad;
    public Material Angry;
    public Material Confused;
    public Material WTF;
    public Material Special;
    #endregion


    void Idle()
    {
        GetComponent<Animation>().Play("Idle");
        SetEyes(Neutral);
    }

    void Looming()
    {
        GetComponent<Animation>().Play("Looming");
        SetEyes(Special);
    }

    void SetEyes(Material eyes)
    {
        leftEye.GetComponent<Renderer>().material = eyes;
        rightEye.GetComponent<Renderer>().material = eyes;

    }
}
