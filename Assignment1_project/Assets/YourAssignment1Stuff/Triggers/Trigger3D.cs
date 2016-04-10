using UnityEngine;
using System.Collections;

public class Trigger3D : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameObject[] receivers = GameObject.FindGameObjectsWithTag("StrategicAgent");
            foreach (GameObject obj in receivers) {
                StrategicAgent r = (StrategicAgent) obj.GetComponent(typeof(StrategicAgent));
                r.TriggerInput("4");
            }
        }      
    }
}
