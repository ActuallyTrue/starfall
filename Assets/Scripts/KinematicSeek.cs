using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek
{
    
    private readonly StarfallCharacterController _character;
    private readonly Vector3 _targetPos;

    public KinematicSeek(StarfallCharacterController character, Vector3 targetPos)
    {
        _character = character;
        _targetPos = targetPos;
    }
    
    public KinematicSteeringOutput GetSteering()
    {
        var steering = new KinematicSteeringOutput();
        steering.Velocity = _targetPos - _character.GetPosition();
        
        //This always normalizes the velocity vector of seek which probably isn't desired...?
        //Although the alternative would slow the agent down as it approaches the target, which also isn't desired.
        //Leaving this for now.
        steering.Velocity = steering.Velocity.normalized;
        
        //For now, looking is just set to look in the same direction you're walking. This could be overwritten in 
        //the higher level AI controller given some state.
        steering.Rotation = steering.Velocity;

        return steering;
    }
}