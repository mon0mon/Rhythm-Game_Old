using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimationManager : MonoBehaviour
{
    private static SceneAnimationManager instance;
    
    public Animator SceneTransition;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneTransition = GameObject.Find("Crossfade").GetComponent<Animator>();
    }

    public static SceneAnimationManager Instance => instance;

    public void StartTransition()
    {
        SceneTransition.SetTrigger("Start");
    }
    
    public void EndTransition()
    {
        SceneTransition.SetTrigger("End");
    }
}
