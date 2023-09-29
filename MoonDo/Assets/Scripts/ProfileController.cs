using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProfileController : MonoBehaviour
{

    public TMPro.TextMeshProUGUI userNameText;
    public TMPro.TextMeshProUGUI introductionText;

    void Start()
    {
        StartCoroutine(UnityWebRequestGet());
    }

    IEnumerator UnityWebRequestGet()
    {
        string url = "http://localhost:8080/user/info?id=1";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();


        if (www.error == null)
        {

            string jsonResponse = www.downloadHandler.text;
            UserInfo userInfo = JsonUtility.FromJson<UserInfo>(jsonResponse);
            Debug.Log(www.downloadHandler.text);

            userNameText.text = "닉네임: " + userInfo.userName;
            introductionText.text = "자기소개: " + userInfo.introduction;
        }
        else
        {
            Debug.Log("Error");
        }
    }
}

[System.Serializable]
public class UserInfo
{
    public string userName;
    public string introduction;
}