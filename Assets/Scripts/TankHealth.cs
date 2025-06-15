using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public float _StartingHealth = 100f;  
    public float _CurrentHealth;  
    private bool _Dead;      

    public ParticleSystem _ExplosionParticles;   
    public AudioSource _ExplosionAudio;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        // When the tank is instantaniated, reset the tank's health and whether or not it's dead.
        _CurrentHealth = _StartingHealth;
        _Dead = false;
        
    }
    
    public void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done.
        _CurrentHealth -= amount;

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (_CurrentHealth <= 0f && !_Dead)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        _Dead = true;
        Debug.Log($"{gameObject.name} est détruit !");

        // Play the particle system of the tank exploding.
        _ExplosionParticles.Play ();

        // Play the tank explosion sound effect.
        _ExplosionAudio.Play();

        gameObject.SetActive(false);
    }

}
