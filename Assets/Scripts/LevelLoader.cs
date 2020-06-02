using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int levelIndex;
    [SerializeField] float timeToDelay = 2f;

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
        SceneManager.LoadScene(levelIndex + 1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(levelIndex);
    }

}
