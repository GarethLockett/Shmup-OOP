using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script: ParticleSystemDestroy
    Author Gareth lockett
    Version: 1.0
    Description:    Destroys the game object a particle system is attached to when it is no longer emitting particles.
*/

[ RequireComponent( typeof( ParticleSystem ) ) ]
public class ParticleSystemDestroy : MonoBehaviour
{
    // Properties
    private ParticleSystem pSystem;

    // Methods
    private void Start()
    {
        this.pSystem = this.gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if( this.pSystem.IsAlive() == false ) { Destroy( this.gameObject ); }
    }
}
