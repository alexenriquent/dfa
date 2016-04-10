using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Node {
    public Vector3 cell;
    public int parent;
    public float g;

    public Node(Vector3 cell, int parent, float g) {
        this.cell = cell;
        this.parent = parent;
        this.g = g;
    }
}

public class AStarPath : MonoBehaviour {

    public float gridWidth;
    public Vector3 currentDestination;
    public Vector3 desiredDestination;
    private bool findingPath;
    protected float nextSearchTime = 0.0f;
    protected List<GameObject> obstacles;
    protected LinkedList<Vector3> path = new LinkedList<Vector3>();

    public List<GameObject> Obstacles {
        get { return obstacles; }
    }

    public void SetObstacles(string tag) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        obstacles = new List<GameObject>(objects);
        obstacles.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    public LinkedList<Vector3> Path {
        get { return path; }
    }

	public IEnumerator FindPath(Vector3 goal) {
        if (!findingPath && this.desiredDestination != goal) {
            currentDestination = FindValidGoalPosition(goal);
            while (path.Count <= 0 || path.Last.Value != currentDestination) {
                if (!findingPath) {
                    StartCoroutine(AStarPathSearch());
                }
                yield return null;
            }
            this.desiredDestination = goal;
        }
        yield return 0;
    }

    public Vector3 GetNextPosition() {
        if (path.Count > 0) {
            if (IsAtPosition(path.First.Value)) {
                path.RemoveFirst();
            }
            if (path.Count > 0) {
                return path.First.Value;
            }
        }
        return this.transform.position;
    }

    protected IEnumerator AStarPathSearch() {
        findingPath = true;
        nextSearchTime = Time.time + 0.02f;

        SortedList<float, Node> open = new SortedList<float, Node>();
        Node root = new Node(this.transform.position, -1, 0.0f);
        open.Add(Vector3.Distance(this.transform.position, currentDestination), root);
        SortedList<int, Node> closed = new SortedList<int, Node>();
        List<Node> children = new List<Node>();
        Node x;

        while (open.Count > 0) {  
            x = open.Values[0];
            open.RemoveAt(0);
            int parent = closed.Count;
            closed.Add(parent, x);

            float g = x.g + gridWidth;
            children.Add(new Node(new Vector3(x.cell.x + gridWidth, x.cell.y, x.cell.z), parent, g));
            children.Add(new Node(new Vector3(x.cell.x - gridWidth, x.cell.y, x.cell.z), parent, g));
            children.Add(new Node(new Vector3(x.cell.x, x.cell.y, x.cell.z + gridWidth), parent, g));
            children.Add(new Node(new Vector3(x.cell.x, x.cell.y, x.cell.z - gridWidth), parent, g));

            g = x.g + (Mathf.Sqrt(2.0f) * gridWidth);
            children.Add(new Node(new Vector3(x.cell.x + gridWidth, x.cell.y, x.cell.z + gridWidth), parent, g));
            children.Add(new Node(new Vector3(x.cell.x - gridWidth, x.cell.y, x.cell.z + gridWidth), parent, g));
            children.Add(new Node(new Vector3(x.cell.x + gridWidth, x.cell.y, x.cell.z - gridWidth), parent, g));
            children.Add(new Node(new Vector3(x.cell.x - gridWidth, x.cell.y, x.cell.z - gridWidth), parent, g));

            while (children.Count > 0) {
                x = children[0];
                children.RemoveAt(0);
                if (!Intersects(x.cell)) {
                    if (Vector3.Distance(x.cell, currentDestination) <= gridWidth) {
                        LinkedList<Vector3> returnList = new LinkedList<Vector3>();
                        returnList.AddFirst(currentDestination);
                        returnList.AddFirst(x.cell);
                        while (x.parent > -1) {
                            if (!closed.TryGetValue(x.parent, out x)) {
                                findingPath = false;
                                yield break;
                            }
                            returnList.AddFirst(x.cell);
                        }
                        findingPath = false;
                        path = returnList;
                        yield break;
                    }
                    if (!IsInList<float>(x.cell, open) && !IsInList<int>(x.cell, closed)) {
                        g = x.g + Vector3.Distance(x.cell, currentDestination) + 0.1f * Random.value * Random.value;
                        try {
                            open.Add(g, x);
                        } catch (System.Exception e) {
                            print(e.Message);
                            findingPath = false;
                            yield break;
                        }
                    }
                }
                if (Time.time > nextSearchTime) {
                    nextSearchTime = Time.time + 0.02f;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        findingPath = false;
        yield break;
    }

    protected Vector3 FindValidGoalPosition(Vector3 goal) {
        goal = new Vector3(goal.x, this.transform.position.y, goal.z);
        if (Intersects(goal)) {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(new Vector3(goal.x + gridWidth, goal.y, goal.z));
            positions.Add(new Vector3(goal.x - gridWidth, goal.y, goal.z));
            positions.Add(new Vector3(goal.x, goal.y, goal.z + gridWidth));
            positions.Add(new Vector3(goal.x, goal.y, goal.z - gridWidth));
            positions.Add(new Vector3(goal.x + gridWidth, goal.y, goal.z + gridWidth));
            positions.Add(new Vector3(goal.x - gridWidth, goal.y, goal.z + gridWidth));
            positions.Add(new Vector3(goal.x + gridWidth, goal.y, goal.z - gridWidth));
            positions.Add(new Vector3(goal.x - gridWidth, goal.y, goal.z - gridWidth));

            int attempts = 0;

            while (attempts < 100) {
                for (int i = 0; i < positions.Count; i++) {
                    if (!Intersects(positions[i])) {
                        return positions[i];
                    }
                }
                positions[0] = new Vector3(positions[0].x + gridWidth, positions[0].y, positions[0].z);
                positions[1] = new Vector3(positions[1].x - gridWidth, positions[1].y, positions[1].z);
                positions[2] = new Vector3(positions[2].x, positions[2].y, positions[2].z + gridWidth);
                positions[3] = new Vector3(positions[3].x, positions[3].y, positions[3].z - gridWidth);
                positions[4] = new Vector3(positions[4].x + gridWidth, positions[4].y, positions[4].z + gridWidth);
                positions[5] = new Vector3(positions[5].x - gridWidth, positions[5].y, positions[5].z + gridWidth);
                positions[6] = new Vector3(positions[6].x + gridWidth, positions[6].y, positions[6].z + gridWidth);
                positions[7] = new Vector3(positions[7].x - gridWidth, positions[7].y, positions[7].z - gridWidth);

                attempts++;
            }
        }
        return goal;
    }

    protected bool Intersects(Vector3 pos) {
        Bounds pointBounds = new Bounds(pos, new Vector3(gridWidth, gridWidth, gridWidth));
        foreach (GameObject obstacle in obstacles) {
            Bounds ob = obstacle.GetComponent<Collider>().bounds;
            if (ob.Intersects(pointBounds)) {
                return true;
            }
        }
        return false;
    }

    protected bool IsInList<T>(Vector3 pos, SortedList<T, Node> list) {
        foreach (KeyValuePair<T, Node> elem in list) {
            if (elem.Value.cell == pos) {
                return true;
            } 
        }
        return false;
    }

    protected bool IsAtPosition(Vector3 goal) {
        if (Vector3.Distance(this.transform.position, goal) <= gridWidth) {
            return true;
        }
        return false;
    }
}
