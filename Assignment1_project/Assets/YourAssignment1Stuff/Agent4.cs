using UnityEngine;
using System.Collections;

public class Agent4 : DFA {

    public Transform[] patrolPoints;
    private int currPatrolPoint;
    private float speed;
 
	protected override void Initialise() {
        speed = 2.5f;
        currPatrolPoint = 0;
        destPos = patrolPoints[currPatrolPoint].position;
    }

    protected override void DFAUpdate() {
        Patrol();
    }

    private void Patrol() {
        Vector3 targetDir;
        Vector3 newDir;

        if (Vector3.Distance(transform.position, destPos) < 0.1f) {
            currPatrolPoint++;
            destPos = patrolPoints[(currPatrolPoint) % 3].position;
        }
      
        targetDir = destPos - transform.position;
        newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, Time.deltaTime * 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * speed);
    }
}
