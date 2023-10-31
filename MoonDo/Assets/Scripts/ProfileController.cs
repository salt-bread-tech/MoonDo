using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

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

        string url = "http://localhost:8080/user/info";

        string jsonData = "{\"userId\":" + userId + "}";

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("서버 응답: " + www.downloadHandler.text);
            var responseData = JsonUtility.FromJson<UserInfoResponse>(www.downloadHandler.text);

            if (responseData != null)
            {
                string userName = responseData.userName;
                string introduction = responseData.introduction;
                Debug.Log("아이디 " + userName + " 소개 " + introduction);
                userNameText.text = userName;
                introductionText.text = introduction;
            }
            else
            {
                Debug.LogError("오류");
            }

        }
    }
}
