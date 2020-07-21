using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ingame_Charactor_Animation_Manager : MonoBehaviour
{
    public Animator[] Animators;
    public AnimState CntAnimState = AnimState.NULL;
    public SceneList ActiveScene = SceneList.NULL;

    private Ingame_Charactor_Animation_Controller Mammoth;
    private Ingame_Charactor_Animation_Controller Babarian;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var VARIABLE in Animators)
        {
            Debug.Log(VARIABLE.name);
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage_StoneAge" :
                Debug.Log("Stage_StoneAge");
                ActiveScene = SceneList.StoneAge;
                Mammoth = GameObject.Find("Mammoth").GetComponent<Ingame_Charactor_Animation_Controller>();
                Babarian = GameObject.Find("Babarian").GetComponent<Ingame_Charactor_Animation_Controller>();
                break;
            case "Stage_MiddleAge" :
                Debug.Log("Stage_MiddleAge");
                ActiveScene = SceneList.MiddleAge;
                break;
            case "Stage_ModernAge" :
                Debug.Log("Stage_MordenAge");
                ActiveScene = SceneList.ModernAge;
                break;
            case "Stage_SciFi" :
                Debug.Log("Stage_SciFi");
                ActiveScene = SceneList.SciFi;
                break;
            default :
                Debug.Log("ingame_Chrarctor_Animation_Manager - Start : UnExpected Value");
                ActiveScene = SceneList.NULL;
                break;
        }
    }

    public void GetAction(AnimState state)
    {
        switch (state)
        {
            case AnimState.PlayerAttack :
                
                switch (ActiveScene)
                {
                    case SceneList.StoneAge :
                        CntAnimState = AnimState.PlayerAttack;
                        break;
                    case SceneList.MiddleAge :
                        break;
                    case SceneList.ModernAge :
                        break;
                    case SceneList.SciFi :
                        break;
                    case SceneList.NULL :
                        break;
                }

                break;
            
            case AnimState.PlayerDodge :
                
                switch (ActiveScene)
                {
                    case SceneList.StoneAge :
                        CntAnimState = AnimState.PlayerDodge;
                        break;
                    case SceneList.MiddleAge :
                        break;
                    case SceneList.ModernAge :
                        break;
                    case SceneList.SciFi :
                        break;
                    case SceneList.NULL :
                        break;
                }
                
                break;
            
            case AnimState.PlayerDamaged :
                
                switch (ActiveScene)
                {
                    case SceneList.StoneAge :
                        CntAnimState = AnimState.PlayerDamaged;
                        break;
                    case SceneList.MiddleAge :
                        break;
                    case SceneList.ModernAge :
                        break;
                    case SceneList.SciFi :
                        break;
                    case SceneList.NULL :
                        break;
                }
                
                break;
            default :
                Debug.Log("ingame_Chrarctor_Animation_Manager - GetAction : UnExpected Value");
                return;
        }
        
        AnimationPlay();
    }

    public void AnimationPlay()
    {
        switch (ActiveScene)
        {
            case SceneList.StoneAge :

                switch (CntAnimState)
                {
                    case AnimState.PlayerAttack :
                        if (Mammoth != null && Babarian != null)
                        {
                            Babarian.TriggerAttack();
                            Mammoth.TriggerDamage();
                        }
                        else
                        {
                            Debug.Log("Mammoth or Babarian not Assigned");
                        }
                        break;
                    case AnimState.PlayerDodge :
                        if (Mammoth != null && Babarian != null)
                        {
                            Babarian.TriggerDodge();
                            Mammoth.TriggerAttack();
                        }
                        else
                        {
                            Debug.Log("Mammoth or Babarian not Assigned");
                        }
                        break;
                    case AnimState.PlayerDamaged :
                        if (Mammoth != null && Babarian != null)
                        {
                            Babarian.TriggerDamage();
                            Mammoth.TriggerAttack();
                        }
                        else
                        {
                            Debug.Log("Mammoth or Babarian not Assigned");
                        }
                        break;
                }
                break;
            case SceneList.MiddleAge :
                break;
            case SceneList.ModernAge :
                break;
            case SceneList.SciFi :
                break;
            default :
                break;
        }
    }
}

public enum AnimState
{
    NULL, PlayerAttack, PlayerDamaged, PlayerDodge
}
