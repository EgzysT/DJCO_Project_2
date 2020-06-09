using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private NavMeshAgent agent;
    private Rigidbody rigidBody;
    private Transform raycastTargets;
    private Transform bodies;
    private Light[] lights;
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
        lights = GetComponentsInChildren<Light>();

        DisableAllBodies();
        ChooseBody();

        if (WorldsController.instance.GetCurrentWorld() == World.NORMAL) 
        {
            BecomeDisabled();
        } 
        else
        {
            BecomeIdle();
        }   

        GameEvents.instance.onNormalWorldEnter += (_) => BecomeDisabled();
        GameEvents.instance.onArcaneWorldEnter += (_) => BecomeIdle();
    }

    void Update()
    {

        if (currentState == State.DISABLED) return;

        if (PlayerCanSeeMe())
        {
            if (currentState != State.IDLE)
            {
                currentState = State.IDLE;
                BecomeIdle();
            }
        }
        else
        {
            if (currentState != State.CHASING)
            {
                currentState = State.CHASING;
                StartChasing();
                ChooseBody();
                agent.SetDestination(player.transform.position);
            }
        }

        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        switch (currentState)
        {
            case State.CHASING:
                agent.SetDestination(player.transform.position);
                UpdateLights();
                break;

            case State.IDLE:
                UpdateLights();
                break;

            default:
                break;
        }
    }

    private void UpdateLights()
    {
        float lightIntensity = 3 - Mathf.Clamp(distanceToPlayer - 1, 0, 9) / 3;
        foreach (Light light in lights)
        {
            light.intensity = lightIntensity;
        }
    }

    private void TurnOfLights()
    {
        foreach (Light light in lights)
        {
            if (light.gameObject.activeInHierarchy)
            {
                StartCoroutine(TurnOfLightSmoothly(light));
            }
        }
    }


    IEnumerator TurnOfLightSmoothly(Light light)
    {
        while(light.intensity > 0)
        {
            light.intensity -= 0.1f;
            yield return new WaitForSeconds(0.1f);
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

    private void BecomeIdle()
    {
        rigidBody.isKinematic = true;
        agent.isStopped = true;
        currentState = State.IDLE;
    }

    private void BecomeDisabled()
    {
        currentState = State.DISABLED;
        rigidBody.isKinematic = true;
        TurnOfLights();
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