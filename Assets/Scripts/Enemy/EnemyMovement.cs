using FMODUnity;
using System.Collections;
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
    private PlayerHealth playerHealth;
    private StudioEventEmitter[] sounds;
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
        playerHealth = player.GetComponent<PlayerHealth>();
        sounds = GetComponentsInChildren<StudioEventEmitter>();

        DisableAllBodies();
        ChooseBody();

        if (WorldsController.instance.GetCurrentWorld() == World.NORMAL) 
        {
            BecomeDisabled();
        } 
        else
        {
            sounds[0].Play();
            BecomeIdle();
        }   

        GameEvents.instance.onNormalWorldEnter += (_) => BecomeDisabled();
        GameEvents.instance.onArcaneWorldEnter += (_) =>
        {
            sounds[0].Play();
            BecomeIdle();
        };

        StartCoroutine(GrowlRandomly());
    }

    IEnumerator GrowlRandomly()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(15f, 30f));
            if (currentState == State.CHASING) 
                sounds[1].Play();
        }
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
                AttackIfClose();
                break;

            case State.IDLE:
                UpdateLights();
                break;

            default:
                break;
        }
    }

    private void AttackIfClose()
    {
        if (distanceToPlayer - 1 < 2)
        {
            playerHealth.StartAfraid();
        }
        else
        {
            playerHealth.StopAfraid();
            
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
        playerHealth.StopAfraid();
        rigidBody.isKinematic = true;
        agent.isStopped = true;
        currentState = State.IDLE;
    }

    private void BecomeDisabled()
    {
        currentState = State.DISABLED;
        rigidBody.isKinematic = true;
        StartCoroutine(WaitForWorldChange());
    }

    IEnumerator WaitForWorldChange()
    {
        yield return new WaitWhile(() => gameObject.layer == LayerMask.NameToLayer("UninteractiveWorld"));
        sounds[0].Stop();
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
            if (Physics.Linecast(cam.transform.position, raycastTarget.position, out hit, ~(1 << LayerMask.NameToLayer("UninteractiveWorld") | 1 << LayerMask.NameToLayer("Trigger"))))
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