using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: Bullet
    Author: Gareth Lockett
    Version: 1.0
    Description:    Simple bullet script.
                    Note: Could have made a parent 'projectile' class to inherit from, but keeping this simple for demo.
*/

public class Bullet : MonoBehaviour
{
    // Properties
    public float moveSpeed = 150f;          // Speed at which this bullet will move along its' Y axis (Make sure this is set up correctly with the prefab)
    public int damage = 1;                  // Amount of damage this bullet will do when hitting a ship.

    public float maximumRange = 100f;       // When this bullets Z position is beyond this value, it will destroy itself (Eg set so is offscreen)

    [HideInInspector] public bool isPlayerBullet;             // Tracks if the player fired this bullet (Otherwise was fired by an enemy ship)

    public AudioClip fireSound;             // Sound that will be played when this bullet is instantiated.

    // Methods
    private void Start()
    {
        // Play the fire sound.
        AudioSource.PlayClipAtPoint( this.fireSound, Camera.main.transform.position );
    }

    private void Update()
    {
        // Get the current bullet position (For linecasting)
        Vector3 lastPosition = this.transform.position;

        // Calculate the bullets next position, moving along its' Y axis.
        Vector3 nextPosition = lastPosition + ( this.transform.up * this.moveSpeed * Time.deltaTime );

        // Shoot a ray between the lastPosition and the nextPosition to see if the bullet will hit anything.
        // Details of what was hit will be populated into the 'hit' variable.
        RaycastHit hit;
        if( Physics.Linecast( lastPosition, nextPosition, out hit ) == true )
        {
            // Check this bullet is hitting a ship of some sort.
            Spaceship spaceship = hit.collider.gameObject.GetComponent<Spaceship>();
            if( spaceship != null )
            {
                // Check if the spaceship is a player AND the bullet is NOT a player bullet.
                if( ( spaceship is SpaceshipPlayer == true && this.isPlayerBullet == false ) 
                    || ( spaceship is SpaceshipEnemy == true && this.isPlayerBullet == true ) )
                {
                    // Do damge to the ship that got hit by this bullet.
                    spaceship.TakeDamage( this.damage );

                    // If this bullet has hit a ship, then destroy the bullet.
                    Destroy( this.gameObject );

                    return; // Nothing more needs to happen with this bullet. Return from this method.
                }
            }
            
            // NOTE:    Sometimes this linecast can miss a hit. This is because the ships may move putting the lastPosition AND nextPosition inside a ships collider.
            //          Best to combine linecast with Rigidbody physics - which will catch if this bullet is within a collider.
        }
        else
        {
            // If nothing was hit then move the bullet to the nextPosition.
            this.transform.position = nextPosition;
        }

        // Check if offscreen. If it is, then destroy it.
        if( this.transform.position.z < -this.maximumRange || this.transform.position.z > this.maximumRange ) { Destroy( this.gameObject ); }
    }
}
