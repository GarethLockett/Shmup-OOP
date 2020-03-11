using UnityEngine;

/*
    Script: SpaceshipEnemy
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for the enemy spaceships. Inherits from Spaceship.
*/

public class SpaceshipEnemy : Spaceship
{
    // Properties
    public int scoreValue = 10;             // Amount the score increases when a players' bullet destroys this enemy ship.
    public float randomFireTime = 5f;       // Will randomly generate time to try and fire again between 1 second and this, since last fired.
    public Vector2 randomSpeedRange;        // Randomizes the move speed when starting and recycling this ship.

    private int originalHitPoints;          // Used when reseting this ship to the top of the screen after it has been destroyed.
    private float nextTimeToTryFiring;      // Time (in seconds) to try to fire again.

    // Methods
    private void Start()
    {
        // Record the original hit points so can reset when moving to the top of the screen.
        this.originalHitPoints = this.hitPoints;

        // Randomize the starting speed.
        this.moveSpeed = Random.Range( this.randomSpeedRange.x, this.randomSpeedRange.y );

        // Set the first random fire time for this ship.
        this.nextTimeToTryFiring = Time.time + Random.Range( 1f, this.randomFireTime );
    }

    private void Update()
    {
        // Move this enemy ship down the screen at an even rate.
        this.transform.position -= this.transform.forward * this.moveSpeed * Time.deltaTime;

        // Check for random firing time.
        if( Time.time >= this.nextTimeToTryFiring )
        {
            this.Fire();

            // Set the next random fire time for this ship.
            this.nextTimeToTryFiring = Time.time + Random.Range( 1f, this.randomFireTime );
        }

        // Check if this ship is off the bottom of the screen. If so, recycle the ship to the top of the screen.
        if( this.transform.position.z < -GameManager.GetScreenEdges().y ) { this.RecycleShip(); }
    }

    public override bool HitByBullet( Bullet bullet )
    {
        // Check if an enemy fired bullet. If so, ignore.
        if( bullet.isPlayerBullet == false )
        {
            return false;
        }

        // Do some damage.
        this.TakeDamage( bullet.damage );
        
        // Return true, indicating the bullet hit this ship.
        return true;
    }

    protected override void DestroyShip()
    {
        // If this ship gets 'destroyed' here, award the player some points before recycling this enemy ship.
        GameManager.AddToScore( this.scoreValue );

        // Recycle the ship (Instead of destroying its game object!)
        this.RecycleShip();
    }

    private void RecycleShip()
    {
        // Recycle this ship to the top of the screen instead of destroying it.
        Vector3 position = this.transform.position;
        position.z = GameManager.GetScreenEdges().y;
        position.x = Random.Range( -GameManager.GetScreenEdges().x, GameManager.GetScreenEdges().x );
        this.transform.position = position;

        // Randomize the starting speed.
        this.moveSpeed = Random.Range( this.randomSpeedRange.x, this.randomSpeedRange.y );

        // Reset hit points.
        this.hitPoints = this.originalHitPoints;
    }
}
