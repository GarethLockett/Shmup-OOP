using UnityEngine;

/*
    Script: GameManager
    Author: Gareth Lockett
    Version: 1.0
    Description:    Main game control script.
                    Tracks and displays player score.
*/

public class GameManager : MonoBehaviour
{
    // Properties
    public static GameManager instance;     // A static (Eg globally accessible) reference to this GameManager.
    public TextMesh scoreText;              // Reference to a TextMesh component. This is used to display the score as it changes.
    private int score = 0;                  // The players' score.

    // Methods
    private void Awake()
    {
        // Set the global instance of GameManager to this GameManager component (There should only be 1 in the scene!)
        if( GameManager.instance == null ) { GameManager.instance = this; }
        else { if( GameManager.instance != this ) { Destroy( this ); } } // Will destroy this GameManager component if there is already one set.
    }

    public void AddScore( int scoreAmountToAdd )
    {
        // Add the amount to the existing score.
        this.score += scoreAmountToAdd;

        // Update the displayed score.
        if( this.scoreText != null )
        {
            this.scoreText.text = this.score.ToString( "D8" );
        }
    }
}
