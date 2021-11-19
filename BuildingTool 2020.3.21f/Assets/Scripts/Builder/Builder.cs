using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work
{
    public Dictionary<int,List<int>> removeBlockTypeList = new Dictionary<int, List<int>>();
    public Dictionary<int,List<Vector3>> removeBlockPosList = new Dictionary<int, List<Vector3>>();

    public Dictionary<int, List<Block>> removeBlock = new Dictionary<int, List<Block>>();
    public List<Block> addBlockList = new List<Block>();
     
}
public class Builder : MonoBehaviour
{
    public string Version = "1.0.0";

    public Material[] materials;

    public static Builder instance;
    [Header("맵 가로 Size")]
    public int rowSize;
    [Header("맵 세로 Size")]
    public int columnSize;
    [Header("맵 Y축 Size")]
    public int YSize;
    public enum Command
    {
        None,
        Add,        //추가된
        Remove      //삭제된
    }

    [Header("생성 모델1")]
    public GameObject blockModel1;

    private Transform visibleBlock;
    //private GameObject poolingBlockParent;
    
    private Transform addTransform;
    private Transform poolingBlockTransform;

    //private List<Block> poolingBlockList = new List<Block>();

    [HideInInspector]
    public Work work;

    [HideInInspector]
   // public GameObject blockModel;
    public Block blockModel;

    [HideInInspector]
    public GameObject bottomFloor;

    [HideInInspector]
    public bool isBuild;

    [HideInInspector]
    public int undoRedoKey = 0;

    [HideInInspector]
    private int buildCount;

    [HideInInspector]
    public int blockType;

    [HideInInspector]
    public Position clickTempPosition;
    [HideInInspector]
    public Position currentTempPosition;
  

    private void Awake()
    {
        instance = this;
        InitSetting();
        Test();
        
    }
    private void Test()
    {
        //for (int i = 0; i < 100; i++)
        //{
        //    for (int j = 0; j < 100; j++)
        //    {
        //        for (int z = 0; z < 10; z++)
        //        {
        //            Vector3 position = new Vector3(i, j, z);
        //            AddBlock(0, position);
        //        }

        //    }
        //}

    }
    private void InitSetting()
    {
        blockModel = Resources.Load<Block>("Blocks/Block");
        addTransform = transform.Find("Add");
        poolingBlockTransform = transform.Find("PoolingBlock");

        bottomFloor = GameObject.Find("BottomFloor");
        bottomFloor.transform.localScale = new Vector3(rowSize, 1, columnSize);

        visibleBlock = Resources.Load<Transform>("Blocks/VisibleBlock"); //pooling block 만들기\
        visibleBlock = Instantiate(visibleBlock, poolingBlockTransform);
        //visibleBlock = visible.transform.GetChild(0).GetComponent<Block>();
        
        visibleBlock.gameObject.SetActive(false);

        work = new Work();
        for (int i = 0; i < 10; i++)
        {
            work.removeBlockPosList[i] = new List<Vector3>();
            work.removeBlockTypeList[i] = new List<int>();
            work.removeBlock[i] = new List<Block>();
        }
        undoRedoKey = 0;
        SetMaterial();
    }
    private void SetMaterial()
    {
        materials = new Material[5];
        for (int i = 0; i < 5; i++)
            materials[i] = Resources.Load<Material>("Blocks/BlockMaterial/Block" + (i+1) + "Mat");
    }
    // Start is called before the first frame update
    
    //PreBlock 정보 설정
    public void AddVisibleBlock(int _blockSelect, Vector3 clickMousePos, Vector3 currentMousePos)
    {
        if (currentMousePos.x >= 50 || currentMousePos.x < -50
             || currentMousePos.z >= 50 || currentMousePos.z < -50)
        {
            return;
        }
        Position clickPos = TransPosition.instance.TransLocalPosition(clickMousePos);
        Position currentpos = TransPosition.instance.TransLocalPosition(currentMousePos);
        clickTempPosition = clickPos;
        currentTempPosition = currentpos;

        //Debug.Log("current Pos X: " + currentpos.x + "Pos Y : " + currentpos.y + "Pos Z :" + currentpos.z);
        RectMake(clickPos, currentpos);
    }

