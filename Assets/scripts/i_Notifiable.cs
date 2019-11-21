using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * A class implementing i_Notifiable will be able to be 'Notified' of events from other gameobjects
 * 
 * */

public abstract class i_Notifiable : MonoBehaviour
{
    public abstract void Notify(string notification, GameObject other);
}
