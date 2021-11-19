using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBtnDialog : MonoBehaviour
{
    // Start is called before the first frame update
    public void Click_SaveBtn()
    {
        GameManager.instance.Save();
    }
    public void Click_Loadbtn()
    {
        GameManager.instance.Load();
    }
    public void Click_FileBtn()
    {
        Transform filePanel = GameObject.Find("BlockFileManagerPanel").transform.Find("FileManager_Panel");
        filePanel.gameObject.SetActive(!filePanel.gameObject.active);
        Transform matPanel = GameObject.Find("BlockFileManagerPanel").transform.Find("MatManager_Panel");
        if (matPanel.gameObject.activeSelf)
            matPanel.gameObject.SetActive(false);

    }
    public void Click_MatBtn()
    {
        Transform matPanel = GameObject.Find("BlockFileManagerPanel").transform.Find("MatManager_Panel");
        matPanel.gameObject.SetActive(!matPanel.gameObject.active);
        Transform filePanel = GameObject.Find("BlockFileManagerPanel").transform.Find("FileManager_Panel");
        if(filePanel.gameObject.activeSelf)
        {
            filePanel.gameObject.SetActive(false);
        }
    }
    public void Click_Color()
    {
        Transform color = GameObject.Find("Canvas").transform.Find("ColorMask");
        color.gameObject.SetActive(!color.gameObject.active);
    }
}
