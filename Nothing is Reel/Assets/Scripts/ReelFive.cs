using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReelFive : MonoBehaviour
{
    public Inventory inventory;
    public MeshRenderer renderReel;
    public BoxCollider collisionReel;

    // When Fifth Reel appears.
    void Start()
    {
        renderReel.enabled = false;
        collisionReel.enabled = false;
    }
    void Update()
    {
        if (inventory.reelFive == true)
        {
            renderReel.enabled = true;
            collisionReel.enabled = true;
        }
    }
}