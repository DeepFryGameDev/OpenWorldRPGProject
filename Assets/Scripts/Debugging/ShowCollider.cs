using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCollider : MonoBehaviour
{
    //This script is only to help identify where invisible colliders are in Unity
    void OnDrawGizmos()
    {
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}