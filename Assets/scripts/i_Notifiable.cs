using UnityEngine;

/**
 * 
 * A class implementing i_Notifiable will be able to be 'Notified' of events from other gameobjects
 * 
 * */

public interface i_Notifiable
{
    void Notify(string notification, GameObject other);
}