    //MouseClickposition에서 현재 마우스포지션의 안의 사각형 정보 추출 
    private void RectMake(Position clickPos, Position currentPos)
    {
        int xWidth = currentPos.x - clickPos.x;
        int zHeight = currentPos.z - clickPos.z;


        if (xWidth <= 0 && zHeight <= 0) //오른쪽 위쪽에서 왼쪽 아래쪽으로 드래그
        {
            DragVisibleBlock(clickPos, currentPos,0);
        }
        else if (xWidth <= 0 && zHeight >= 0) // 오른쪽 아래쪽에서 왼쪽 위쪽으로 드래그
        { 
            //X축 - Z축 +
            DragVisibleBlock(clickPos, currentPos, 1);
        }
        else if (xWidth >= 0 && zHeight >= 0) // 왼쪽 아래쪽에서 오른쪽 위쪽으로 드래그
        {
            //X축 + Z축 +
            DragVisibleBlock(clickPos, currentPos, 2);
        }
        else if (xWidth >= 0 && zHeight <= 0) //왼쪽 위쪽에서 오른쪽 아래쪽으로 드래그
        {
            //X축 + Z축 -  
            DragVisibleBlock(clickPos, currentPos, 3);
        }
    }
    //사각형 정보를 토대로 PreBlock 건설 
    private void DragVisibleBlock(Position clickPos, Position currentPos, int flag)
    {

        float distance_X = 1.0f;
        float distance_Z = 1.0f;
        float currentY = 0.0f;


        if (visibleBlock.gameObject.activeSelf == false)
            visibleBlock.gameObject.SetActive(true);

        //if (currentPos.x == clickPos.x || currentPos.z == clickPos.z)
        //    return;
        
        Debug.Log("ClickPos  X : " + clickPos.x + "Y : "+clickPos.y + "Z : "+clickPos.z);
        Debug.Log("CurrentPos  X : " + currentPos.x + "Y : " + currentPos.y + "Z : " + currentPos.z);
        Vector3 minus = new Vector3((float)0.5, (float)0.0, (float)0.5);

        switch (flag)
        {
            case 0:
                distance_X = currentPos.x - clickPos.x;
                distance_Z = currentPos.z - clickPos.z;
                break;
            case 1:
                distance_X = currentPos.x - clickPos.x;
                distance_Z = Mathf.Abs(clickPos.z - currentPos.z);
                break;
            case 2:
                distance_X = Mathf.Abs(currentPos.x - clickPos.x);
                distance_Z = Mathf.Abs(clickPos.z - currentPos.z);
                break;
            case 3:
                distance_X = Mathf.Abs(currentPos.x - clickPos.x);
                distance_Z = currentPos.z - clickPos.z;
                break;
        }

        if (clickPos.y == -1)
        {
            clickPos.y = 0;
        }
        if (clickPos.y > currentPos.y)
            currentY = currentPos.y * -1;
        else
            currentY = currentPos.y;
        visibleBlock.transform.position = TransPosition.instance.TransWorldPosition(clickPos) - minus;
        visibleBlock.transform.localScale = new Vector3(distance_X, currentY + 1, distance_Z);

        //Debug.Log("VisibleBlockPos : " + visibleBlock.transform.position);
    }
    public void DragBuildBlock()
    {
        if (clickTempPosition.x < currentTempPosition.x)
        {
            for (int i = clickTempPosition.x; i < currentTempPosition.x; i++)
            {
                if(clickTempPosition.z > currentTempPosition.z)
                {
                    for (int j = currentTempPosition.z; j < clickTempPosition.z; j++)
                    {
                        if (clickTempPosition.y <= currentTempPosition.y)
                        {
                            for (int z = clickTempPosition.y; z <= currentTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                        if(clickTempPosition.y > currentTempPosition.y)
                        {
                            for (int z = currentTempPosition.y; z <= clickTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
              
                        
                    }
                }
                if(clickTempPosition.z < currentTempPosition.z)
                {
                    for (int j = clickTempPosition.z; j < currentTempPosition.z; j++)
                    {
                        if (clickTempPosition.y <= currentTempPosition.y)
                        {
                            for (int z = clickTempPosition.y; z <= currentTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                        if (clickTempPosition.y > currentTempPosition.y)
                        {
                            for (int z = currentTempPosition.y; z <= clickTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                    }
                }
            }
        }
        if(clickTempPosition.x > currentTempPosition.x)
        {
            for (int i = currentTempPosition.x; i < clickTempPosition.x; i++)
            {
                if (clickTempPosition.z > currentTempPosition.z)
                {
                    for (int j = currentTempPosition.z; j < clickTempPosition.z; j++)
                    {
                        if (clickTempPosition.y <= currentTempPosition.y)
                        {
                            for (int z = clickTempPosition.y; z <= currentTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                        if (clickTempPosition.y > currentTempPosition.y)
                        {
                            for (int z = currentTempPosition.y; z <= clickTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                    }
                }
                if (clickTempPosition.z < currentTempPosition.z)
                {
                    for (int j = clickTempPosition.z; j < currentTempPosition.z; j++)
                    {
                        if (clickTempPosition.y <= currentTempPosition.y)
                        {
                            for (int z = clickTempPosition.y; z <= currentTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                        if (clickTempPosition.y > currentTempPosition.y)
                        {
                            for (int z = currentTempPosition.y; z <= clickTempPosition.y; z++)
                            {
                                Block block = AddBlock(blockType, new Vector3(i, z, j));
                            }
                        }
                    }
                }
            }
        }

    }

    public void VisibleBlockFalse()
    {
        visibleBlock.gameObject.SetActive(false);
    }
    public void ClickBuildBlock(Vector3 _normal)
    {
        if (_normal.y == 1.0f)
            _normal.y = 0;
        if (_normal.x == 1.0f)
            _normal.x = 0.1f;
        if (_normal.z == 1.0f)
            _normal.z = 0.1f;

        Block blockGo;
        Vector3 tempMousepos = TransPosition.instance.TranslatePosition(InputBlockPos.Instance.mouseOnClickPosition + _normal);
        int tempY;

        //Debug.Log("TempMousePos X :" + tempMousepos.x + "Y :" + tempMousepos.y + "Z : " + tempMousepos.z);
        if ((tempMousepos.x + _normal.x) > 50 || (tempMousepos.x + _normal.x) < -50
            || (tempMousepos.z + _normal.z) > 50 || (tempMousepos.z + _normal.z) < -50)
            return;

        blockGo = AddBlock(blockType, tempMousepos);

        tempY = (int)InputBlockPos.Instance.mouseOnClickPosition.y; //0.0이지만 Local변경시 Local Pos 가 -1로 되는 이상한 현상으로 인해 0일떄는 무조건                                                //0이 되게 수정 
        if (tempY == 0)                                       //0이 되게 수정 
            tempMousepos.y = 0.0f;

        work.addBlockList.Add(blockGo); //workList에 추가
    }

   
    public Block AddBlock(int blockType, Vector3 pos)
    {
        Block blockGo = Instantiate(blockModel, addTransform);
        blockGo.transform.position = pos;
        blockGo.blockType = blockType;
        work.addBlockList.Add(blockGo);
        return blockGo;

    }

    public void DragDeleteBlock()
    {
        //이진탐색으로 최적화 해야함
        // int count = 0;
        //for (int i = poolingBlockList.Count - 1; i >= 0; i--)
        //{
        //    if (!poolingBlockList[i].gameObject.activeSelf)
        //        continue;
        //    if (!poolingBlockList[i].isVisible)
        //        continue;   
        //    if (poolingBlockList[i].gameObject.activeSelf == true)
        //    {
        //        for (int j = work.addBlockList.Count - 1; j >= 0; j--)
        //        {
        //            Block block = work.addBlockList[j];

        //            if (poolingBlockList[i].transform.position == block.transform.position)
        //            {
        //                Destroy(block.gameObject);
        //                work.addBlockList.Remove(work.addBlockList[j]);
        //                poolingBlockList[i].gameObject.SetActive(false);
        //                poolingBlockList[i].isVisible = false;

        //            }
        //        }
        //    }
        //}
        //foreach (Block poolingBlock in poolingBlockList)
        //{
        //    if (!poolingBlock.gameObject.activeSelf)
        //        continue;

        //    if (!poolingBlock.isVisible) //보이는 PoolingBlock 아닐 경우
        //        continue;

        //    if (poolingBlock.gameObject.activeSelf == true)
        //    {
        //        for (int i = work.addBlockList.Count - 1; i >= 0; i--)
        //        {
        //            Block block = work.addBlockList[i];
        //            if (poolingBlock.transform.position == block.transform.position)
        //            {
        //                Destroy(block.gameObject);
        //                work.addBlockList.Remove(work.addBlockList[i]);

        //                poolingBlock.gameObject.SetActive(false);
        //                poolingBlock.isVisible = false;

        //            }
        //        }
        //    }
        //}
    }
    public void ClickDeleteBlock()
    {
        RaycastHit hit;
        Ray mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 7;
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            for (int i = work.addBlockList.Count - 1; i >= 0; i--)
            {
                Vector3 position = TransPosition.instance.TranslatePosition(hit.transform.position);
                if(work.addBlockList[i].transform.position == position)
                {
                    Destroy(work.addBlockList[i].gameObject);
                    work.addBlockList.Remove(work.addBlockList[i]);
                }
            }
        }
    }
    public void UndoInfoSave(Block block)
    {
        work.removeBlock[undoRedoKey].Add(block);
    }
    public void UndoPop()
    {

    }
    public void UndoPush(int index)
    {
        if (index <= 9)
        {
            undoRedoKey = index;
        }
        if(index > 9)
        {
            for(int i = 9; i < 0; i--)
            {
                work.removeBlock[i - 1] = work.removeBlock[i];
            }
        }
    }
    public void UndoInfoSave(Vector3 position, int type)
    {
        //work.removeBlockPosList[undoRedoKey] = new List<Vector3>();
        //work.removeBlockTypeList[undoRedoKey] = new List<int>();

        work.removeBlockPosList[undoRedoKey].Add(position);
        work.removeBlockTypeList[undoRedoKey].Add(type);
    }
    public void Undo()
    {
        //이진탐색으로
        //최적화 해야한다.

        //Debug.Log("undoRedoKey : " + undoRedoKey);
        //foreach (Vector3 position in work.removeBlockPosList[undoRedoKey])
        //{
        //    //foreach(Block block in work.addBlockList)
        //    for (int i = work.addBlockList.Count - 1; i >= 0; i--)
        //    {
        //        if (position == work.addBlockList[i].transform.position)
        //        {
        //            work.addBlockList[i].isDestroy = true;
        //            work.addBlockList.Remove(work.addBlockList[i]);
        //        }
        //        //count++;
        //    }
        //}
        if (work.removeBlock.Count != 0 )
        {
            foreach (Block block in work.removeBlock[undoRedoKey - 1])
            {
                for (int i = work.addBlockList.Count - 1; i >= 0; i--)
                {
                    if (work.addBlockList[i] == block)
                    {
                        work.addBlockList[i].isDestroy = true;
                        work.addBlockList.Remove(work.addBlockList[i]);

                    }
                    //count++;
                }

            }
        }
        work.removeBlock[undoRedoKey - 1].Clear();
    }
    public void Redo()
    {
        List<int> typeList = work.removeBlockTypeList[undoRedoKey];
        List<Vector3> posList = work.removeBlockPosList[undoRedoKey];
        

        for(int i=0;i <= posList.Count-1;i++)
        {
            AddBlock(typeList[i], posList[i]);
        }
    }
    
    
    private bool OverlapBlockBuild(Block block)
    {
        foreach(Block addBlock in work.addBlockList)
        {
            if(addBlock.transform.position == block.transform.position)
            {
                return true;
            }
        }
        return false;
    } 
}
