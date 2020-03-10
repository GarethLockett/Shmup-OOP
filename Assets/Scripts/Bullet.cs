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
    public AudioClip fireSound;             // Sound that will be played when this bullet is instantiated.

    [HideInInspector] public bool isPlayerBullet;   // Tracks if the player fired this bullet (Otherwise was fired by an enemy ship)

    // Methods
    private void Start()
    {
        // Play the fire sound.
        AudioSource.PlayClipAtPoint( this.fireSound, Camera.main.transform.position );
    }

    private void Update()
    {
        // Move the bullet forwards along its Y axis.
        this.transform.position += this.transform.up * this.moveSpeed * Time.deltaTime;

        // Check if offscreen. If it is, then destroy it.
        if( this.transform.position.z < -GameManager.GetScreenEdges().y || this.transform.position.z > GameManager.GetScreenEdges().y ) { Destroy( this.gameObject ); }
    }
}
