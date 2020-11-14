﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // finds the player; there should only be one, which should be generated by the tile controller.
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // if it couldn't find the object before, it tries again.
        // without this, the object would instantiate after the CameraBehaviour's Start() function is called.
        // that would thus leave this object as null.
        if(player == null)
            player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;

        // the z-axis position doesn't change.
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
