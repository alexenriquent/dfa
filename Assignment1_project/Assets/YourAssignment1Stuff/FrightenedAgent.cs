using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrightenedAgent : AStarNPC {

    public Transform lookPoint;
    private Bounds area;
    private List<GameObject> covers;
    private const float LARGE_NUMBER = 100000000.0f;

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
            Vector3 randomPoint = GetPatrolPosition(area);
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
                Vector3 randomPoint = GetPatrolPosition(area);
                FindPathTo(randomPoint);
            }
        } 
//        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
//        if (pathFinder.Path.Count > 0) {
//            destination = pathFinder.GetNextPosition();
//            MoveToward(destination);
//        } else if (DetectPlayer(gameManager.player.transform.position)) {
////            Dictionary<GameObject, float> agentAspect = GetCoversDistances(covers, this.transform.position);
////            Dictionary<GameObject, float> playerAspect = GetCoversDistances(covers, playerPos);
////            GameObject cover = GetCover(agentAspect, playerAspect);
////            Vector3 coverPosition = GetCoverPosition(cover, playerPos);
////            FindPathTo(coverPosition);
//            Vector3 randomPoint = GetPatrolPosition(area);
//            FindPathTo(randomPoint);
//        } else {
//            Pause();
//        }   
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

//    private Dictionary<GameObject, float> GetCoversDistances(List<GameObject> objs, Vector3 pos) {
//        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();
//        foreach (GameObject obj in objs) {
//            float distance = Vector3.Distance(obj.transform.position, pos);
//            distances.Add(obj, distance);
//        }
//        return distances;
//    }
//
//    private GameObject GetNearestCover(Dictionary<GameObject, float> objs, Vector3 pos) {
//        float distance = LARGE_NUMBER;
//        GameObject nearestCover = null;
//        foreach (GameObject elem in objs.Keys) {
//            if (objs[elem] < distance) {
//                nearestCover = elem;
//                distance = objs[elem];
//            }   
//        }
//        return nearestCover;
//    }
//
//    private Vector3 GetCoverPosition(GameObject obj, Vector3 player) {
//        Vector3 distance = Vector3.Normalize(player - obj.transform.position);
//        Vector3 position = (-1 * distance) * (obj.transform.localScale.x / 2);
//        return obj.transform.position + position;
//    }
//
//    private GameObject GetCover(Dictionary<GameObject, float> agentAspect, 
//                                Dictionary<GameObject, float> playerAspect) {
//        GameObject cover = GetNearestCover(agentAspect, this.transform.position);
//        float distance = 0.0f;
//        foreach (GameObject elem in agentAspect.Keys) {
//            if (playerAspect[elem] > distance && playerAspect[elem] >= agentAspect[elem]) {
//                cover = elem;
//                distance = playerAspect[elem];
//            }
//        }
//        return cover;
//    }

    private Vector3 GetPatrolPosition(Bounds b) {
        return new Vector3(Random.Range(b.min.x, b.max.x), 
                           Random.Range(b.min.y, b.max.y), 
                           Random.Range(b.min.z, b.max.z));
    }
}
