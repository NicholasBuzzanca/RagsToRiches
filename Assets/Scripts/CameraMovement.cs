﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//obsolete class...only used for the "isHovering" var.
//isHovering is for detecting if mouse is over UI element
public class CameraMovement : MonoBehaviour {

    public static CameraMovement singleton;

    public bool isHovering;

    public float startSpeed;
    public float maxSpeed;

    public float topBarrier;
    public float botBarrier;
    public float leftBarrier;
    public float rightBarrier;

    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float botBoundary;

    Vector3 lastMove;

    void Start()
    {
        if (singleton == null)
            singleton = this;
    }

    // Update is called once per frame
    //void Update () {
        /*if (isHovering)
        {
            lastMove = Vector3.zero;
            return;
        }

        Vector3 move = Vector3.zero;
		if(Input.mousePosition.y >= Screen.height * topBarrier && transform.position.z <= topBoundary)
        {
            move += Vector3.forward * Time.deltaTime * startSpeed;
        }
        if (Input.mousePosition.y <= Screen.height * botBarrier && transform.position.z >= botBoundary)
        {
            move += Vector3.back * Time.deltaTime * startSpeed;
        }
        if (Input.mousePosition.x >= Screen.width * rightBarrier && transform.position.x <= rightBoundary)
        {
            move += Vector3.right * Time.deltaTime * startSpeed;
        }
        if (Input.mousePosition.x <= Screen.width * leftBarrier && transform.position.x >= leftBoundary)
        {
            move += Vector3.left * Time.deltaTime * startSpeed;
        }
        if (move == Vector3.zero)
        {
            lastMove = Vector3.zero;
        }
        else
        {
            transform.Translate(move + lastMove, Space.World);

            lastMove += move;
            lastMove = lastMove.normalized * Mathf.Min(lastMove.magnitude, maxSpeed);
        }

        if (transform.position.x >= rightBoundary)
        {
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        }
        if (transform.position.x <= leftBoundary)
        {
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
        if (transform.position.z >= topBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, topBoundary);
        }
        if (transform.position.z <= botBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, botBoundary);
        }
    }*/
}
