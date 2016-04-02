using UnityEngine;
using System;
using System.Collections;

public class DFA {

	private int state;
    private int[,] specification;

    public DFA(int[,] specification) {
        this.state = 0;
        this.specification = specification;
    }

    public int State {
        get { return this.state; }
        set { this.state = value; }
    }

    public int[,] Specification {
        get { return this.specification; }
        set { this.specification = value; }
    }

    public void DFAProgram(string trigger) {
        Debug.Log("Current state: " + this.state);
        int index = Convert.ToInt32(trigger);
        this.state = specification[this.state, index + 1];
        Debug.Log("Trigger: " + index + " Current state: " + this.state);
    }
}
