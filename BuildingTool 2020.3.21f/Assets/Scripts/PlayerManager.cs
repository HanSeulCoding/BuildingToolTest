using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public enum Mode
    {
        Insert, //추가 모드
        Delete,
        BlockSelect  
    }
    [HideInInspector]
    public bool isTranslateScale;

    [HideInInspector]
    public Vector3 mouseOnClickPosition;

    [HideInInspector]
    public Vector3 currentMousePosition;

    [HideInInspector]
    public Vector3 _clickNormal;

    [HideInInspector]
    public Mode mCurrentMode = Mode.Insert;

    [HideInInspector]
    public GameObject clickedBlock;

    private bool mIsLookAtMove;
    
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

    

    private void BlockManageUpdate()
    {
        
            

        if (Input.GetMouseButtonDown(0) == true)
        {
            Builder.Instance.BlockClick();
            //GameObject block = 
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
            }
           
        }
        if(mCurrentMode == Mode.BlockSelect)
        {
            if(isTranslateScale)
            {
                if(clickedBlock != null)
                {
                    Block block = clickedBlock.GetComponent<Block>();
                    block.TranslateScale(mouseOnClickPosition,_clickNormal);
                }
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
