using UnityEngine;

public class TankUserManager : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float turnSpeed = 180f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed;

    private Rigidbody m_Rigidbody;
    
    void Awake(){
        m_Rigidbody = gameObject.transform.GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shoot();
    }

    void Movement(){
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        // transform.Translate(Vector3.forward * forwardMovement);
        m_Rigidbody.MovePosition(m_Rigidbody.position + forwardMovement);

        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnMovement = Quaternion.Euler (0f, turn, 0f);
        // transform.Rotate(Vector3.up * turnMovement);
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnMovement);
    }

    void Shoot(){
   
        if(Input.GetButtonDown("Fire1")){
            Debug.Log("Fire btn");
            GameObject bulletInstance = Instantiate(projectilePrefab,firePoint.position,firePoint.rotation);
            Rigidbody bulletRigidbody = bulletInstance.AddComponent<Rigidbody>();
            //Rigidbody bulletInstance = Instantiate(projectilePrefab,firePoint.position,firePoint.rotation) as Rigidbody;
          // bulletRigidbody.AddForce(firePoint.forward * projectileSpeed);

             // Set the shell's velocity to the launch force in the fire position's forward direction.
            bulletRigidbody.linearVelocity = projectileSpeed * firePoint.forward;

            /*ShellExplosion explosionData = shellInstance.GetComponent<ShellExplosion>();
            explosionData.m_ExplosionForce = m_ExplosionForce;
            explosionData.m_ExplosionRadius = m_ExplosionRadius;
            explosionData.m_MaxDamage = m_MaxDamage;*/
        }
    }
}
