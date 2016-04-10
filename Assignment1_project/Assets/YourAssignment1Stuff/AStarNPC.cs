using UnityEngine;
using System.Collections;

public class AStarNPC : NPC {

    protected AStarPath pathFinder;

    public void FindPathTo(Vector3 position) {
        StartCoroutine(pathFinder.FindPath(position));
    }

    protected virtual void MoveAlongPath() {
        if (pathFinder.Path.Count > 0) {
            this.destination = pathFinder.GetNextPosition();
            MoveToward(destination);
        } else {
            Pause();
        }
    }

    protected virtual Vector3 GetRandomPosition(Bounds b) {
        return new Vector3(Random.Range(b.min.x, b.max.x), 
                           Random.Range(b.min.y, b.max.y), 
                           Random.Range(b.min.z, b.max.z));
    }
}
