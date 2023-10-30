using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PaperInfoRequest
{
    public int userId;
}

[System.Serializable]
public class PaperInfoData
{
    public int problemPaperId;
    public string title;
    public string field;
    public string detailedField;
    public int count;
    public string date;
}

public class PaperInfoController : MonoBehaviour
{
    public GameObject PaperInfoprefab;
    public Transform parentTransform;
    public float yOffset = 150f;
    private float initialYOffset;

    private int userId;
    private List<PaperInfoData> paperInfoList;

    public TMP_Dropdown sortingDropdown;

    private void Start()
    {
        initialYOffset = parentTransform.GetComponent<RectTransform>().anchoredPosition.y;
        userId = PlayerPrefs.GetInt("UserId", -1);

        if (userId != -1)
        {
            Debug.Log("UserId: " + userId);
            StartCoroutine(UnityWebRequestPost());
        }
        else
        {
            Debug.Log("UserId를 찾을 수 없음");
        }

        sortingDropdown.onValueChanged.AddListener(SortPaperInfo);
    }

    private void UpdatePaperInfoUI()
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }

        float currentYOffset = initialYOffset;

        foreach (PaperInfoData responseData in paperInfoList)
        {
            GameObject newUIElement = Instantiate(PaperInfoprefab, parentTransform);
            RectTransform newUIElementRect = newUIElement.GetComponent<RectTransform>();
            newUIElementRect.anchoredPosition = new Vector2(newUIElementRect.anchoredPosition.x, currentYOffset);
            TextMeshProUGUI[] textComponents = newUIElement.GetComponentsInChildren<TextMeshProUGUI>();
            
            if (textComponents.Length >= 4)
            {
                textComponents[0].text = responseData.title;
                textComponents[1].text = responseData.field + " /";
                textComponents[2].text = responseData.detailedField + " /";
                textComponents[3].text = responseData.count.ToString();
            }

            Button button = newUIElement.GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            int problemPaperId = responseData.problemPaperId;
            button.onClick.AddListener(() => OnProblemButtonClicked(problemPaperId));

            currentYOffset -= yOffset;
        }
    }

    private IEnumerator UnityWebRequestPost()
    {
        string url = "http://localhost:8080/paper/info";
        PaperInfoRequest requestData = new PaperInfoRequest
        {
            userId = userId
        };

        string jsonData = JsonUtility.ToJson(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("서버 응답: " + www.downloadHandler.text);
            paperInfoList = new List<PaperInfoData>(JsonHelper.getJsonArray<PaperInfoData>(www.downloadHandler.text));
            SortPaperInfo(0);
        }
        else
        {
            Debug.LogError("서버 요청 실패: " + www.error);
        }
    }

    private void SortPaperInfo(int index)
    {
        if (paperInfoList == null || paperInfoList.Count == 0)
        {
            return;
        }

        switch (index)
        {
        case 0: // 최신 순 (기본임)
            paperInfoList.Sort((a, b) => b.date.CompareTo(a.date));
            break;
        case 1: // 오래된 순
            paperInfoList.Sort((a, b) => a.date.CompareTo(b.date));
            break;
        case 2: // 가나다 순
            paperInfoList.Sort((a, b) => a.title.CompareTo(b.title));
            break;
        }

        UpdatePaperInfoUI();
    }

    public void OnProblemButtonClicked(int problemPaperId)
    {
        if (problemPaperId != -1)
        {
            PlayerPrefs.SetInt("ProblemPaperId", problemPaperId);
            PlayerPrefs.Save();
            Debug.Log("ProblemPaperId : " + problemPaperId);
            SceneManager.LoadScene("ProblemPaper");
        }
        else
        {
            Debug.LogError("버튼 이벤트 오류");
        }
    }

    public static class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
