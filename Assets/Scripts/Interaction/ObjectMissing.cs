using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ObjectMissing : MonoBehaviour {

    [Header("Event trigger ID (negative to not trigger)")]
    public int id = -1;
    public GameObject objectNeeded;

    [Header("Particle System Settings")]
    public float particleSystemRadius = -1.0f;

    private ParticleSystem initialParticleSystem;

    // Start is called before the first frame update
    void Start() {
        objectNeeded.GetComponent<InteractableObject>().id = id;
        initialParticleSystem = Instantiate(Resources.Load("MissingObjectParticleSystem") as GameObject, gameObject.transform).GetComponent<ParticleSystem>();
        if (particleSystemRadius >= 0) {
            ParticleSystem.ShapeModule shapeModule = initialParticleSystem.shape;
            shapeModule.radius = particleSystemRadius;
        }
        initialParticleSystem.Play();
    }

    //private void OnTriggerEnter(Collider other) {
    //    EvaluateObject(other);
    //}

    private void OnTriggerStay(Collider other) {
        EvaluateObject(other);
    }

     private void EvaluateObject(Collider other) {
        if (other.gameObject.transform.up.y <= 0)
            return;

        if (other.gameObject.CompareTag(objectNeeded.tag)) {
            initialParticleSystem.Stop();

            Destroy(other.gameObject);

            FMOD.Studio.EventInstance sound = RuntimeManager.CreateInstance("event:/SFX/Achievement");
            sound.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
            sound.start();

            Instantiate(Resources.Load("FoundObjectParticleSystem") as GameObject, gameObject.transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();
            GameObject newObject = Instantiate(objectNeeded, gameObject.transform.parent);
            newObject.transform.position = gameObject.transform.position;
            newObject.transform.forward = gameObject.transform.forward;

            Destroy(gameObject);
        }
    }
}
