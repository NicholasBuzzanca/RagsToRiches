using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {
    public bool isMuted;
    public static PersistentData singleton;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        singleton = this;
        isMuted = false;
	}
	
}
