using UnityEngine;
using System.Collections;

public class GuardAgent : AStarNPC {

    public Transform fleePoint;
    private Bounds area;

    void Start() {
        this.speed = 3.0f;
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").
                           GetComponent<GameManager>();
        this.pathFinder = this.gameObject.AddComponent<AStarPath>();
        this.pathFinder.gridWidth = this.GetComponent<Collider>().bounds.size.x / 2;
        this.pathFinder.SetObstacles("Obstacle");
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().freezeRotation = true;
        area = GameObject.Find("FirstRoomArea").GetComponent<Collider>().bounds;
        this.dfa = new DFA(new int[,] 
                   {{1, 0, 1, -1, -1},
                    {0, 1, 0, 2, -1},
                    {0, 2, 0, 2, 3},
                    {0, 3, -1, 1, 3}});
	}
	
	void Update() {
	    switch (this.dfa.State) {
            case 0: Wander(); break;
            case 1: Alert(); break; 
            case 2: Chase(); break;
            case 3: Flee(); break;
        }
	}
       
    private void Wander() {
        if (pathFinder.Path.Count > 0) {
            MoveAlongPath();
        } else {
            Vector3 randomPoint = GetWanderPosition(area);
            FindPathTo(randomPoint);
        }
    }

    private void Alert() {
        if (Grounded(this.transform.position)) {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 7, 0);
        }
    }

    private void Chase() {
        MoveTo(this.gameManager.player.transform.position);
    }

    private void Flee() {
        MoveTo(fleePoint.position);
    }

    private bool Grounded(Vector3 pos) {
        float distToGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(pos, -Vector3.up, distToGround + 0.1f)) {
            return true;
        }
        return false;
    }

    private Vector3 GetWanderPosition(Bounds b) {
        return new Vector3(Random.Range(b.min.x, b.max.x), 
                           Random.Range(b.min.y, b.max.y), 
                           Random.Range(b.min.z, b.max.z));
    }
}
