using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoutController : MonoBehaviour
{
    public void Logout()
    {
        //PlayerPrefs.DeleteKey("UserId");
        //PlayerPrefs.DeleteKey("AuthToken");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene("Login");

        Debug.Log("Logout");
    }
}

