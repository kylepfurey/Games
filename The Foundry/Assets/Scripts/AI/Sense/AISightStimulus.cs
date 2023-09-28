using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.AI;

/// <summary>
/// Component for any game objects that can be seen by NPCs.
/// </summary>
public class AISightStimulus : MonoBehaviour, IAISightStimulus
{
    [SerializeField]
    private Transform owningTransform;
    public Transform OwningTransform => owningTransform;

    [SerializeField] 
    private bool shouldAgentsIgnoreThis = false;
    public bool ShouldAgentsIgnoreThis => shouldAgentsIgnoreThis;

    [SerializeField] 
    private Vector3 lookOffset = new Vector3(0.0f, 1.0f, 0.0f);

    private void OnValidate()
    {
        if (!owningTransform)
            owningTransform = transform;
    }

    public Vector3 GetFocusPosition()
    {
        return transform.position + lookOffset;
    }

    private void OnEnable()
    {
        AISightManager.EnsureCreation();
        AISightManager.Instance.SubscribeStimulus(this);
    }

    private void OnDisable()
    {
        AISightManager.Instance?.UnsubscribeStimulus(this);
    }
}
