using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
public class LoginController : MonoBehaviour
{
    public TMP_InputField userIdText;
    public TMP_InputField userPasswordText;

    public void LoginToHome()
    {
        StartCoroutine(UnityWebRequestPost());
    }

    IEnumerator UnityWebRequestPost()
    {
        string url = "http://localhost:8080/user/login";
        string jsonData = "{\"email\":\"" + userIdText.text + "\",\"password\":\"" + userPasswordText.text + "\"}";

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            string jsonResponse = www.downloadHandler.text;
            
            if (jsonResponse.Contains("1"))
            {
                Debug.Log("로그인 성공");
                SceneManager.LoadSceneAsync("Home");
            }
            else if (jsonResponse.Contains("2"))
            {
                Debug.LogError("로그인 실패: 비밀번호 오류");
            }
            else if (jsonResponse.Contains("3"))
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