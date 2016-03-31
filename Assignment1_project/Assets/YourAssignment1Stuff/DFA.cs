using UnityEngine;
using System.Collections;

abstract public class DFA : MonoBehaviour {

    protected Transform player;
    protected Vector3 destination;
    protected int state;
    protected int[,] specification;

    protected abstract void Initialise();
    protected abstract void DFAUpdate();

    void Start () {
        Initialise();
    }
    
    void Update () {
        DFAUpdate();
    }
}
