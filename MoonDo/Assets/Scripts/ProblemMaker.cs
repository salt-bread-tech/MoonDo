using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Text;


[System.Serializable]
public class CreateProblemResponse
{
    public int problemPaperId;
}

public class ProblemMaker : MonoBehaviour
{
    public TMP_InputField fieldText;
    public TMP_InputField detailedFieldText;
    public TMP_InputField categoryText;
    public TMP_InputField countText;
    public Slider difficultySlider;

    private int userId;
    private int problemPaperId;

    public void ProblemMakerToProblemPaper()
    {
        StartCoroutine(UnityWebRequestPost());
    }

    void Start()
    {
        //저장된 UserId값 불러옴
        userId = PlayerPrefs.GetInt("UserId", -1); //기본값 -1 반환
        if (userId != -1)
        {
            Debug.Log("UserId: " + userId);
        }
        else
        {
            Debug.Log("userId를 찾을 수 없음");
        }
    }

    IEnumerator UnityWebRequestPost()
    {

        string url = "http://121.163.89.235:8080/problem/creation";

        //입력값 JSON형식으로 저장
        string jsonData = "{\"userId\":\"" + userId + "\",\"title\":\"" + fieldText.text + "\",\"field\":\"" + fieldText.text + "\",\"detailedField\":\"" + detailedFieldText.text +
            "\",\"category\":\"" + categoryText.text + "\",\"count\":" + countText.text + ",\"difficulty\":" + difficultySlider.value + "}";

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log("서버 응답: " + www.downloadHandler.text);

            // 서버 응답 파싱
            var responseData = JsonUtility.FromJson<CreateProblemResponse>(www.downloadHandler.text);

            if (responseData != null)
            {
                problemPaperId = responseData.problemPaperId;
                PlayerPrefs.SetInt("ProblemPaperId", problemPaperId);
                PlayerPrefs.Save();
                Debug.Log("문제 만들기 성공, 문제지 ID: " + problemPaperId);
                SceneManager.LoadScene("ProblemPaper");
            }
            else
            {
                Debug.LogError("서버 응답에서 문제지 ID를 파싱하는 데 문제가 발생.");
            }
        }
        else
        {
            Debug.LogError("서버 요청 실패: " + www.error);
        }
    }
}
