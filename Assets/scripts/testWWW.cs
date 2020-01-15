using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class testWWW : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Upload());

    }

    IEnumerator Upload()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", "foo"));
        formData.Add(new MultipartFormDataSection("score", "56"));

        UnityWebRequest www = UnityWebRequest.Post("https://heyyouplaymygames.com/leaderboard", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

}
