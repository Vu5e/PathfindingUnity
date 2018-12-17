﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAI : MonoBehaviour
{

    public GameObject player;
    private Vector3 playerLastPos;

    public GameObject[] Seekers;
    public GameObject[] respawn;
    Grid m_grid;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLastPos = player.transform.position;

        Seekers = GameObject.FindGameObjectsWithTag("NPC");
        m_grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (GameObject s in Seekers)
            {
                int numX = (int)Random.Range(0, m_grid.gridWorldSize.x);
                int numY = (int)Random.Range(0, m_grid.gridWorldSize.y);
                s.GetComponent<Unit>().target = m_grid.grid[numX, numY].NodeMesh.transform;
                m_grid.grid[numX, numY].NodeMesh.GetComponent<GridColor>().UpdateColor(Walkable.Blocked);
                StartCoroutine(ResetGridColor(m_grid.grid[numX, numY]));
            }
        }

        if (player.transform.position != playerLastPos)
        {
            playerLastPos = player.transform.position;
            SetBackToMainTarget();
        }


        if (Input.GetKeyUp(KeyCode.R))
        {
            foreach (GameObject s in Seekers)
            {
                StartCoroutine(RespawnAI(s));
            }

        }
    }

    public void SetBackToMainTarget()
    {
        foreach (GameObject s in Seekers)
        {
            s.GetComponent<Unit>().target = player.transform;
        }
    }

    IEnumerator ResetGridColor(Node n)
    {
        yield return new WaitForSeconds(5);
        n.NodeMesh.GetComponent<GridColor>().UpdateColor(Walkable.Passable);
    }

    IEnumerator RespawnAI(GameObject go)
    {
        yield return new WaitForSeconds(0.2f);
        go.transform.position = respawn[Random.Range(0, respawn.Length)].transform.position;
        foreach(Node n in GetComponent<Grid>().grid)
        {
            if(n.walkable != Walkable.Impassable)
            {
                n.walkable = Walkable.Passable;
            }
        }
        go.GetComponent<Unit>().isMoving = true;
        go.GetComponent<Unit>().UpdatePath();

    }
}
