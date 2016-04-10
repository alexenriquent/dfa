using UnityEngine;
using System.Collections;

public class Trigger1C : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameObject[] receivers = GameObject.FindGameObjectsWithTag("GuardAgent");
            foreach (GameObject obj in receivers) {
                GuardAgent r = (GuardAgent) obj.GetComponent(typeof(GuardAgent));
                r.TriggerInput("3");
            }
        }      
    }
}
