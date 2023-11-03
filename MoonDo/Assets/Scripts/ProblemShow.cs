using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System;

[System.Serializable]
public class ResponseObject
{
    public string[] getProblemResponses;
}

public class ProblemShow : MonoBehaviour
{
    public TMPro.TextMeshProUGUI problemText; // UI Text ��ҿ� ���� ����
    int problemPaperId;
    private object jsonArray;

    public void ProblemPaperShow()
    {
        StartCoroutine(UnityWebRequestPost());
    }
    private void Start()
    {
        Debug.Log("ProblemPaperId : " + problemPaperId);
        /*
        problemPaperId = PlayerPrefs.GetInt("ProblemPaperId", -1);
        if(problemPaperId != -1)
        {
            Debug.Log("ProblemPaperId : " + problemPaperId);
        }
        else
        {
            Debug.Log("ProblemPaperId�� ã�� �� ����");
        }
        */
        ProblemPaperShow();
    }

    IEnumerator UnityWebRequestPost()
    {
        string url = "http://localhost:8080/problem/all";
        Debug.Log("��������Ʈ ����");
        if (problemPaperId != -1)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            CreateProblemResponse requestData = new CreateProblemResponse();
            requestData.problemPaperId = PlayerPrefs.GetInt("ProblemPaperId", -1); ; // ����� ���� ���
            string jsonData = JsonUtility.ToJson(requestData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.error == null)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("JSON Response: " + jsonResponse);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    // ���� ǥ������ ����Ͽ� "���� X: "�� ����
                    jsonResponse = Regex.Replace(jsonResponse, @"(^\[|\]$|\\)", "");
                    jsonResponse = jsonResponse.Replace("?", "?\n");
                    jsonResponse = jsonResponse.Replace("����", "\n����");


                    ResponseObject responseData = new ResponseObject();
                    responseData.getProblemResponses = jsonResponse.Split(new char[] { '\"', ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (responseData != null && responseData.getProblemResponses != null)
                    {
                        // ���ڿ� ����� ���� ���ڷ� �����Ͽ� Text UI�� ����
                        string resultText = string.Join("\n", responseData.getProblemResponses);
                        problemText.text = resultText;
                    }
                    else
                    {
                        Debug.Log("Invalid or missing response data");
                    }
                }
                else
                {
                    Debug.Log("Empty response data");
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
        else
        {
            Debug.LogError("ProblemPaperId�� ã�� �� ����");
        }
    }
}