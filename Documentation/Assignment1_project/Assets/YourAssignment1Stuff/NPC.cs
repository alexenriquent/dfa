using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

    protected GameManager gameManager;
    protected DFA dfa;
    protected Vector3 destination;
    protected float speed;

    public void TriggerInput(string trigger) {
        dfa.DFAProgram(trigger);
    } 

    public void MoveTo(Vector3 position) {
        Vector3 direction = (position - this.transform.position).normalized;
        this.GetComponent<Rigidbody>().MovePosition(this.transform.position + direction * speed * Time.deltaTime);
    }

    public void MoveToward(Vector3 position) {
        Vector3 target = position - this.transform.position;
        this.GetComponent<Rigidbody>().velocity = Vector3.Normalize(target) * speed;
    }

    public void Pause() {
        float y = this.GetComponent<Rigidbody>().velocity.y;
        this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, y, 0.0f);
    }
}
