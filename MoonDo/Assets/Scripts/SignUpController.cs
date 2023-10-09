using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class SignUpController : MonoBehaviour
{
    public TMP_InputField emailText; // 이메일(Id) 입력
    public TMP_InputField passwordText; // Password 입력
    public TMP_InputField confirmPasswordText; // PW 확인 입력
    public TMP_InputField nickNameText; // 닉네임 입력
    public TMP_Text errorText; //비밀번호 에러 표시

    public void SignUpToLogin()
    {

        if (passwordText.text != confirmPasswordText.text)
        {
            // 비밀번호와 비밀번호 확인이 다를 때 에러 메시지를 표시
            errorText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        StartCoroutine(UnityWebRequestPost());
    }

    IEnumerator UnityWebRequestPost()
    {
        string url = "http://localhost:8080/user/register";

        // 입력한 값 JSON 형식으로 저장
        string jsonData = "{\"email\":\"" + emailText.text + "\",\"password\":\"" + passwordText.text + "\",\"nickname\":\"" + nickNameText.text + "\"}";

        // POST 요청 설정
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("서버 응답: " + www.downloadHandler.text);

            if (www.downloadHandler.text == "1")
            {
                Debug.Log("회원가입 성공");
                SceneManager.LoadSceneAsync("Login");
            }
            else if (www.downloadHandler.text == "2")
            {
                Debug.LogError("회원가입 실패: 아이디 중복");
            }

        }
        else
        {
            Debug.LogError("서버 요청 실패: " + www.error);
        }
    }
}
