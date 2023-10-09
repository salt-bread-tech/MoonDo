using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

[System.Serializable]
public class LoginResponse
{
    public string code; //로그인 응답 코드
    public int userId;
}


public class LoginController : MonoBehaviour
{
    public TMP_InputField userIdText; //ID 입력
    public TMP_InputField userPasswordText; //Password 입력

    public void LoginToHome()
    {
        StartCoroutine(UnityWebRequestPost());
    }

    IEnumerator UnityWebRequestPost()
    {
        string url = "http://localhost:8080/user/login";
        //입력한 id, 비밀번호 JSON 형식으로 저장
        string jsonData = "{\"email\":\"" + userIdText.text + "\",\"password\":\"" + userPasswordText.text + "\"}";

        //post 요청
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            string jsonResponse = www.downloadHandler.text;

            // 서버 응답 LoginResponse로 파싱
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

            if (response.code == "1") 
            {
                PlayerPrefs.SetInt("UserId", response.userId); // UserId PlayerPrefs에 저장
                SceneManager.LoadSceneAsync("Home");
                Debug.Log("로그인 성공, userID :" + response.userId);
            }
            else if (response.code == "2")
            {
                Debug.LogError("로그인 실패: 비밀번호 오류");
            }
            else if (response.code == "3")
            {
                Debug.LogError("로그인 실패: 아이디가 존재하지 않음");
            }
        }
        else
        {
            Debug.LogError("Login failed: " + www.error);
        }
    }
}
