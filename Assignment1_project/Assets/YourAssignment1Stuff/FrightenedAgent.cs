using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrightenedAgent : AStarNPC {

    public Transform lookPoint;
    private Bounds area;
    private List<GameObject> covers;

	void Start() {
        this.speed = 5.0f;
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").
                           GetComponent<GameManager>();
        this.pathFinder = this.gameObject.AddComponent<AStarPath>();
        this.pathFinder.gridWidth = this.GetComponent<Collider>().bounds.size.x / 2;
        this.pathFinder.SetObstacles("Obstacle");
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().freezeRotation = true;
        covers = new List<GameObject>();
        FindCovers();
        area = GameObject.Find("SecondRoomArea").GetComponent<Collider>().bounds;
        this.dfa = new DFA(new int[,] 
                   {{1, 0, 1, -1, -1},
                    {0, 1, 0, 2, 3},
                    {0, 2, 0, 1, 3},
                    {0, 3, -1, -1, 1}});
	}
	
	void Update() {
        switch (this.dfa.State) {
            case 0: Idle(); break;
            case 1: Patrol(); break;
            case 2: Look(); break;
            case 3: Hide(); break;
        }
	}

    private void FindCovers() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in objects) {
            if (obj.GetComponent<Collider>().bounds.Intersects(area) &&
               obj.name == "cylindrical Glass") {
                covers.Add(obj);
            }
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
            Vector3 randomPoint = GetRandomPosition(area);
            FindPathTo(randomPoint);
        }
    }

    private void Look() {
        MoveTo(lookPoint.position);
    }

    private void Hide() {
        if (DetectPlayer(gameManager.player.transform.position)) {
            if (pathFinder.Path.Count > 0) {
                MoveAlongPath();
            } else {
                Vector3 randomPoint = GetRandomPosition(area);
                FindPathTo(randomPoint);
            }
        } 
    }

    private bool DetectPlayer(Vector3 player) {
        RaycastHit ray;
        Vector3 direction = player - this.transform.position;
        if (Physics.Raycast(this.transform.position, direction, out ray)) {
            if (ray.collider.gameObject == gameManager.player) {
                return true;
            }
        }
        return false;
    }
}
