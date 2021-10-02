using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public enum Direction
    {
        xAxis, zAxis
    }
    public Direction stairDirection;

    public Vector3 lowPoint;
    public Vector3 highPoint;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lowPoint, 0.33f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(highPoint, 0.33f);
    }
}
