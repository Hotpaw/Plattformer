using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private Animator sceneChange;
    private string scene;

    public static SceneChanger instance;

    private void Start()
    {
        instance = this;
        sceneChange = GetComponent<Animator>();
    }

    public void TransitionToNewScene(string newScene)
    {
        scene = newScene;
        sceneChange.SetTrigger("Transition");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }
}
