using UnityEngine;
using System;
using System.Collections;

public class AggressiveAgent : DFA {

    public Transform[] patrolPoints;
    public Transform[] alertPoints;
    public Transform[] hidePoints;
    private int currPatrolPoint;
    private int currAlertPoint;
    private int currHidePoint;
    private float speed;
 
	protected override void Initialise() {
        currState = 0;
        speed = 1.5f;
        currPatrolPoint = 0;
        currAlertPoint = 0;
        currHidePoint = 0;
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        destPos = patrolPoints[currPatrolPoint].position;
        dfaSpec = new int[,]{{1, 0, 1, -1, -1},
                             {0, 1, 0, 2, -1},
                             {0, 2, -1, 1, 3},
                             {1, 3, -1, -1, 2}};
    }

    protected override void DFAUpdate() {
        switch (currState) {
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
        Debug.Log("Current state: " + currState); 
        currState = dfaSpec[currState, trigger];
        ResetState();
        Debug.Log("Trigger: " + trigger + " Current state: " + currState);
    }

    private void ResetState() {
        currPatrolPoint = 0;
        currAlertPoint = 0;
        currHidePoint = 0;

        switch (currState) {
            case 0: destPos = patrolPoints[currPatrolPoint].position; break;
            case 1: destPos = alertPoints[currAlertPoint].position; break;
            case 2: destPos = hidePoints[currHidePoint].position; break;
        }
    }

    private void Patrol() {
        Vector3 targetDir;
        Vector3 newDir;

        if (Vector3.Distance(transform.position, destPos) < 0.1f) {
            currPatrolPoint++;
            destPos = patrolPoints[currPatrolPoint % patrolPoints.Length].position;
        }
      
        targetDir = destPos - transform.position;
        newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * speed);
    }

    private void Alert() {
        Vector3 targetDir;
        Vector3 newDir;

        if (Vector3.Distance(transform.position, hidePoints[1].position) < 0.1f) {
            destPos = hidePoints[0].position;
        }

        if (Vector3.Distance(transform.position, destPos) < 0.1f) {
            currAlertPoint++;
            destPos = alertPoints[currAlertPoint % alertPoints.Length].position;
        } 

        targetDir = destPos - transform.position;
        newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * (speed * 1.5f));
    }

    private void Hide() {
        Vector3 targetDir;
        Vector3 newDir;

        if (Vector3.Distance(transform.position, destPos) < 0.1f) {
            currHidePoint = 1;
            destPos = hidePoints[currHidePoint].position;
        }

        targetDir = destPos - transform.position;
        newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * (speed * 1.5f));
      
    }

    private void Attack() {
        Vector3 targetDir;
        Vector3 newDir;

        if (Vector3.Distance(transform.position, hidePoints[1].position) < 0.1f) {
            destPos = hidePoints[0].position;
        }
        if (Vector3.Distance(transform.position, hidePoints[0].position) < 0.1f) {
            destPos = playerTransform.position;
        }

        targetDir = destPos - transform.position;
        newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * (speed * 1.5f));
    }
}
