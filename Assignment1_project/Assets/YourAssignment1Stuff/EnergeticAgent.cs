using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct node {
    public Vector3 cell;
    public int parent;
    public float g;
    
    public node(Vector3 a, int b, float c) {
        cell = a;
        parent = b;
        g = c;
    }
} 

public class EnergeticAgent : DFA {

    public float gridWidth = 0.1f;
    private Vector3 position;
    private LinkedList<Vector3> path = null;
    List<Vector3> paintedSet;
    private bool findingPath;
    private bool foundPath;
    CharacterController controller;

    public Transform[] patrolPoints;
    private int patrolPoint;
    private float speed;

    protected override void Initialise() {
        state = 0;
        speed = 2.0f;
        paintedSet = new List<Vector3> ();
        position = transform.position;
        findingPath = false;
        foundPath = false;
        controller = GetComponent<CharacterController>();
        patrolPoint = 0;
        destination = patrolPoints[patrolPoint].position;
    }

    protected override void DFAUpdate() {
        Patrol();
    }

    public bool Grounded(Vector3 pos) {
        float distToGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(pos, -Vector3.up, distToGround + 0.1f)) {
            Debug.Log("Grounded");
            return true;
        }
        return false;
    }

    public LinkedList<Vector3> GreedySearch(Vector3 root, Vector3 goal, float gridSize, int depth) {
        if (depth == 0) {
            return null;
        }

        SortedList<float, node> children = new SortedList<float, node>();
        Vector3 x;
        float distance;

        x = new Vector3(root.x + gridSize, root.y, root.z);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add (distance, new node (x, -1, distance));
        }
        x = new Vector3(root.x - gridSize, root.y, root.z);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add (distance, new node (x, -1, distance));
        }
        x = new Vector3(root.x, root.y, root.z + gridSize);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add (distance, new node (x, -1, distance));
        }
        x = new Vector3(root.x, root.y, root.z - gridSize);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add (distance, new node (x, -1, distance));
        }
        x = new Vector3(root.x + gridSize, root.y, root.z + gridSize);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add (distance, new node(x, -1, distance));
        }
        x = new Vector3(root.x - gridSize, root.y, root.z + gridSize);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add (distance, new node (x, -1, distance));
        }
        x = new Vector3(root.x + gridSize, root.y, root.z - gridSize);
        if (Grounded(x)) {
            distance = Vector3.Distance(x, goal);
            children.Add(distance, new node(x, -1, distance));
        }
        x = new Vector3(root.x - gridSize, root.y, root.z - gridSize);
        if (Grounded(x)) {
            distance = Vector3.Distance (x, goal);
            children.Add (distance, new node (x, -1, distance));
        }

        node child;

        while (children.Count > 0) {
            child = children.Values[0];
            children.RemoveAt(0);
            if (!InList(child.cell, paintedSet)) {
                paintedSet.Add(child.cell);
                if (Vector3.Distance(child.cell, goal) <= gridSize) {
                    LinkedList<Vector3> path1 = new LinkedList<Vector3>();
                    path1.AddFirst(goal);
                    path1.AddFirst(child.cell);
                    return path1;
                }
                LinkedList<Vector3> path2 = GreedySearch(child.cell, goal, gridSize, depth - 1);
                if (path2 != null) {
                    path2.AddFirst(child.cell);
                    return path2;
                }
            }
        }
        return null;
    }

    public bool InList(Vector3 pos, List<Vector3> list) {
        foreach (Vector3 elem in list) {
            if (Vector3.Distance(elem, pos) < 0.5 * gridWidth) {
                return true;
            }
        }
        return false;
    }

    public LinkedList<Vector3> GreedySearchWrapper(Vector3 goal, float gridSize) {
        findingPath = true;
        LinkedList<Vector3> result = GreedySearch(position, goal, gridSize, 1000);
        findingPath = false;
        return result;
    }

    IEnumerator GreedyCoroutine(Vector3 goal, float gridSize) {
        path = GreedySearchWrapper(goal, gridSize);
        yield return 0;
    }

    private void Patrol() {
//        if (!findingPath && !foundPath) {
//            if (Grounded(patrolPoints[patrolPoint].position)) {
//                Debug.Log("OK");
//                paintedSet.Clear();
//                StartCoroutine(GreedyCoroutine(patrolPoints[patrolPoint].position, gridWidth));
//                foundPath = true;
//            } else {
//                Debug.Log("Invalid Path");
//            }
//        }
//
//        if (foundPath) {
//            if (path != null && path.Count > 0) {
//                Vector3 pos = path.First.Value;
//                Debug.Log (pos.ToString());
//                pos.y = transform.position.y;
//                controller.Move((pos - transform.position));
//                if (path.Count == 1) {
//                    position = pos;
//                    foundPath = false;
//                    patrolPoint = (patrolPoint + 1) % patrolPoints.Length;
//                }
//                path.RemoveFirst();
//            }
//        }
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
}