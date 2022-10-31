using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRange : MonoBehaviour
{
    public float radius = 7f;//²£¥Í¼Ä¤H¥b®|

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0.3f, transform.position.z), radius);
    }
}
