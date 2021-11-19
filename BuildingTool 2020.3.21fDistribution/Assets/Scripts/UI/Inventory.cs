using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    [HideInInspector]
    public int selectBlockNum;

    private void Awake()
    {
        instance = this;
        selectBlockNum = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetActiveTrue_SelectImg(int index)
    {
        transform.GetChild(index).GetChild(1).gameObject.SetActive(true);
    }
    public void SetActiveFalse_SelectImg(int index)
    {
       transform.GetChild(index).GetChild(1).gameObject.SetActive(false);
    }
    public bool IsActiveTrue_BoxImg(int index)
    {
        if (!transform.GetChild(index).GetChild(0).gameObject.activeSelf)
        {
            RootCanvas.instance.transform.Find("NotBlockType").gameObject.SetActive(true);
            return true;
        }
        return false;
    }
}
