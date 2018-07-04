using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public float speed = 5f; // speed of the guard
    public float waitTime = .3f; // how much the guard will stay at each waypoint
    public float turnSpeed = 90f; // 90 degrees per second - the turn speed of the guard

    public Transform pathHolder;

    private void Start() {

        // 2.1 Make guard follow the path

        Vector3[] waypoints = new Vector3[pathHolder.childCount]; // get array of all of the positions of the waypoints in the pathHolder
        for (int i = 0; i < waypoints.Length; i++) { // loop through all indexes in our array
            waypoints[i] = pathHolder.GetChild(i).position; // each waypoint with index i should get the position of each child with index of i
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); // make waypoints to be positioned at the Y axis of the guard.
        }

        StartCoroutine(FollowPath(waypoints)); // start the coroutine
    }

    // 2.1 Make guard follow the path

    IEnumerator FollowPath(Vector3[] waypoints) { // for path we are passing the waypoints array so we have count of 5.
        transform.position = waypoints[0]; // guard is positioned at the first waypoint

        int targetWaypointIndex = 1; // variable to keep track of the index of the waypoint we're currently moving towards. We're already at 0 (the first index), so 1 is the second index.
        Vector3 targetWaypoint = waypoints[targetWaypointIndex]; // the actuall position of the waypoint
        transform.LookAt(targetWaypoint); // make the guard initally faces the target waypoint

        while (true) { // loop forever 
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime); // move the guard from its current position to the target(next) waypoint with a maximum distance of speed * time.deltatime            
            if (transform.position == targetWaypoint) { // if we reach the targetWaypoint
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;// increment targetWaypointIndex by one, and after the guard reach targetWaypointIndex 5 go to 0
                //print("I am at index " + targetWaypointIndex);
                targetWaypoint = waypoints[targetWaypointIndex]; // targetWaypoint is set to the next waypoint index
                yield return new WaitForSeconds(waitTime); // when the guard reach the next waypoint we want to wait for .3 seconds
                yield return StartCoroutine(TurnToFace(targetWaypoint)); // after we finish the waiting at the waipoint we start TurnToFace, but waith while the guard is rotating
            }
            yield return null; // "wait until the next frame" - yeld for one frame between each iteration of the while loop - it will make the movement smooth.
        }
    }

    // 3. Make guard rotate at each waypoint

    IEnumerator TurnToFace(Vector3 lookTarget) { // look target is each next waypoint
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized; // if we know the direction to look at we can find the corresponing angle
        float targetAngle = 90 - Mathf.Atan2(directionToLookTarget.z, directionToLookTarget.x) * Mathf.Rad2Deg; // calculating the angle the guard will need to have on the Y axis to be facing the look target
        while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)  { // stop the while loop once the gard is facing the look target. While delta angle is greater than 0.05 keep rotating towards the target angle
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime); // rotate the guard going from the current rotation on the y axis to the target angle with a "speed" of turnSpeed * Time.deltaTime;
            transform.eulerAngles = Vector3.up * angle; // rotation is equal to rotation on the y axis multiplied by the angle
            print(transform.eulerAngles);
            yield return null; // "wait until the next frame" - yeld for one frame between each iteration of the while loop - it will make the movement smooth.
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
