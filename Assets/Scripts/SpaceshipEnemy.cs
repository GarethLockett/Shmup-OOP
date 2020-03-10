using UnityEngine;

/*
    Script: SpaceshipEnemy
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for the enemy spaceships. Inherits from Spaceship.
*/

public class SpaceshipEnemy : Spaceship
{
    // Enumerators
    public enum EnemyShipMovementType { DOWN_SCREEN, ACROSS_SCREEN, TOWARD_PLAYER }

    // Properties
    public EnemyShipMovementType movementType;      // Type of movement this enemy ship makes.
    public int scoreValue = 10;                     // Amount the score increases when a players' bullet destroys this enemy ship.
    public float bottomOfScreenRange = -120f;       // When this enemy ship Z position is BELOW this value, it will destroy itself (Eg set so is offscreen)

    public float randomFireTime = 3f;               // Will try and fire a bullet at a random time between last fire time and randomFireTime.

    private float startXOffset;                     // Record where this ship starts in the X axis (Used by enemy ships moving across the screen)
    private bool movingLeftToRight = true;          // Track direction of movement so can drop down.
    private float lastMoveDirection;                // Track last direction of movement to detect direction change for drop down.

    // Methods
    private void Start()
    {
        // Set the first fire time.
        this.nextFireTime = Time.time + Random.Range( 0f, randomFireTime );

        // Set the bullet type as not a player.
        this.isPlayer = false;

        // Record this ships starting X offset position.
        this.startXOffset = this.transform.position.x;
    }

    private void Update()
    {
        // Do this enemys movement type.
        switch( this.movementType )
        {
            case EnemyShipMovementType.DOWN_SCREEN:
                // Move this enemy ship down the screen at an even rate. Fire if player is in front of it within an angle range.
                this.transform.position -= this.transform.forward * this.moveSpeed * Time.deltaTime;
                break;

            case EnemyShipMovementType.ACROSS_SCREEN:
                // Move this enemy ship across the screen at an even rate.
                Vector3 position = this.transform.position;
                float scaleHowFarSideToSideToMove = this.maximumMoveRange * 0.3f;
                float moveDirection = Mathf.Sin( Time.time * this.moveSpeed );
                position.x = ( moveDirection * scaleHowFarSideToSideToMove ) + this.startXOffset;

                // Check for move direction change.
                if( this.movingLeftToRight == true && moveDirection - this.lastMoveDirection < 0f )
                {
                    position.z -= 10f;
                    this.movingLeftToRight = false; // Moving right to left.
                }
                else if( this.movingLeftToRight == false && moveDirection - this.lastMoveDirection > 0f )
                {
                    position.z -= 10f;
                    this.movingLeftToRight = true; // Moving left to right.
                }

                // Update the last move direction and this ships position.
                this.lastMoveDirection = moveDirection;
                this.transform.position = position;
                break;

            case EnemyShipMovementType.TOWARD_PLAYER:
                // Move this enemy ship towards the players current position.
                SpaceshipPlayer playerShip = GameObject.FindObjectOfType<SpaceshipPlayer>(); // Find the SpaceshipPlayer component in the scene (Note: will return the first one found!)
                if( playerShip != null )
                {
                    if( playerShip.transform.position.z < this.transform.position.z ) // ONLY move this ship towards the player if the player ship is BELOW this ship on screen.
                    {
                        // Move towards the players ship.
                        this.transform.position += ( playerShip.transform.position - this.transform.position ).normalized * this.moveSpeed * Time.deltaTime;
                    }
                }
                this.transform.position -= this.transform.forward * this.moveSpeed * Time.deltaTime; // Keep moving down the screen (So player can dodge them)
                break;
        }

        // Check for firing.
        if( Time.time >= this.nextFireTime )
        {
            // Fire (Note: Rate of fire will still dictate if will fire or not)
            this.Fire();

            // Set the next fire time.
            this.nextFireTime = Time.time + Random.Range( 0f, randomFireTime );
        }

        // Check if off the bottom of the screen. If so, destroy this ship.
        if( this.transform.position.z < this.bottomOfScreenRange )
        {
            this.destroyedSound = null; // Remove any destroy sound.
            this.destroyedParticleSystem = null; // Remove any particle system.
            Destroy( this.gameObject );
        }
    }

    public override void TakeDamage( int amountOfDamage )
    {
        // First run the parent class TakeDamage method.
        base.TakeDamage( amountOfDamage );

        // Check if this ship is destroyed.
        if( this.hitPoints <= 0 )
        {
            // Increase the score.
            GameManager.instance.AddScore( this.scoreValue );
        }
    }

    private void OnTriggerEnter( Collider collider )
    {
        // Check if another enemy is hitting this enemy.
        SpaceshipEnemy otherEnemy = collider.gameObject.GetComponent<SpaceshipEnemy>();
        if( otherEnemy != null ) { return; } // If another enemy is hitting this enemy then do nothing.

        // Check if a bullet is hitting this enemy ship.
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if( bullet != null )
        {
            // Check if it is a player bullet hitting this enemy. If so, do damage.
            if( bullet.isPlayerBullet == true )
            {
                // Do damage to this enemy ship.
                this.TakeDamage( bullet.damage );
            }

            // Destroy the bullet game object.
            Destroy( bullet.gameObject );
        }
    }
}
