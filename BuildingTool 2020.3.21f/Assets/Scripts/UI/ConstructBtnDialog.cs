using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructBtnDialog : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockInfoPanel;
    private GameObject blockInfoBtn;
    public void ConstructBtnClick()
    {
        blockInfoBtn = GameObject.Find("BlockInfoBtn");
        GameObject.Find("Canvas").transform.Find("BlockInfoPanel").gameObject.SetActive(true);
        blockInfoBtn.SetActive(false);
        
    }
}
