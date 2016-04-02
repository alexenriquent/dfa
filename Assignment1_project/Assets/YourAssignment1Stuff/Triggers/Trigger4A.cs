using UnityEngine;
using System.Collections;

public class Trigger4A : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameObject[] receivers = GameObject.FindGameObjectsWithTag("EntityAgent");
            foreach (GameObject obj in receivers) {
                EntityAgent r = (EntityAgent) obj.GetComponent(typeof(EntityAgent));
                r.TriggerInput("1");
            }
        }      
    }
}
