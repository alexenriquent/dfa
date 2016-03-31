using UnityEngine;
using System;
using System.Collections;

public class StrategicAgent : DFA {

    public Transform[] patrolPoints;
    private int patrolPoint;
    private int waitPoint;
    private int hidePoint;
    private float speed;

    protected override void Initialise() {
        state = 0;
        speed = 2.0f;
        patrolPoint = 0;
        waitPoint = 0;
        hidePoint = patrolPoints.Length / 2;
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        player = objPlayer.transform;
        destination = patrolPoints[patrolPoint].position;
        specification = new int[,] {{1, 0, 1, -1, -1, -1, -1},
                                    {0, 1, 0, 2, -1, -1, -1},
                                    {0, 2, -1, 1, 3, 3, 3},
                                    {0, 3, -1, 1, 3, 3, 3}};
    }

    protected override void DFAUpdate() {
        switch (state) {
            case 0: Patrol(); break;
            case 1: Wait(); break;
            case 2: Hide(); break;
            case 3: Chase(); break;
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

    private void Wait() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            if (Vector3.Distance(transform.position, patrolPoints[waitPoint].position) < 0.1f) {
                destination = patrolPoints[waitPoint].position;
            } else {
                patrolPoint = waitPoint;
                destination = patrolPoints[patrolPoint % patrolPoints.Length].position;
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
            if (Vector3.Distance(transform.position, patrolPoints[hidePoint].position) < 0.1f) {
                destination = patrolPoints[hidePoint].position;
            } else {
                patrolPoint++;
                destination = patrolPoints[patrolPoint % patrolPoints.Length].position;
            }
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    private void Chase() {
        destination = player.position;

        Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
