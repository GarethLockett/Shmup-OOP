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
    public float maximumMoveRange = 170f;               // Maximum distance either left or right this ship can move (Match this to the camera FOV)

    public Bullet bulletPrefab;                         // Reference to a prefab that will get instantiated into the scene when the ships' Fire() method is called.
    public float rateOfFire = 0.25f;                    // How many seconds between when the player can fire (Eg smaller value = faster rate of fire)

    public AudioClip destroyedSound;                    // Reference to an audio clip that will play when this ship is destroyed.
    public ParticleSystem destroyedParticleSystem;      // Reference to the prefab particle system that will be instantiated when this ship is destroyed.

    protected bool isPlayer;                            // Set by SpaceshipPlayer if true.

    private float nextFireTime;                         // The next time (in seconds) the player can fire/instantiate a bullet.

    // Methods
    protected virtual void OnDestroy() // This 'virtual' version of OnDestroy() will be called by all ships when they are destroyed, unless overriden in a child class.
    {
        // Play the ships' destroyed sound (If reference assigned) when this ship is destroyed.
        if( this.destroyedSound != null )
        {
            // Play the destroyed sound at the location of the camera, so the camera can hear it (Eg may be too far away to be heard if we play it at the ship)
            AudioSource.PlayClipAtPoint( this.destroyedSound, Camera.main.transform.position );
        }

        // Instantiate a particle system (If reference assigned) when this ship is destroyed.
        if( this.destroyedParticleSystem != null )
        {
            GameObject particleSystemGameObject = GameObject.Instantiate( this.destroyedParticleSystem.gameObject );

            // Position the newly instantiated particle system at the same position as this ship.
            particleSystemGameObject.transform.position = this.transform.position;
        }
    }

    protected virtual void Fire()
    {
        // Sanity check.
        if( this.bulletPrefab == null ) { return; } // Check a bullet prefab reference has been assigned.

        // Check if time is less than the set nextFireTime (Eg don't fire if not enough time has passed since last fired)
        if( Time.time < this.nextFireTime ) { return; }
        this.nextFireTime = Time.time + this.rateOfFire; // Set the next time the ship can fire.

        // Instantiate a bullet game object.
        GameObject bulletGO = GameObject.Instantiate( this.bulletPrefab.gameObject );

        // Position the newly instantiated bullet just in front of this ship.
        bulletGO.transform.position = this.transform.position - ( this.transform.forward * 10f );

        // Have the newly instantiated bullet face away from this ship.
        bulletGO.transform.up = -this.transform.forward;

        // Mark the newly instantiated bullet as being fired by the player, or not.
        bulletGO.GetComponent<Bullet>().isPlayerBullet = isPlayer;
    }

    public virtual void TakeDamage( int amountOfDamage )
    {
        // Do the damage.
        this.hitPoints -= amountOfDamage;

        // Check if this ship is destroyed.
        if( this.hitPoints <= 0 )
        {
            // Destroy the ship if hit points are less than or equal to zero.
            Destroy( this.gameObject );
        }
    }
}
