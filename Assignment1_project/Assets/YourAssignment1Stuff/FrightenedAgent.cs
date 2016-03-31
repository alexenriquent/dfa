using UnityEngine;
using System;
using System.Collections;

public class FrightenedAgent : DFA {

    public Transform[] patrolPoints;
    public Transform[] hidePoints;
    public Transform[] fleePoints;
    public Transform awarePoint;
    public Rigidbody rb;
    private int patrolPoint;
    private int hidePoint;
    private int fleePoint;
    private float speed;

    protected override void Initialise() {
        state = 0;
        speed = 2.0f;
        patrolPoint = 0;
        hidePoint = 0;
        fleePoint = 0;
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        player = objPlayer.transform;
        rb = GetComponent<Rigidbody>();
        destination = patrolPoints[patrolPoint].position;
        specification = new int[,] {{1, 0, 1, -1, -1, -1, -1},
                                    {0, 1, 0, 2, -1, -1, -1},
                                    {0, 2, -1, 1, 3, -1, -1},
                                    {0, 3, -1, -1, 2, 4, 3},
                                    {0, 4, -1, -1, 2, 4, 3}};
    }

    protected override void DFAUpdate() {
        switch (state) {
            case 0: Patrol(); break;
            case 1: Aware(); break;
            case 2: Hide(); break;
            case 3: Frightened(); break;
            case 4: Flee(); break;
        }
    }

    public void PostMessage(string str) {
        int val = Convert.ToInt32(str);
        DFAProgram(val + 1);
    }

    public void DFAProgram(int trigger) {
        Debug.Log("Current state: " + state); 
        state = specification[state, trigger];
        ResetState();
        Debug.Log("Trigger: " + (trigger - 1) + " Current state: " + state);
    }

    private void ResetState() {
        if (state == -1) {
            state = 0;
        }

        patrolPoint = 0;
        hidePoint = 0;
        fleePoint = 0;

        switch (state) {
            case 0: destination = patrolPoints[patrolPoint].position; break;
            case 1: destination = awarePoint.position; break;
            case 2: destination = hidePoints[hidePoint].position; break;
            case 3: destination = fleePoints[fleePoint].position; break;
            case 4: destination = fleePoints[fleePoint + 1].position; break;
        }
    }

    private void Patrol() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            patrolPoint++;
            destination = patrolPoints[patrolPoint % patrolPoints.Length].position;
        }
      
        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private void Aware() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            if (transform.position.y <= awarePoint.position.y) {
                rb.velocity = new Vector3(0, 7, 0);
            }
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private void Hide() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            if (Vector3.Distance(transform.position, hidePoints[hidePoints.Length - 1].position) < 0.1f) {
                hidePoint = hidePoints.Length - 1;
            } else {
                hidePoint++;
                destination = hidePoints[hidePoint % hidePoints.Length].position;
            }
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private void Frightened() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            destination = fleePoints[fleePoint].position;
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private void Flee() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            destination = fleePoints[fleePoint + 1].position;
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }   	
}
