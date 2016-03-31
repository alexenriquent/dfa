using UnityEngine;
using System;
using System.Collections;

public class AggressiveAgent : DFA {

    public Transform[] patrolPoints;
    public Transform[] alertPoints;
    public Transform[] hidePoints;
    public Transform attackPoint;
    private int patrolPoint;
    private int alertPoint;
    private int hidePoint;
    private float speed;
 
	protected override void Initialise() {
        state = 0;
        speed = 2.0f;
        patrolPoint = 0;
        alertPoint = 0;
        hidePoint = 0;
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        player = objPlayer.transform;
        destination = patrolPoints[patrolPoint].position;
        specification = new int[,] {{1, 0, 1, -1, -1},
                                    {0, 1, 0, 2, -1},
                                    {0, 2, -1, 1, 3},
                                    {1, 3, -1, -1, 2}};
    }

    protected override void DFAUpdate() {
        switch (state) {
            case 0: Patrol(); break;
            case 1: Alert(); break;
            case 2: Hide(); break;
            case 3: Attack(); break;
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
        patrolPoint = 0;
        alertPoint = 0;
        hidePoint = 0;

        switch (state) {
            case 0: destination = patrolPoints[patrolPoint].position; break;
            case 1: destination = alertPoints[alertPoint].position; break;
            case 2: destination = hidePoints[hidePoint].position; break;
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

    private void Alert() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, hidePoints[1].position) < 0.1f) {
            destination = hidePoints[0].position;
        }

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            alertPoint++;
            destination = alertPoints[alertPoint % alertPoints.Length].position;
        } 

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * (speed * 1.5f));
    }

    private void Hide() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, destination) < 0.1f) {
            hidePoint = 1;
            destination = hidePoints[hidePoint].position;
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * (speed * 1.5f));
      
    }

    private void Attack() {
        Vector3 target;
        Vector3 direction;

        if (Vector3.Distance(transform.position, hidePoints[1].position) < 0.1f) {
            destination = attackPoint.position;
        }
        if (Vector3.Distance(transform.position, attackPoint.position) < 0.1f) {
            destination = player.position;
        }

        target = destination - transform.position;
        direction = Vector3.RotateTowards(transform.forward, target, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * (speed * 1.5f));
    }
}
