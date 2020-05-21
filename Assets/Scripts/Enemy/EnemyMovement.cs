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
    private bool isVisible;
    public float offScreenDot = 0.8f;
    public 
    void Awake()
    {
        player = GameObject.Find("Player");
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log(PlayerCanSeeMe());
        if (PlayerCanSeeMe())
        {
            agent.isStopped = true;
            //rigidBody.isKinematic = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            //rigidBody.angularVelocity = Vector3.zero;
            //Debug.Log("isKinematic");
            //rigidBody.velocity = Vector3.zero;
        } else
        {
            //rigidBody.isKinematic = false;
            //Debug.Log("NOT isKinematic");
            rigidBody.constraints = RigidbodyConstraints.None;
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
        }
    }

    //se calhar e preciso mudar estas duas para o dot https://youtu.be/JAAGdF-Wdas
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private bool PlayerCanSeeMe()
    {
        return IsInView(player, gameObject);
        if (!isVisible) return false;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.TransformDirection(Vector3.forward), transform.position - cam.transform.position, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.CompareTag("Enemy");
        }

        return false;
        /*Vector3 forward = Camera.main.transform.forward;
        Vector3 other = (transform.position - playerTransform.position).normalized;
        float dot = Vector3.Dot(forward, other);
        Debug.Log(dot);
        return dot > offScreenDot;*/
    }

    private bool IsInView(GameObject origin, GameObject toCheck)
    {
        Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            Debug.Log("Behind: " + toCheck.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = toCheck.transform.position - origin.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out hit))
        {
            if (hit.transform.name != toCheck.name)
            {
                Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        return true;
    }
}