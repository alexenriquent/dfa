using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject player;

	void Start() {
	    player = GameObject.FindGameObjectWithTag("Player");
	}
}
