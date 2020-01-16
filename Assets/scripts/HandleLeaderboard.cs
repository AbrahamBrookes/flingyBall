using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using flingyball;
using UnityEngine.Networking;

public class HandleLeaderboard : MonoBehaviour
{

    public GameObject submitToLeaderboardSign;
    public Text threeDigitName;

    public void submitScore()
    {
        string name = threeDigitName.text;
        int score = gameObject.GetComponent<GameMode>().totalEnemiesKilled;

        // sub mit!
        StartCoroutine(SubmitToServer(name, score));
    }


    IEnumerator SubmitToServer(string name, int score)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", name));
        formData.Add(new MultipartFormDataSection("score", score.ToString()));

        UnityWebRequest www = UnityWebRequest.Post("https://heyyouplaymygames.com/leaderboard", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            submitToLeaderboardSign.GetComponent<Animation>().Play("submitToLeaderboard-slideout");
        }
        else
        {
            submitToLeaderboardSign.GetComponent<Animation>().Play("submitToLeaderboard-slideout");
            UpdateLocalLeaderboard(www.downloadHandler.text);
        }
    }


    IEnumerator GetLeaderboard()
    {

        UnityWebRequest www = UnityWebRequest.Get("https://heyyouplaymygames.com/leaderboard");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            UpdateLocalLeaderboard(www.downloadHandler.text);
        }
    }

    public void cancelSubmit()
    {
        submitToLeaderboardSign.GetComponent<Animation>().Play("submitToLeaderboard-slideout");
    }

    public void UpdateLocalLeaderboard(string scores)
    {
        Debug.Log(scores);
    }

    public void Start()
    {
        StartCoroutine(GetLeaderboard());
    }
}
