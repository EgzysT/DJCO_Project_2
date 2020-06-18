using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Shader))]
public class WorldChanger : MonoBehaviour {
    public World belongsTo;
    public bool shouldApplyTextures;
    public bool isParticleSystem;
    public bool isLight;
    public bool shouldDisableChild;

    private Pickable pickableObject;

    // Start is called before the first frame update
    void Start() {
        pickableObject = GetComponent<Pickable>();

        // Alter layer at the start
        if (belongsTo == World.ARCANE) {
            if (shouldApplyTextures)
                ChangeAllTextures(true);

            ChangeAllLayers("UninteractiveWorld");
            //gameObject.layer = LayerMask.NameToLayer("UninteractiveWorld");
        }
        else if (belongsTo == World.NORMAL) {
            if (shouldApplyTextures)
                ChangeAllTextures(false);

            ChangeAllLayers("InteractiveWorld");
            //gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
        }
        else {
            ChangeAllLayers("Default");
            //gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
        }

        CheckParticleSystem(belongsTo != World.ARCANE);

        // It only subscribes to events if it is going to change worlds
        if (belongsTo != World.BOTH) {
            GameEvents.instance.onNormalWorldEnter += NormalWorldEnter;
            GameEvents.instance.onArcaneWorldEnter += ArcaneWorldEnter;
        }
    }

    void NormalWorldEnter(Vector3 effectOrigin) {
        float timeUntilEffect = (WorldsController.instance.maxRadius - Vector3.Distance(transform.position, effectOrigin)) / WorldsController.instance.effectSpeed;
        if (gameObject.activeInHierarchy) {
            StartCoroutine(HoldEffect(timeUntilEffect, true));
        }
    }

    void ArcaneWorldEnter(Vector3 effectOrigin) {
        float timeUntilEffect = Vector3.Distance(transform.position, effectOrigin) / WorldsController.instance.effectSpeed;
        if (gameObject.activeInHierarchy) {
            StartCoroutine(HoldEffect(timeUntilEffect, false));
        }
    }

    IEnumerator HoldEffect(float timeToWait, bool isEnteringNormal) {
        yield return new WaitForSeconds(timeToWait);
        if (belongsTo == World.NORMAL) {
            if (isEnteringNormal) {
                // Will appear
                //gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
                ChangeAllLayers("InteractiveWorld");
            }
            else {
                // Will disappear (can change worlds)
                //gameObject.layer = LayerMask.NameToLayer("UninteractiveWorld");
                ChangeAllLayers("UninteractiveWorld");
                CheckForObjectWorldChange(true);
            }
            CheckParticleSystem(isEnteringNormal);
            CheckLight(isEnteringNormal);
        }
        else if (belongsTo == World.ARCANE) {
            if (!isEnteringNormal) {
                // Will appear
                //gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
                ChangeAllLayers("InteractiveWorld");
            }
            else {
                // Will disappear (can change worlds)
                //gameObject.layer = LayerMask.NameToLayer("UninteractiveWorld");
                ChangeAllLayers("UninteractiveWorld");
                CheckForObjectWorldChange(false);
            }
            CheckParticleSystem(!isEnteringNormal);
            CheckLight(!isEnteringNormal);
        }
        // If it belongs to both then the shader does the work
    }

    void CheckForObjectWorldChange(bool belongsToNormal) {
        // Will change worlds if it is a pickable object and the world changer is interacting with it
        if (pickableObject != null && pickableObject.isInteracting) {

            ChangeEntireObjectWorld(belongsToNormal, "InteractiveWorld");

            //Renderer[] allChildren = GetComponentsInChildren<Renderer>();
            //foreach (Renderer childRenderer in allChildren) {
            //    ChangeTexture(childRenderer, belongsToNormal);
            //}

            //ChangeTexture(GetComponent<Renderer>(), belongsToNormal);
            //Texture textParent = GetComponent<Renderer>().material.mainTexture;

            //// If it belongs to the Normal world then it will change to the Arcane world, and vice-versa
            //if (belongsToNormal) {
            //    belongsTo = World.ARCANE;
            //    GetComponent<Renderer>().material = new Material(Shader.Find("Custom/AppearShader"));
            //}
            //else {
            //    belongsTo = World.NORMAL;
            //    GetComponent<Renderer>().material = new Material(Shader.Find("Custom/DisappearShader"));
            //}

            //GetComponent<Renderer>().material.SetTexture("_MainTex", textParent);

            //gameObject.layer = LayerMask.NameToLayer("InteractiveWorld");
        }
    }

