using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePositionSave //to save transform positions for gameobject
{
    public string name; //name of game object
    public string Scene; //which scene the object is on
    public Vector3 newPosition; //which position to be saved to
    public string newDirection; //which direction they should be facing (not yet implemented)
}
