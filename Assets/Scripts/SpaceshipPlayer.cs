using UnityEngine;

/*
    Script: SpaceshipPlayer
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for the players' spaceship. Inherits from Spaceship.
*/


//[ RequireComponent( typeof( Rigidbody ) ) ] // Require a Rigidbody for collisions (Eg with enemy ships etc)
public class SpaceshipPlayer : Spaceship
{
    // Properties
    public KeyCode moveLeftKey = KeyCode.LeftArrow;         // Key the player presses to move the player ship left.
    public KeyCode moveRightKey = KeyCode.RightArrow;       // Key the player presses to move the player ship right.
    public KeyCode fireKey = KeyCode.Space;                 // Key the player presses to shoot a bullet.
    
    // Methods
    private void Start()
    {
        // Set the parent class isPlayer property to true.
        this.isPlayer = true;
    }

    private void Update()
    {
        // Check for left/right movement keys.
        if( Input.GetKey( this.moveLeftKey ) == true ) { this.transform.position += this.transform.right * this.moveSpeed * Time.deltaTime; }
        if( Input.GetKey( this.moveRightKey ) == true ) { this.transform.position -= this.transform.right * this.moveSpeed * Time.deltaTime; }

        // Check the ship is still within range. If not, move it back into range.
        Vector3 position = this.transform.position; // We can't change transform.position.x directly, so must put it in a temporary variable.
        if( position.x > this.maximumMoveRange ) { position.x = this.maximumMoveRange; } // Check if x position is greater than maximum.
        if( position.x < -this.maximumMoveRange ){ position.x = -this.maximumMoveRange; } // Check if x position is less than minimum (Eg negative maximum)
        this.transform.position = position; // Assign the position back to the transform.position.

        // Check if the player is firing.
        if( Input.GetKey( this.fireKey ) == true ) { this.Fire(); }
    }

    private void OnTriggerEnter( Collider collider )
    {
        // Check if the colliding game object has a Bullet component.
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if( bullet != null )
        {
            if( bullet.isPlayerBullet == false )
            {
                // Do damge to this ship.
                this.TakeDamage( bullet.damage );

                // Check if this player has no hit points left.
                if( this.hitPoints <= 0f )
                {
                    // Notify the GameManager the game is over.
                    GameManager.instance.GameOver();
                }

                // Destroy the bullet hitting this ship.
                Destroy( bullet.gameObject );
            }
        }

        // Check if an enemy ship is colliding with this ship.
        SpaceshipEnemy enemyShip = collider.gameObject.GetComponent<SpaceshipEnemy>();
        if( enemyShip != null )
        {
            // Destroy this ship if it collides with an enemy ship.
            this.Destroy();
        }
    }

    private void OnDestroy()
    {
        // If this player ship gets destroyed for any reason, then notify the GameManager the game is over.
        GameManager.instance.GameOver();
    }
}
