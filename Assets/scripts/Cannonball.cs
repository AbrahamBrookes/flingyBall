using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour {

    public GameObject splosion;

    private i_Notifiable shootingAt;

    public float moveTime = 5f;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        inverseMoveTime = 1f / moveTime;
    }

    IEnumerator moveTowardTarget(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);

            transform.position = newPostion;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        splode();

    }

    public void fire(i_Notifiable target)
    {
        shootingAt = target;
        StartCoroutine("moveTowardTarget", target.transform.position);
    }

    public void splode()
    {
        // notify
        if( shootingAt != null )
            shootingAt.Notify("I shot you", gameObject);
        Instantiate(splosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
