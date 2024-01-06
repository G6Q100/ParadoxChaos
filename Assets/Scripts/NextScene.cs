using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        string path = Application.persistentDataPath + "/paradoxChaos" + "/data.this";
        if (File.Exists(path))
            File.Delete(path);

        SceneManager.LoadScene(1);
        //GoToNextScene();
    }
}
