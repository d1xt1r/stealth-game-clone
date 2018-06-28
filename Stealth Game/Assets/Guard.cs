using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform pathHolder;

    // 2. Make guard moving along the path

    private void Start() {

        Vector3[] waypoints = new Vector3[pathHolder.childCount]; // get array of all of the positions of the waypoints in the pathHolder
        for (int i = 0; i < waypoints.Length; i++) { // loop through all indexes in our array
            waypoints[i] = pathHolder.GetChild(i).position; // each waypoint with index i should be equal to each child (posistion) with index of i  
        }
    }

    // 1. Draw spheres on each wayoint and connect them with lines

    void OnDrawGizmos() {
        Vector3 startPosition = pathHolder.GetChild(0).position; // startPosition is the position of the first waypoint
        Vector3 previousPosition = startPosition; // previousPosition is initially equal to the startPosition
        foreach (Transform  waypoint in pathHolder) { // loop through all waypoints (child objects) in pathHolder 
            Gizmos.DrawSphere (waypoint.position, .3f); // draw sphere on the center of the position with radius .3f
            Gizmos.DrawLine(previousPosition, waypoint.position); // draw line from the previous positon to the current waypoint position
            previousPosition = waypoint.position; // then the previousPosition get set to the current waypoint position so DrawLine will draw again from previousPosition which is now the next waypoint
        }
        Gizmos.DrawLine(previousPosition, startPosition); // close the loop - connect the last waypoint to the first waypoint
    }
}
