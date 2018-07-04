using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveSpeed = 7f;
    public float smoothMoveTime = .1f; // the time it takes for the smoothInputMagnitude to catch up with the target magnitude - inputMagnitude
    public float turnSpeed = 8;

    float angle; // for keeping track of the current angle
    float smoothInputMagnitude; // smoother version of the inputMagnitude
    float smoothMoveVelocity; // store the velocity of the smoothing which Mathf.SmoothDamp will need to keep track of
    Vector3 velocity;

    Rigidbody playeRrigidbody;

    // Use this for initialization
    void Start () {
        playeRrigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // since we're dealing with direction we normalize the vectors
        float inputMagnitude = inputDirection.magnitude; // the lenght of the movement. If any of the keys is pressed it will be 1, otherwise it will be 0
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime); // damp from the current value to the target value, reference to the current smoothing velocity and the smoothTime

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg; // calculating our rotation using Atan2 based on the input converted in degreees
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime * inputMagnitude); // lerp from the current angle to the target angle with a speed of time.delta time * turnspeed for making the rotation smoother. If inputMagnitude is 0 it will stop the rotation at the current postion.

        // The two lines below are no longer needed, because after adding rigidbody to the object we will make use of the rigidbody (movement and rotation) in fixed update 

        //transform.eulerAngles = Vector3.up * angle; // set the rotation of the target (in eulerAngles, because targetAngle returns Quaternion) to rotation around the Y axis multiplied by the angle

        //transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagnitude, Space.World); // move the player along the z axis by the speed, by the smoothInputMagnitude (if there is no input the player will not move) and by Time.deltaTime in world space

        velocity = transform.forward * moveSpeed * smoothInputMagnitude; // to move the rigidbody we need to know the current velocity of the player

    }

    private void FixedUpdate() {
        playeRrigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle)); // rotates the rigidbody(the whole object) along the Y axis
        playeRrigidbody.MovePosition(playeRrigidbody.position + velocity * Time.deltaTime); // moves the object from its current position to a new position based on the velocity
        
    }
}
