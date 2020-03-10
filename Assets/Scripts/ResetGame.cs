using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Script: ResetGame
    Author: Gareth Lockett
    Version: 1.0
    Description:    Super simple script for reloading the current scene.
*/

public class ResetGame : MonoBehaviour
{
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape ) == true ) { SceneManager.LoadScene( this.gameObject.scene.name ); }
    }
}
