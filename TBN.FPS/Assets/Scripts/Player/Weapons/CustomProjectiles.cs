using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Thanks for downloading my custom projectiles script! :D
/// Feel free to use it in any project you like!
/// 
/// The code is fully commented but if you still have any questions
/// don't hesitate to write a yt comment
/// or use the #coding-problems channel of my discord server
/// 
/// Dave

public class CustomProjectiles : MonoBehaviour
{
    public bool activated;

    [Header("Please attatch components:")]
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Header("Set the basic stats:")]
    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;

    [Header("Explosion:")]
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    [Header("Lifetime:")]
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    private int collisions;

    private PhysicMaterial physic_mat;
    public bool alreadyExploded;

    /// Call the setup and attribute functions that need to be called directly at the start
    /// as well as set some variables
    void Start()
    {
        Setup();
    }

    /// Here are all functions called (except Setup), it works always the same,
    /// check if a specific requirement is fullfilled and if so, call the function
    void Update()
    {
        if (!activated) return;

        if (collisions >= maxCollisions && activated) Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0 && activated) Explode();        
    }

    ///Just to set the basic variables of the bullet/projectile
    private void Setup()
    {
        //Setup physics material
        physic_mat = new PhysicMaterial();
        physic_mat.bounciness = bounciness;
        physic_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physic_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Apply the physics material to the collider
        GetComponent<SphereCollider>().material = physic_mat;

        //Don't use unity's gravity, we made our own (to have more control)
        rb.useGravity = useGravity;
    }

    public void Explode()
    {
        GameObject instExplosion;
        rb.velocity = Vector3.zero;

        //Bug fixing
        if (alreadyExploded) return;
        alreadyExploded = true;

        Debug.Log("Explode");

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        //Instantiate explosion if attatched
        if (explosion != null)
        {
           instExplosion = Instantiate(explosion, transform.position, Quaternion.identity);

           Destroy(instExplosion, 1f);
        }

        //Check for enemies and damage them
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            //Damage enemies
            if (enemies[i].GetComponent<Damageable>())
                enemies[i].GetComponent<Damageable>().Hit(explosionDamage);

            //Add explosion force to enemies
            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, 2f);
        }

        Destroy(gameObject, 0.1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!activated) return;        

        //Explode on touch
        if (explodeOnTouch && collision.collider.CompareTag("Enemy")) Explode();

        //Count up collisions
        collisions++;
    }
    ///Just for visualizing a few variables
    #region Debugging
    private void OnDrawGizmosSelected()
    {
        //visualize the explosion range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    #endregion

    /// The setters need to be here that the slider Ui works, 
    /// if you don't need the ingame sliders anyway, just delete them :D
    #region Setters

    public void SetMaxCollisions(float v)
    {
        int _v = Mathf.RoundToInt(v);
        maxCollisions = _v;
    }
    public void SetMaxLifetime(float v)
    {
        int _v = Mathf.RoundToInt(v);
        maxLifetime = _v;
    }
    public void SetExplosionRange(float v)
    {
        explosionRange = v;
    }
    public void SetExplosionDamage(float v)
    {
        int _v = Mathf.RoundToInt(v);
        explosionDamage = _v;
    }

    #endregion
}