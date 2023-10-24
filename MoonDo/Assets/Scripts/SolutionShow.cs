using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SolutionShow : MonoBehaviour
{
    public Text solutionText;
    public int problemPaperId;

    private void Start()
    {
        StartCoroutine(LoadSolutionData());
    }

    IEnumerator LoadSolutionData()
    {
        string url = "http:/localhost:8080/problem/solution";
        string jsonData = problemPaperId.ToString();

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            string responseJson = www.downloadHandler.text;
            List<string> solutions = JsonUtility.FromJson<List<string>>(responseJson);

            if (solutions != null)
            {
                string combinedSolutionText = string.Join("\n", solutions);
                solutionText.text = combinedSolutionText;
            }
            else
            {
                solutionText.text = "해설지를 찾을 수 없음.";
            }
        }
        else
        {
            Debug.LogError("서버 요청 실패: " + www.error);
        }
    }
}
