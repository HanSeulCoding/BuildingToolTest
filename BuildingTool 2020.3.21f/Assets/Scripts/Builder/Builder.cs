using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder Instance;
    [Header("ī�޶� �̵� �ӵ�")]
    public float camSpeed = 2f;
    [Header("ī�޶� �����ӵ�")]
    public float camAccel;
    [Header("ī�޶� ȸ�� �ӵ�")]
    public float camRotateSpeed = 10;
    [Header("ī�޶� �� �ӵ�")]
    public float camZommSpeed = 10f;
    [Header("Vertical Speed")]
    public float camVerticalSpeed = 5f;

    private Ray mCameraHitRay = new Ray();

    private int blockSelectIndex = 0;
   

    private void InitSetting()
    {
   
    }
    private void Awake()
    {
        Instance = this;
        InitSetting();

       
    }

    public void AddBlockTypeSelect(int _index)
    {
        blockSelectIndex = _index;
        RootCanvas.instance.SelectBlock(_index);
    }
    public void AddBlock()
    {
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            if (hit.transform.gameObject.layer != 7 && hit.transform.gameObject.layer != 6)
                return;

          //  Debug.Log("Add Block" + hit.transform.name);

            switch (hit.transform.gameObject.layer)
            {
                case 7:
                    WorldGenerator.Instance.AddBlock(blockSelectIndex, hit.transform.gameObject.layer, hit.transform.position, hit.normal, true);
                    break;
                case 6:
                    WorldGenerator.Instance.AddBlock(blockSelectIndex, hit.transform.gameObject.layer, hit.point, hit.normal, true);
                    break;
            }

            //RootCanvas.Instance.SetWorkFlow(WorldGenerator.Instance.kCurrentWorkList, WorldGenerator.Instance.kCurrentFillWorkCount, WorldGenerator.Instance.kCurrentWorkNextIndex - 1);
        }
    }
    Vector2 rotation = Vector2.zero;

    public void BlockClick()
    {
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
  
        RaycastHit hit;

        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            if (hit.transform.gameObject.layer == 7)
            {
                BlockSelectImg.instance.SetActive(true);
                Block block = hit.transform.gameObject.GetComponent<Block>();
                block.isPrintUI = true;            
                
                PlayerManager.instance.mCurrentMode = PlayerManager.Mode.BlockSelect;
                PlayerManager.instance.clickedBlock = hit.transform.gameObject;
                PlayerManager.instance._clickNormal = hit.normal;
                Debug.Log("hit");
            }
            else
            {
                if(PlayerManager.instance.clickedBlock != null)  //block ���� �� Ŭ�� �� 
                { 
                    PlayerManager.instance.clickedBlock.GetComponent<Block>().isPrintUI = false; //block Click UI ��� X 
                    BlockSelectImg.instance.SetActive(false);  //block UI Active false ó��
                    PlayerManager.instance.mCurrentMode = PlayerManager.Mode.Insert; //Mode->Insert mode ����
                }
            }


        }
    }

}
