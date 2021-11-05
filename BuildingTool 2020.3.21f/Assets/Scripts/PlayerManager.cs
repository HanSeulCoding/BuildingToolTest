using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    enum Mode
    {
        Insert, //추가 모드
        Delete,
        BlockSelect  
    }

    public bool isTranslateScale;
    public Vector3 mouseOnClickPosition;
    public Vector3 currentMousePosition;

    private Mode mCurrentMode = Mode.Insert;
    private bool mIsLookAtMove;
    private GameObject blockObj;
    private Ray mCameraHitRay = new Ray();
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mIsLookAtMove = false;
    }
    private void Update()
    {
        //BuilderMoveUpdate();

        if (Input.GetKeyDown(KeyCode.Insert) == true)
            mCurrentMode = Mode.Insert;
        //if (Input.GetKeyDown(KeyCode.Delete) == true)
        //    mCurrentMode = Mode.Delete;


        BlockManageUpdate();
        BlockTypeSelect(); //블럭 타입 선택
    }

    private GameObject BlockClick()
    {
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
     
        RaycastHit hit;
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            if(hit.transform.gameObject.layer == 7)
            {
                BlockSelectImg.instance.SetActive(true);
                Block block = hit.transform.gameObject.GetComponent<Block>();
                block.isPrintUI = true;
                mCurrentMode = Mode.BlockSelect;
                blockObj = hit.transform.gameObject;
            }
            Debug.Log("hit");
        }
        return blockObj;
    }

    private void BlockManageUpdate()
    {
        
            

        if (Input.GetMouseButtonDown(0) == true)
        {
            GameObject block = BlockClick();
            switch (mCurrentMode)
            {
                //case Mode.Delete:
                //    {
                //        Builder.Instance.RemoveBlock();
                //    }
                //    break;
                case Mode.Insert:
                    {
                        Builder.Instance.AddBlock();
                    }
                    break;
                case Mode.BlockSelect:
                    {
                       // if (Input.GetMouseButton(0))

                    }
                    break;
            }
           
        }
    }
    private void BlockTypeSelect()
    {
        if (Input.GetKey(KeyCode.Alpha1) == true)
            Builder.Instance.AddBlockTypeSelect(0);
        if (Input.GetKey(KeyCode.Alpha2) == true)
            Builder.Instance.AddBlockTypeSelect(1);
        if (Input.GetKey(KeyCode.Alpha3) == true)
            Builder.Instance.AddBlockTypeSelect(2);
        if (Input.GetKey(KeyCode.Alpha4) == true)
            Builder.Instance.AddBlockTypeSelect(3);
        if (Input.GetKey(KeyCode.Alpha5) == true)
            Builder.Instance.AddBlockTypeSelect(4);
        if (Input.GetKey(KeyCode.Alpha6) == true)
            Builder.Instance.AddBlockTypeSelect(5);
        if (Input.GetKey(KeyCode.Alpha7) == true)
            Builder.Instance.AddBlockTypeSelect(6);
        if (Input.GetKey(KeyCode.Alpha8) == true)
            Builder.Instance.AddBlockTypeSelect(7);
        if (Input.GetKey(KeyCode.Alpha9) == true)
            Builder.Instance.AddBlockTypeSelect(8);
        if (Input.GetKey(KeyCode.Alpha0) == true)
            Builder.Instance.AddBlockTypeSelect(9);
    }
   
}
