using UnityEngine;

public class ShellExplosion : MonoBehaviour
{

    public LayerMask _TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
    public ParticleSystem _ExplosionParticles;         // Reference to the particles that will play on explosion.
    public AudioSource _ExplosionAudio;                // Reference to the audio that will play on explosion.
    [HideInInspector] public float _MaxLifeTime = 2f;  // The time in seconds before the shell is removed.

        // All those are hidden in inspector as they will actually come from the TankShooting scripts
    [HideInInspector] public float _MaxDamage = 10f;                    // The amount of force added to a tank at the centre of the explosion.
    [HideInInspector] public float _ExplosionRadius = 5f;   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         // If it isn't destroyed by then, destroy the shell after its lifetime.
        Destroy (gameObject, _MaxLifeTime);
    }

    
    private void OnTriggerEnter (Collider other)
    {
        Debug.Log("Entered Trigger zone");

        Collider[] colliders = Physics.OverlapSphere(transform.position, _ExplosionRadius,_TankMask);

        foreach (Collider nearbyObject in colliders)
        {
            TankHealth tankHealth = nearbyObject.GetComponent<TankHealth>();
            if (tankHealth != null)
            {
                tankHealth.TakeDamage(_MaxDamage);
            }

            // Optionnel : ajouter une force de poussée
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(5f, transform.position, _ExplosionRadius);
            }
        }
          // Unparent the particles from the shell.
            _ExplosionParticles.transform.parent = null;

            _ExplosionParticles.Play();
            ParticleSystem.MainModule  main = _ExplosionParticles.main;
            
            Destroy (_ExplosionParticles.gameObject,main.duration);

            // Play the explosion sound effect.
            _ExplosionAudio.Play();
            Renderer rend = GetComponent<Renderer>();
            rend.enabled = false;

            // Destroy the shell.
            Destroy (gameObject,_ExplosionAudio.clip.length);
    }

        
}
