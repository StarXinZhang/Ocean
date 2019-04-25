using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CastleControl : MonoBehaviour
{
    public Material castleMaterial;

	// Use this for initialization
	void Start ()
    {
        this.GetComponent<Renderer>().material = castleMaterial;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeRotate()
    {
        this.transform.rotation = Random.rotation;
    }
}
