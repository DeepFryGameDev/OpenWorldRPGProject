using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpawnPoint : MonoBehaviour
{
    //shows red box for spawn points for easy identifying
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
