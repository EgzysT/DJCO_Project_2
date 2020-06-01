﻿using NaughtyAttributes.Editor;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private NavMeshAgent agent;
    private Rigidbody rigidBody;
    private Transform raycastTargets;
    private Transform bodies;
    private GameObject currentBody;
    private State currentState;
    private float distanceToPlayer;

    private enum State
    {
        IDLE,
        CHASING,
        DISABLED
    }

    void Start()
    {
        player = GameObject.Find("Player");
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        raycastTargets = transform.Find("RaycastTargets");
        bodies = transform.Find("Body").transform;

        DisableAllBodies();
        ChooseBody();

        currentState = (WorldsController.instance.GetCurrentWorld() == World.NORMAL) ? State.DISABLED : State.IDLE;
        GameEvents.instance.onNormalWorldEnter += (_) => currentState = State.DISABLED;
        GameEvents.instance.onArcaneWorldEnter += (_) => currentState = State.IDLE;
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (currentState == State.DISABLED) return;

        if (PlayerCanSeeMe())
        {
            if (currentState != State.IDLE)
            {
                currentState = State.IDLE;
                ChooseBody();
                StopChasing();
            }
        }
        else
        {
            if (currentState != State.CHASING)
            {
                currentState = State.CHASING;
                StartChasing();
                agent.SetDestination(player.transform.position);
            }
        }

        switch (currentState)
        {
            case State.CHASING:
                agent.SetDestination(player.transform.position);
                // Damage Player
                break;

            case State.IDLE:
                break;

            default:
                break;
        }
    }

    private void ChooseBody()
    {
        int nextBodyIndex = UnityEngine.Random.Range(0, bodies.childCount);
        currentBody?.SetActive(false);
        currentBody = bodies.GetChild(nextBodyIndex).gameObject;
        currentBody.SetActive(true);
    }

    private void DisableAllBodies()
    {
        foreach (Transform body in bodies)
        {
            body.gameObject.SetActive(false);
        }
    }

    private void StopChasing()
    {
        rigidBody.isKinematic = true;
        agent.isStopped = true;
        currentState = State.IDLE;
    }

    private void StartChasing()
    {
        rigidBody.isKinematic = false;
        agent.isStopped = false;
        currentState = State.CHASING;
    }

    private bool PlayerCanSeeMe()
    {
        RaycastHit hit;

        foreach(Transform raycastTarget in raycastTargets)
        {
            Vector3 targetScreen = cam.WorldToViewportPoint(raycastTarget.position);
            if (targetScreen.x < 0 || targetScreen.x > 1 || targetScreen.y < 0 || targetScreen.y > 1 || targetScreen.z <= 0)
                continue;

            //Debug.DrawLine(cam.transform.position, raycastTarget.position, Color.white);
            if (Physics.Linecast(cam.transform.position, raycastTarget.position, out hit, ~(1 << LayerMask.NameToLayer("UninteractiveWorld"))))
            {
                if (hit.transform.name == gameObject.name)
                {
                    return true;
                }
            }
        }

        return false;
    }
}