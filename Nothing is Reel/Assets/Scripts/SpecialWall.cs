using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpecialWall : MonoBehaviour
{
    public Inventory inventory;
    public MeshRenderer renderWall;
    public BoxCollider collisionWall;

    // When Wall appears.
    void Start()
    {
        renderWall.enabled = false;
        collisionWall.enabled = false;
    }
    void Update()
    {
        if (inventory.reelAll == true)
        {
            renderWall.enabled = true;
            collisionWall.enabled = true;
        }
    }
}