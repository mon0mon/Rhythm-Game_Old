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

    public Ingame_Charactor_Animation_Controller Actor_Player;
    public Ingame_Charactor_Animation_Controller Actor_NonPlayer;

    private Ingame_Charactor_Animation_Controller Mammoth;
    private Ingame_Charactor_Animation_Controller Babarian;
    private Ingame_Warnning_Indicator_Controller Player_Hit_Indicator;
    private ScoreBarAnimController ScoreBarAnimController;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage_StoneAge" :
                ActiveScene = SceneList.StoneAge;
                Mammoth = GameObject.Find("Mammoth").GetComponent<Ingame_Charactor_Animation_Controller>();
                Babarian = GameObject.Find("Babarian").GetComponent<Ingame_Charactor_Animation_Controller>();
                Player_Hit_Indicator = GameObject.Find("Player_Warnning_Indicator")
                    .GetComponent<Ingame_Warnning_Indicator_Controller>();
                Actor_Player = Babarian;
                Actor_NonPlayer = Mammoth;
                ScoreBarAnimController = GameObject.Find("Boss_HP_Indicator").GetComponent<ScoreBarAnimController>();
                break;
            case "Stage_MiddleAge" :
                ActiveScene = SceneList.MiddleAge;
                break;
            case "Stage_ModernAge" :
                ActiveScene = SceneList.ModernAge;
                break;
            case "Stage_SciFi" :
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
            
            case AnimState.PlayerMiss :
                
                switch (ActiveScene)
                {
                    case SceneList.StoneAge :
                        CntAnimState = AnimState.PlayerMiss;
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
            
            case AnimState.BossDie :
                
                switch (ActiveScene)
                {
                    case SceneList.StoneAge :
                        CntAnimState = AnimState.BossDie;
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
                
            case AnimState.Default :
                
                switch (ActiveScene)
                {
                    case SceneList.StoneAge :
                        CntAnimState = AnimState.Default;
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
                            Player_Hit_Indicator.PlayHitAnim();
                            Mammoth.TriggerDamage();
                            ScoreBarAnimController.PlayOnChangeScoreBar();
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
                            Mammoth.SetDefault();
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
                            ScoreBarAnimController.PlayOnChangeScoreBar();
                        }
                        else
                        {
                            Debug.Log("Mammoth or Babarian not Assigned");
                        }
                        break;
                    case AnimState.PlayerMiss :
                        Babarian.SetDefault();
                        break;
                    case AnimState.BossDie :
                        Mammoth.TriggerDie();
                        break;
                    case AnimState.Default :
                        Babarian.SetDefault();
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
    NULL, 
    PlayerAttack, PlayerDamaged, PlayerDodge, PlayerMiss, BossDie, Default
}
