using UnityEngine;
using System.Collections;

public class StrategicAgent : AStarNPC {

    private Bounds area;
    public Transform hidePoint;

	void Start () {
        this.speed = 5.0f;
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").
                           GetComponent<GameManager>();
        this.pathFinder = this.gameObject.AddComponent<AStarPath>();
        this.pathFinder.gridWidth = this.GetComponent<Collider>().bounds.size.x / 2;
        this.pathFinder.SetObstacles("Obstacle");
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().freezeRotation = true;
        area = GameObject.Find("ThirdRoomArea").GetComponent<Collider>().bounds;
        this.dfa = new DFA(new int[,] 
                   {{1, 0, 1, -1, -1, -1, -1},
                    {0, 1, 0, 2, -1, -1, -1},
                    {0, 2, -1, 1, 3, -1, -1},
                    {0, 3, -1, -1, 2, 3, 3}});
	}
	
	void Update () {
        switch (this.dfa.State) {
            case 0: Idle(); break;
            case 1: Patrol(); break;
            case 2: Hide(); break;
            case 3: Attack(); break;
        }
	}

    private void Idle() {
        if (pathFinder.Path.Count > 0) {
            destination = pathFinder.GetNextPosition();
            MoveToward(destination);
        } else {
            Pause();
        }
    }

    private void Patrol() {
        if (pathFinder.Path.Count > 0) {
            MoveAlongPath();
        } else {
            Vector3 randomPoint = GetPatrolPosition(area);
            FindPathTo(randomPoint);
        }
    }

    private void Hide() {
        MoveTo(hidePoint.position);
    }

    private void Attack() {
        MoveTo(this.gameManager.player.transform.position);
    }

    private Vector3 GetPatrolPosition(Bounds b) {
        return new Vector3(Random.Range(b.min.x, b.max.x), 
                           Random.Range(b.min.y, b.max.y), 
                           Random.Range(b.min.z, b.max.z));
    }
}
