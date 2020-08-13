using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public SceneList ActiveScene = SceneList.NULL;
    public bool TutorialActive = true;
    public GameObject[] TriggerAction;

    private bool[] TriggerActionChecker;

    // Start is called before the first frame update
    void Start()
    {
        if (TutorialActive)
        {
            switch (ActiveScene)
            {
                case SceneList.NULL :
                    TutorialActive = false;
                    break;
                default :
                    TutorialActive = true;
                    break;
            }
        }
        
        TriggerActionChecker = new bool[TriggerAction.Length];
        for (int i = 0; i < TriggerActionChecker.Length; i++)
        {
            TriggerActionChecker[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialActive)
        {
            
        }
    }
}
