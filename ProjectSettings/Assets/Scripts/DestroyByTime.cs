using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    private float lifetime = 2f;
    
	void Start () {
        Destroy(gameObject, lifetime);
	}

}
