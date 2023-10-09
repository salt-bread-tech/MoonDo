using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class UserInfoResponse
{
    public string userName;
    public string introduction;
}

public class ProfileController : MonoBehaviour
{

    public TMPro.TextMeshProUGUI userNameText;
    public TMPro.TextMeshProUGUI introductionText;

    private int userId;

    void Start()
    {
        userId = PlayerPrefs.GetInt("UserId", -1); //기본값 -1 반환
        if (userId != -1)
        {
            Debug.Log("UserId: " + userId);
        }
        else
        {
            Debug.Log("userId를 찾을 수 없음");
        }

        StartCoroutine(UnityWebRequestGet());
    }
    
    IEnumerator UnityWebRequestGet()
    {

         if (userId == -1)
        {
            Debug.LogError("사용자 ID가 유효하지 않습니다.");
            yield break;
        }

        string url = "http://localhost:8080/user/info?id=" + userId;

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();


        if (www.error == null)
        {

            string jsonResponse = www.downloadHandler.text;
            var responseData = JsonUtility.FromJson<UserInfoResponse>(jsonResponse);
            Debug.Log(www.downloadHandler.text);

            userNameText.text = "아이디: " + responseData.userName;
            introductionText.text = "소개: " + responseData.introduction;
        }
        else
        {
            Debug.Log("Error" + www.error);
        }
    }
}