    void ChangeEntireObjectWorld(bool belongsToNormal, string layer) {
        //Renderer[] allChildren = GetComponentsInChildren<Renderer>();
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) {
            ChangeTexture(child.GetComponent<Renderer>(), belongsToNormal);
            ChangeLayer(child, layer);
        }

        ChangeTexture(GetComponent<Renderer>(), belongsToNormal);
        ChangeLayer(transform, layer);
    }

    void ChangeAllTextures(bool belongsToNormal) {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) {
            ChangeTexture(child.GetComponent<Renderer>(), belongsToNormal);
        }

        ChangeTexture(GetComponent<Renderer>(), belongsToNormal);
    }

    void ChangeAllLayers(string layer) {

        if (shouldDisableChild) {
            transform.GetChild(0)?.gameObject.SetActive(layer == "InteractiveWorld");
            return;
        }

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren) {
            ChangeLayer(child, layer);
        }

        ChangeLayer(transform, layer);
    }

    private void ChangeTexture(Renderer rend, bool belongsToNormal) {
        if (rend == null)
            return;

        Texture mainTexture = rend.material.GetTexture("_MainTex");
        Texture normalMap = null;
        Texture metallicMap = null;
        Color color = rend.material.GetColor("_Color");

        if(rend.material.HasProperty("_NormalMap"))
            normalMap = rend.material.GetTexture("_NormalMap");

        if (rend.material.HasProperty("_MetallicMap"))
            normalMap = rend.material.GetTexture("_MetallicMap");

        float useNormal = 0f;
        float useMetallic = 0f;
        float metallic = 0f;
        float smoothness = 0f;

        if (rend.material.HasProperty("_UseNormalMap"))
            useNormal = rend.material.GetFloat("_UseNormalMap");
        
        if (rend.material.HasProperty("_UseMetallicMap"))
            useNormal = rend.material.GetFloat("_UseMetallicMap");       
        
        if (rend.material.HasProperty("_Metallic"))
            useNormal = rend.material.GetFloat("_Metallic");        
        
        if (rend.material.HasProperty("_Glossiness"))
            useNormal = rend.material.GetFloat("_Glossiness");

        //Debug.Log(mainTexture);
        //Debug.Log(normalMap);
        //Debug.Log(metallicMap);
        //Debug.Log(metallic);

        // If it belongs to the Normal world then it will change to the Arcane world, and vice-versa
        if (belongsToNormal) {
            belongsTo = World.ARCANE;
            rend.material = new Material(Shader.Find("Custom/AppearShader"));
        }
        else {
            belongsTo = World.NORMAL;
            rend.material = new Material(Shader.Find("Custom/DisappearShader"));
        }

        rend.material.SetTexture("_MainTex", mainTexture);
        rend.material.SetTexture("_NormalMap", normalMap);
        rend.material.SetTexture("_MetallicMap", metallicMap);
        rend.material.SetColor("_Color", color);

        rend.material.SetFloat("_UseNormalMap", useNormal);
        rend.material.SetFloat("_UseMetallicMap", useMetallic);
        rend.material.SetFloat("_Metallic", metallic);
        rend.material.SetFloat("_Glossiness", smoothness);
    }

    private void ChangeLayer(Transform transf, string layer) {
        transf.gameObject.layer = LayerMask.NameToLayer(layer);
    }

    void CheckParticleSystem(bool willAppear) {
        if (!isParticleSystem)
            return;

        if (willAppear) {
            gameObject.GetComponent<ParticleSystem>().Play();
        }
        else {
            gameObject.GetComponent<ParticleSystem>().Stop();
            gameObject.GetComponent<ParticleSystem>().Clear();
        }
    }

    void CheckLight(bool willAppear) {
        if (!isLight)
            return;

        gameObject.GetComponent<Light>().enabled = willAppear;
    }

    void OnDestroy() {
        // Make sure to always unsubscribe the events when the object no longer exists
        if (belongsTo != World.BOTH) {
            GameEvents.instance.onNormalWorldEnter -= NormalWorldEnter;
            GameEvents.instance.onArcaneWorldEnter -= ArcaneWorldEnter;
        }
    }
}
