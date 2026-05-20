using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using System.Collections;


public class PetRock : MonoBehaviour
{
    [SerializeField] 
    private float happiness = 100f;

    [SerializeField] 
    private Key feedKey = Key.F;

    [SerializeField] 
    private Key playKey = Key.P;

    [SerializeField] 
    private GameObject treatPrefab;

    [SerializeField] 
    private float treatSpawnForward = 300.35f;

    [SerializeField]
    private float amount = 200f;

    [SerializeField] private float jiggleAmmount = 0.15f;
    [SerializeField] private float jiggleDuration = 0.35f;
    private float jiggleX;
    private float jiggleEnd;

    private float speed => 0.025f * happiness;

    private float floorY = 0f;
    
    void Start()
    {
        floorY = transform.position.y;
        
        StartCoroutine(DecayHappiness());
    }

    void Update()
    {
        var pos = transform.position;
        
        pos.y = floorY + Mathf.Sin(Time.time * speed) * amount * happiness / 100f;

        if (Time.time < jiggleEnd)
        {
            float fade = (jiggleEnd - Time.time) / jiggleDuration;
            pos.x = jiggleX + Mathf.Sin(Time.time * 40f) * jiggleAmmount * fade;
        }
        
        transform.position = pos;
        
        
        
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[playKey].wasPressedThisFrame)
        {
            Play();
        }
        
        if (Keyboard.current[feedKey].wasPressedThisFrame)
        {
            Feed();
        }
    }

    void Play()
    {
        happiness = Mathf.Clamp(happiness + 5, 0, 100);
        
        jiggleX = transform.position.x;
        jiggleEnd = Time.time + jiggleDuration;
    }

    void Feed()
    {
        Vector3 spawnPos = transform.position + transform.forward * treatSpawnForward;
        GameObject treat = Instantiate(treatPrefab, spawnPos, transform.rotation);
        treat.GetComponent<Rigidbody>().AddForce(transform.forward * -500, ForceMode.Impulse);
        treat.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), ForceMode.Impulse);
        Destroy(treat, 5f);
        
        happiness = Mathf.Clamp(happiness + 10, 0, 100);
    }

    IEnumerator DecayHappiness()
    {
        float decayTime = 0.1f;

        var wait = new WaitForSeconds(3);

        while (true)
        {
            yield return wait;
            
            happiness = Mathf.Clamp(happiness - 2f, 0f, 100f);
        }
    }
    
}
