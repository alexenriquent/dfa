﻿using UnityEngine;
using System.Collections;

public class Trigger2E : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        GameObject[] receivers = GameObject.FindGameObjectsWithTag("FrightenedAgent");
        foreach (GameObject obj in receivers) {
            FrightenedAgent r = (FrightenedAgent) obj.GetComponent(typeof(FrightenedAgent));
            r.PostMessage("5");
        }
    }
}