using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public bool isMenuAnimationOn = true;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(LateStart(0.01f));
    }

    public void EnableMenuAnimation()
    {
        if (GameObject.Find("Bird_Anim") != null) GameObject.Find("Bird_Anim").GetComponent<Animator>().SetBool("IsEnabled", true);
        if (GameObject.Find("Title_Anim") != null) GameObject.Find("Title_Anim").GetComponent<Animator>().SetBool("IsEnabled", true);
        isMenuAnimationOn = true;
    }
    
    public void DienableMenuAnimation()
    {
        if (GameObject.Find("Bird_Anim") != null) GameObject.Find("Bird_Anim").GetComponent<Animator>().SetBool("IsEnabled", false);
        if (GameObject.Find("Title_Anim") != null) GameObject.Find("Title_Anim").GetComponent<Animator>().SetBool("IsEnabled", false);
        isMenuAnimationOn = false;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isMenuAnimationOn = SceneData.Instance.GetMenuAnimationState();
        
        if (!isMenuAnimationOn)
        {
            if (GameObject.Find("Bird_Anim") != null) GameObject.Find("Bird_Anim").GetComponent<Animator>().SetBool("IsEnabled", false);
            if (GameObject.Find("Title_Anim") != null) GameObject.Find("Title_Anim").GetComponent<Animator>().SetBool("IsEnabled", false);
        }
    }
}
