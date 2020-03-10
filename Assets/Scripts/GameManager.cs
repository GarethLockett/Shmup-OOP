using UnityEngine;

/*
    Script: GameManager
    Author: Gareth Lockett
    Version: 1.0
    Description:    Main game control script.
                    Manages game state (Eg game is either playing or over)
                    Tracks and displays player score.
*/

public class GameManager : MonoBehaviour
{
    // Properties
    private static GameManager instance;        // A static reference to this GameManager (Only used internally within this class)

    public TextMesh scoreText;                  // Reference to a TextMesh component. This is used to display the score as it changes.
    public Vector2 screenRangeSize;             // Set this to around the screen edges (Eg x = width, y = height)

    private bool gameIsOver;                    // Game state (Eg game is only ever playing or over)
    private int score;                          // The players score.

    // Methods
    private void Awake()
    {
        // Set this GameManager as static (Globally accessible)
        GameManager.instance = this;

        // Initialize the player score and hit point display.
        GameManager.UpdateScoreDisplayAndHitPoints();
    }

    public static void AddToScore( int scoreAmountToAdd )
    {
        // Add the amount to the existing score.
        GameManager.instance.score += scoreAmountToAdd;

        // Update the displayed score and hit points.
        GameManager.UpdateScoreDisplayAndHitPoints();
    }

    public static void UpdateScoreDisplayAndHitPoints()
    {
        // Update the displayed score.
        GameManager.instance.scoreText.text = GameManager.instance.score.ToString( "D8" );

        // Show remaining player hit points under score.
        SpaceshipPlayer playerShip = GameObject.FindObjectOfType<SpaceshipPlayer>();
        if( playerShip != null )
        {
            GameManager.instance.scoreText.text += "\n";
            for( int i = 0; i < playerShip.hitPoints; i++ ) GameManager.instance.scoreText.text += "<color=red>♥</color>";
        }
    }

    public static Vector2 GetScreenEdges() { return GameManager.instance.screenRangeSize; }

    public static bool GameIsOver() { return GameManager.instance.gameIsOver; }

    public static void GameOver()
    {
        // Update the displayed score.
        if( GameManager.instance.scoreText != null )
            GameManager.instance.scoreText.text = GameManager.instance.score.ToString( "D8" ) + "\n<color=red>Game Over!</color>";

        // Set the game state as over.
        GameManager.instance.gameIsOver = true;
    }
}
