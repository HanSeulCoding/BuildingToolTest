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
        Transform filePanel = transform.Find("FileSaveLoadPanel");
        filePanel.gameObject.SetActive(!filePanel.gameObject.active);
    }
}
