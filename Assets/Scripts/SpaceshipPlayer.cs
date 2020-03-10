using UnityEngine;

/*
    Script: SpaceshipPlayer
    Author: Gareth Lockett
    Version: 1.0
    Description:    Script for the players' spaceship. Inherits from Spaceship.
*/

public class SpaceshipPlayer : Spaceship
{
    // Properties
    public KeyCode moveLeftKey = KeyCode.LeftArrow;     // Key the player presses to move the player ship left.
    public KeyCode moveRightKey = KeyCode.RightArrow;   // Key the player presses to move the player ship right.
    public KeyCode fireKey = KeyCode.Space;             // Key the player presses to shoot a bullet.

    // Methods
    private void Update()
    {
        // Check for left/right movement keys.
        if( Input.GetKey( this.moveLeftKey ) == true ) { this.transform.position += this.transform.right * this.moveSpeed * Time.deltaTime; }
        if( Input.GetKey( this.moveRightKey ) == true ) { this.transform.position -= this.transform.right * this.moveSpeed * Time.deltaTime; }

        // Check if the player is firing.
        if( Input.GetKey( this.fireKey ) == true )
        {
            this.Fire( true ); // Passing in 'true' here will mark the bullet as fired from the player (Eg not an enemy)
        }

        // Keep the player ship within the screen horizontal bounds.
        Vector3 position = this.transform.position; // We can't change transform.position.x directly, so must put it in a temporary variable.
        position.x = Mathf.Clamp( position.x, -GameManager.GetScreenEdges().x, GameManager.GetScreenEdges().x );
        this.transform.position = position;           
    }

    public override bool HitByBullet( Bullet bullet )
    {
        // Check if a player fired bullet. If so, ignore.
        if( bullet.isPlayerBullet == true ) { return false; }

        // Do some damage.
        this.TakeDamage( bullet.damage );

        // Update the displayed score and hit points.
        GameManager.UpdateScoreDisplayAndHitPoints();

        // Return true, indicating the bullet hit this ship.
        return true;
    }

    private void OnDestroy()
    {
        // If this player ship gets destroyed for any reason, then notify the GameManager the game is over.
        GameManager.GameOver();
    }
}
