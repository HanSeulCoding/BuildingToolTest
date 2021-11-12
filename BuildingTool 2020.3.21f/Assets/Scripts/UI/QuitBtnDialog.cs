using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitBtnDialog : MonoBehaviour
{
    // Start is called before the first frame update
    public void ClickQuitBtn()
    {
      
            //RootCanvas.instance.transform.Find("QuitPanel").gameObject.SetActive(true);
        RootCanvas.instance.quitPanel.gameObject.SetActive(true);
 
    }
    public void CancelBtnClick()
    {
       
         RootCanvas.instance.quitPanel.gameObject.SetActive(false);
      
    }
    public void ExitBtnClick()
    {
       
         Application.Quit();
       
    }
}
