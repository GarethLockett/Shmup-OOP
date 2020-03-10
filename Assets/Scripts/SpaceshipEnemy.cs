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

    // Methods
    private void Update()
    {
        switch( this.movementType )
        {
            case EnemyShipMovementType.DOWN_SCREEN:
                // Move this enemy ship down the screen at an even rate. Fire if player is in front of it within an angle range.
                this.transform.position -= this.transform.forward * this.moveSpeed * Time.deltaTime;
                break;
        }

        // Check if off the bottom of the screen. If so, destroy this ship.
        if( this.transform.position.z < this.bottomOfScreenRange ) { Destroy( this.gameObject ); }
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
    
}
