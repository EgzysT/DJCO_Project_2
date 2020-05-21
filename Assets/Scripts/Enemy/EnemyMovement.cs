using NaughtyAttributes.Editor;
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
    private bool playerSawSeeMe;

    void Awake()
    {
        player = GameObject.Find("Player");
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        raycastTargets = transform.Find("RaycastTargets");
        bodies = transform.Find("Body").transform;
        ChooseBody();
        StartChasing();
    }

    void Update()
    {
        if (playerSawSeeMe == PlayerCanSeeMe()) return;
        else playerSawSeeMe ^= true;

        if (playerSawSeeMe)
        {
            ChooseBody();
            StopChasing();
        } 
        else
        {
            StartChasing();
        }
    }

    private void ChooseBody()
    {
        int nextBodyIndex = UnityEngine.Random.Range(0, bodies.childCount);
        currentBody?.SetActive(false);
        currentBody = bodies.GetChild(nextBodyIndex).gameObject;
        currentBody.SetActive(true);
    }

    private void StopChasing()
    {
        agent.isStopped = true;
        rigidBody.isKinematic = true;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void StartChasing()
    {
        rigidBody.isKinematic = false;
        rigidBody.constraints = RigidbodyConstraints.None;
        agent.SetDestination(player.transform.position);
        agent.isStopped = false;
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
            if (Physics.Linecast(cam.transform.position, raycastTarget.position, out hit))
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