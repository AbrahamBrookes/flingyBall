using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class leaderboard_no_btn : MonoBehaviour
{

    public GameObject submitToLeaderboardSign;

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("no btn click");
        submitToLeaderboardSign.GetComponent<Animation>().Play("submitToLeaderboard-slideout");
    }
}
