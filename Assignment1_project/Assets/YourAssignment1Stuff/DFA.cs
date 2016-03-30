using UnityEngine;
using System.Collections;

abstract public class DFA : MonoBehaviour {

//    protected Transform playerTransform;
    protected Vector3 destPos;
//    protected GameObject[] pointList;
    protected int currentState;
    protected int[,] dfaSpec;

    protected abstract void Initialise();
    protected abstract void DFAUpdate();

    void Start () {
        Initialise();
    }
    
    void Update () {
        DFAUpdate();
    }
}
