using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    public Transform BottomExitPoint;
    public Transform UpperExitPoint;

    [HideInInspector]
    public List<Transform> ExitPoints;

    private void Awake()
    {
        ExitPoints.Add(BottomExitPoint);
        ExitPoints.Add(UpperExitPoint);
    }

    public Transform GetNearestExitPoint(Transform target) {

        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform point in ExitPoints)
        {
            float distance = Vector2.Distance(target.position, point.position);
            if (distance < minDistance) { 
                minDistance = distance;
                nearest = point;
            }
        }

        return nearest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.targetLadder = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.targetLadder = null;
        }
    }
}
