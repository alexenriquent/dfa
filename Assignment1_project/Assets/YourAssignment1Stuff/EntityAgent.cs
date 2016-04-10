using UnityEngine;
using System.Collections;

public class EntityAgent : NPC {

    private Vector3 velocity;
    private Vector3 angularVelocity;
    private const int MAGNITUDE = 0;

	void Start () {
        this.speed = 5.0f;
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").
                           GetComponent<GameManager>();
        Vector3 randVector = new Vector3(Random.Range(-1.0f, 1.0f), 
                             Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        this.GetComponent<Rigidbody>().velocity = randVector;
        this.GetComponent<Rigidbody>().velocity *= this.speed;
        this.velocity = Vector3.zero;
        this.angularVelocity = Vector3.zero;
        this.dfa = new DFA(new int[,] 
                   {{1, 0, 1, 2, 3, 1},
                    {0, 1, 0, 2, 3, 0},
                    {0, 2, 0, 2, 3, 0},
                    {0, 3, 0, 2, 3, 0}});
	}

    void Update() {
        switch (this.dfa.State) {
            case 0: Stop(); break;
            case 1: Bounce(); break;
            case 2: Chase(); break;
            case 3: Flee(); break;
        }
    }

    private void OnCollisionExit(Collision collision) {
        this.GetComponent<Rigidbody>().velocity = 
            this.GetComponent<Rigidbody>().velocity.normalized * this.speed;
    }

    private void Stop() {
        if (this.GetComponent<Rigidbody>().velocity.sqrMagnitude > MAGNITUDE) {
            velocity = this.GetComponent<Rigidbody>().velocity;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            angularVelocity = this.GetComponent<Rigidbody>().angularVelocity;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        } else if (this.GetComponent<Rigidbody>().velocity.sqrMagnitude == MAGNITUDE) {
            this.GetComponent<Rigidbody>().velocity = velocity;
            this.GetComponent<Rigidbody>().velocity = angularVelocity;
        } 
    }

    private void Bounce() {
        Vector3 player = this.gameManager.player.transform.position;
        if (Vector3.Distance(this.transform.position, player) < 4.0f) {
            Vector3 direction = this.transform.position - player;
            this.GetComponent<Rigidbody>().velocity = direction.normalized * this.speed;
        }
    }

    private void Chase() {
        Vector3 player = this.gameManager.player.transform.position;
        if (Vector3.Distance(this.transform.position, player) > 2.0f) {
            Vector3 direction = player - this.transform.position;
            this.GetComponent<Rigidbody>().velocity = direction.normalized * this.speed;
        }
    }

    private void Flee() {
        Vector3 player = this.gameManager.player.transform.position;
        if (Vector3.Distance(this.transform.position, player) < 4.0f) {
            Vector3 direction = this.transform.position - player;
            this.GetComponent<Rigidbody>().velocity = direction.normalized * this.speed;
        }
    }
}
