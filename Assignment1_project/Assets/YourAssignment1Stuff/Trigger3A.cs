using UnityEngine;
using System.Collections;

public class Trigger3A : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        GameObject[] receivers = GameObject.FindGameObjectsWithTag("StrategicAgent");
        foreach (GameObject obj in receivers) {
            StrategicAgent r = (StrategicAgent) obj.GetComponent(typeof(StrategicAgent));
            r.PostMessage("1");
        }
    }
}
