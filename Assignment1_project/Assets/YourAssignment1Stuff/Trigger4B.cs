using UnityEngine;
using System.Collections;

public class Trigger4B : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        GameObject[] receivers = GameObject.FindGameObjectsWithTag("AggressiveAgent");
        foreach (GameObject obj in receivers) {
            AggressiveAgent r = (AggressiveAgent) obj.GetComponent(typeof(AggressiveAgent));
            r.PostMessage("2");
        }
    }
}
