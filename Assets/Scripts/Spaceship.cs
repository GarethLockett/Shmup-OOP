using UnityEngine;

/*
    Script: Spaceship
    Author: Gareth Lockett
    Version: 1.0
    Description:    The parent class that all spaceships are inherited from.
*/

public abstract class Spaceship : MonoBehaviour
{
    // Properties
    public int hitPoints = 10;                          // Amount of hit points (or health) this ship has.
    public float moveSpeed = 100f;                      // Speed at which this ship moves at (Units per second)

    public Bullet bulletPrefab;                         // Reference to a prefab that will get instantiated into the scene when the ships' Fire() method is called.
    public float rateOfFire = 0.25f;                    // How many seconds between when the player can fire (Eg smaller value = faster rate of fire)

    public AudioClip destroyedSound;                    // Reference to an audio clip that will play when this ship is destroyed.
    public ParticleSystem destroyedParticleSystem;      // Reference to the prefab particle system that will be instantiated when this ship is destroyed.

    private float nextFireTime;                         // The next time (in seconds) the player can fire/instantiate a bullet.

    // Methods
    protected virtual void Fire( bool isPlayerFired = false )
    {
        // Sanity checks.
        if( this.bulletPrefab == null ) { return; }            // Check a bullet prefab reference has been assigned.
        if( GameManager.GameIsOver() == true ) { return; }     // No more firing if the game is over.

        // Check if time is less than the set nextFireTime (Eg don't fire if not enough time has passed since last fired)
        if( Time.time < this.nextFireTime ) { return; }
        this.nextFireTime = Time.time + this.rateOfFire;        // Set the next time this ship can fire.

        // Instantiate a bullet game object.
        GameObject bulletGO = GameObject.Instantiate( this.bulletPrefab.gameObject );

        // Position the newly instantiated bullet just in front of this ship.
        bulletGO.transform.position = this.transform.position - ( this.transform.forward * 10f );

        // Have the newly instantiated bullet face away from this ship.
        bulletGO.transform.up = -this.transform.forward;

        // Record if the player fired the bullet.
        bulletGO.GetComponent<Bullet>().isPlayerBullet = isPlayerFired;
    }
    
    public void TakeDamage( int amountOfDamage )
    {
        // Do the damage.
        this.hitPoints -= amountOfDamage;

        // Check if hit points is less than or equal to zero.
        if( this.hitPoints <= 0 )
        {
            // Play any assigned destroy sound and instantiate any particle effect.
            this.DestroySoundAndParticles();

            // Execute the ships destroy method (This is different for some ships)
            this.DestroyShip();
        }
    }

    // NOTE: 'virtual' means that this method can be overriden by a child class (Eg the child class can make its own version of this method)
    //       If a child class does not overide this method, then this version is run instead (Or child classes can call base.DestroyShip() to execute this version)
    protected virtual void DestroyShip()
    {
        // Destroy the actual game object.
        Destroy( this.gameObject );
    }

    protected void DestroySoundAndParticles()
    {
        // Play the ships' destroyed sound (If reference assigned and a camera exists) when this ship is destroyed.
        if( this.destroyedSound != null && Camera.main != null )
        {
            // Play the destroyed sound at the location of the camera, so the camera can hear it (Eg may be too far away to be heard if we play it at the ship)
            AudioSource.PlayClipAtPoint( this.destroyedSound, Camera.main.transform.position );
        }

        // Instantiate a particle system (If reference assigned and a camera exists) when this ship is destroyed.
        if( this.destroyedParticleSystem != null && Camera.main != null )
        {
            GameObject particleSystemGameObject = GameObject.Instantiate( this.destroyedParticleSystem.gameObject );

            // Position the newly instantiated particle system at the same position as this ship.
            particleSystemGameObject.transform.position = this.transform.position;
        }
    }

    // NOTE: 'abstract' means that child classes MUST implement their own version of this method.
    public abstract bool HitByBullet( Bullet bullet );

    private void OnTriggerEnter( Collider other )
    {
        //Debug.Log( "TriggerEnter: " +this.name +" hit " +other.name );

        // Find the types that are colliding.
        bool isThisPlayer = this is SpaceshipPlayer;
        SpaceshipEnemy enemyShip = other.gameObject.GetComponent<SpaceshipEnemy>();
        
        // Check if this is an enemy ship colliding with another enemy ship. If so, ignore the collision.
        if( this is SpaceshipEnemy == true && enemyShip != null ) { return; }

        // Check to see if this ship is colliding with a bullet.
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        if( bullet != null )
        {
            if( this.HitByBullet( bullet ) == true )
            {
                // The bullet hit the ship. Bullet no longer needed so can be destroyed here.
                Destroy( bullet.gameObject );
            }

            // Nothing else to do with the bullet.
            return;
        }

        // Check if this is a player ship colliding with an enemy ship then destroy both ships.
        if( isThisPlayer == true && enemyShip != null )
        {
            // Destroy the enemy ship.
            enemyShip.DestroySoundAndParticles();
            enemyShip.DestroyShip();

            // Destroy the player ship.
            this.DestroySoundAndParticles();
            this.DestroyShip();
        }
    }
}
