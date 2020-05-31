using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int levelIndex;
    // Start is called before the first frame update
    void Start()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        levelIndex++;
        SceneManager.LoadScene(levelIndex);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(levelIndex);
    }

}
