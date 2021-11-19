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
    [Header("�� ���� Size")]
    public int rowSize;
    [Header("�� ���� Size")]
    public int columnSize;
    [Header("�� Y�� Size")]
    public int YSize;
    public enum Command
    {
        None,
        Add,        //�߰���
        Remove      //������
    }

    [Header("���� ��1")]
    public GameObject blockModel1;

    private GameObject poolingBlock;
    //private GameObject poolingBlockParent;
    
    private Transform addTransform;
    private Transform poolingBlockTransform;

    private List<Block> poolingBlockList = new List<Block>();

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
  

    private void Awake()
    {
        instance = this;
        InitSetting();
       // Test();
        
    }
    private void Test()
    {

        for (int z = 0; z < 110; z++)
        {
            Vector3 position = new Vector3(0, z, 0);
            AddBlock(0, position);
        }


    }
    private void InitSetting()
    {
        blockModel = Resources.Load<Block>("Blocks/Block");

        addTransform = transform.Find("Add");
        bottomFloor = GameObject.Find("BottomFloor");
        bottomFloor.transform.localScale = new Vector3(rowSize, 1, columnSize);

        poolingBlock = Resources.Load<GameObject>("Blocks/PoolingBlock"); //pooling block �����
        poolingBlock.GetComponent<Block>().isPooling = true;
        poolingBlock.SetActive(false);
        poolingBlockTransform = GameObject.Find("PoolingBlock").transform;

        poolingBlock.gameObject.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < 100; i++) //PoolingBlock Instantiate 
        {
            for (int j = 0; j < 100; j++)
            {
                poolingBlockList.Add(Instantiate(poolingBlock, poolingBlockTransform).GetComponent<Block>());
            }
        }

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
    
    //PreBlock ���� ����
    public void AddVisibleBlock(int _blockSelect, Vector3 clickMousePos, Vector3 currentMousePos)
    {
        if (currentMousePos.x >= 50 || currentMousePos.x < -50
             || currentMousePos.z >= 50 || currentMousePos.z < -50 || currentMousePos.y >= 110)
        {
            Debug.Log("false");
            return;
        }
        Position clickPos = TransPosition.instance.TransLocalPosition(clickMousePos);
        Position currentpos = TransPosition.instance.TransLocalPosition(currentMousePos);
        

        Debug.Log("current Pos X: " + currentpos.x + "Pos Y : " + currentpos.y + "Pos Z :" + currentpos.z);
        RectMake(clickPos, currentpos);
    }

    //MouseClickposition���� ���� ���콺�������� ���� �簢�� ���� ���� 
    private void RectMake(Position clickPos, Position currentPos)
    {
        int xWidth = currentPos.x - clickPos.x;
        int zHeight = currentPos.z - clickPos.z;

        Vector2Int rectTopLeft = new Vector2Int();
        Vector2Int rectBottomRight = new Vector2Int();


        if (xWidth <= 0 && zHeight <= 0) //������ ���ʿ��� ���� �Ʒ������� �巡��
        {
            rectTopLeft = new Vector2Int((int)currentPos.x, (int)clickPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, currentPos.z);
        }
        else if (xWidth <= 0 && zHeight >= 0) // ������ �Ʒ��ʿ��� ���� �������� �巡��
        {
            rectTopLeft = new Vector2Int(currentPos.x, currentPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, clickPos.z);
        }
        else if (xWidth >= 0 && zHeight >= 0) // ���� ���ʿ��� ������ �Ʒ������� �巡��
        {
            rectTopLeft = new Vector2Int(clickPos.x, currentPos.z);
            rectBottomRight = new Vector2Int(currentPos.x, clickPos.z);
        }
        else if (xWidth >= 0 && zHeight <= 0) //���� �Ʒ��ʿ��� ������ �������� �巡��
        {
            rectTopLeft = new Vector2Int(clickPos.x, clickPos.z);
            rectBottomRight = new Vector2Int(currentPos.x, currentPos.z);
        }

        Debug.Log("RectLeft : " + rectTopLeft);
        Debug.Log("RectRight : " +rectBottomRight);

        DragVisibleBlock(rectTopLeft, rectBottomRight, clickPos.y,currentPos.y);
        RemoveVisibleBlock(rectTopLeft, rectBottomRight, currentPos.y); //mousePosition ���� visibleBlock �����ֱ� 
    }
    //�簢�� ������ ���� PreBlock �Ǽ� 
    private void DragVisibleBlock(Vector2Int rectTopLeft, Vector2Int rectBottomRight,int clickYLine, int currentYLine)
    {
        //if (rectTopLeft.y == rectBottomRight.y && rectTopLeft.x == rectBottomRight.x)
        //{
        //    for (int y = 0; y <= yLine; y++)
        //        SetVisiblePosition(rectBottomRight.x, y, rectBottomRight.y);
        //}

        //if (rectTopLeft.y == rectBottomRight.y) //���ΰ� ���� ���(���ڷ� �� ���η� �Ǽ��� ���)
        //{
        //    Debug.Log("serro");
        //    for (int i = rectTopLeft.x; i <= rectBottomRight.x; i++)
        //    {
        //        //for (int y = 0; y <= currentYLine; y++)
        //        //    SetVisiblePosition(i, y, rectTopLeft.y);
        //        if (clickYLine <= currentYLine)
        //        {
        //            for (int y = clickYLine; y <= currentYLine; y++)
        //                SetVisiblePosition(i, y, rectTopLeft.y);
        //        }
        //        if (clickYLine > currentYLine)
        //        {
        //            for (int y = currentYLine; y <= clickYLine; y++)
        //                SetVisiblePosition(i,y,rectTopLeft.y);
        //        }
        //    }
        //    return;
        //}
        //if (rectTopLeft.x == rectBottomRight.x) // ���ΰ� ���� ��� (���ڷ� �� ���η� �Ǽ��� ���)
        //{
        //    Debug.Log("garro");
        //    for (int i = rectBottomRight.y; i <= rectTopLeft.y; i++)
        //    {
        //        //for (int y = 0; y <= currentYLine; y++)
        //        //    SetVisiblePosition(rectTopLeft.x, y, i);
        //        if (clickYLine <= currentYLine)
        //        {
        //            for (int y = clickYLine; y <= currentYLine; y++)
        //                SetVisiblePosition(rectTopLeft.x, y, i);
        //        }
        //        if (clickYLine > currentYLine)
        //        {
        //            for (int y = currentYLine; y <= clickYLine; y++)
        //                SetVisiblePosition(rectTopLeft.x, y, i);
        //        }
        //    }
        //    return;
        //}
        if(!(rectTopLeft.x == rectBottomRight.x) || !(rectTopLeft.y == rectBottomRight.y))
        {
            for (int i = rectTopLeft.x; i <= rectBottomRight.x; i++) //�� �� �Ϲ� �簢�� �Ǽ�
            {
                Debug.Log("rect");  
                for (int z = rectBottomRight.y; z <= rectTopLeft.y; z++)
                {
                    Debug.Log("rectTopLeft  X : " + rectTopLeft.x + " rectBottomRight Right X : " + rectBottomRight.x);
                    Debug.Log("rectTopLeft  Z : " + rectTopLeft.y + "RectBottomRight Right Z : " + rectBottomRight.y);
                    Debug.Log(currentYLine);
                    
                    if (clickYLine <= currentYLine)
                    {
                        for (int y = clickYLine; y <= currentYLine; y++)
                            SetVisiblePosition(i, y, z);
                    }
                    if (clickYLine > currentYLine)
                    {
                        for (int y = currentYLine; y > clickYLine; y--)
                            SetVisiblePosition(i, y, z);
                    }
                }
            }
        }

        //backUp
        //for (int i = rectTopLeft.x; i <= rectBottomRight.x; i++) //�� �� �Ϲ� �簢�� �Ǽ�
        //{
        //    for (int z = rectBottomRight.y; z <= rectTopLeft.y; z++)
        //    {
        //        for (int y = 0; y <= currentYLine; y++)
        //            SetVisiblePosition(i, y, z);
        //    }
        //}
    }

    public void SetVisiblePosition(int x, int y, int z)
    {

        //Debug.Log("SetVisiblePos");
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (poolingBlock.gameObject.activeSelf == true)
            {
                if (poolingBlock.position.x == x && poolingBlock.position.z == z && poolingBlock.position.y == y) //�ߺ��˻�(���� �� block ���� X)
                {
                    break;
                }
            }
            if (y == -1)
                return;
            if (poolingBlock.gameObject.activeSelf == false)
            {
                poolingBlock.gameObject.SetActive(true);

                poolingBlock.isVisible = true; //isVisible�� ���־�� Active ���� �� Position�� ���� ���θ� �� �� �ִ�. 
               
                poolingBlock.transform.position = new Vector3(x, y, z); //pooling Box�� Position ����
                poolingBlock.blockType = blockType;
                poolingBlock.position.x = x;
                poolingBlock.position.z = z;
                poolingBlock.position.y = y;
                break;
            }
            
        }
    }

    public void RemoveVisibleBlock(Vector2Int rectTopLeft, Vector2Int rectBottomRight, int yLine)
    {
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (poolingBlock.isVisible)
            {
                if (poolingBlock.gameObject.activeSelf == true)
                {
                    if (poolingBlock.position.x < rectTopLeft.x || poolingBlock.position.x > rectBottomRight.x
                         || poolingBlock.position.z < rectBottomRight.y || poolingBlock.position.z > rectTopLeft.y
                         || poolingBlock.position.y > yLine)
                    //�簢�� �ȿ� ������ �ʴ°��
                    {
                        //Debug.Log("erase" + "X: " + poolingBlock.position.x + "Z: " + poolingBlock.position.z);
                        poolingBlock.gameObject.SetActive(false); // Active ���ֱ�
                        poolingBlock.isVisible = false;

                    }
                }
            }
        }
    }
    public void VisibleBlockFalse() //�����ֱ� �� Block ���� ����
    {
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (!poolingBlock.gameObject.activeSelf)
                continue;

            if (poolingBlock.gameObject.activeSelf == true)
            {
                poolingBlock.isVisible = false;
                poolingBlock.gameObject.SetActive(false);
            }
        }
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

        Debug.Log("TempMousePos X :" + tempMousepos.x + "Y :" + tempMousepos.y + "Z : " + tempMousepos.z);
        if ((tempMousepos.x +_normal.x) > 50 || (tempMousepos.x + _normal.x) < -50
            || (tempMousepos.z + _normal.z) > 50 || (tempMousepos.z + _normal.z) < -50 ||(tempMousepos.y + _normal.y >= 110))
            return;

        blockGo = AddBlock(blockType, poolingBlock.transform.position);

        tempY = (int)InputBlockPos.Instance.mouseOnClickPosition.y; //0.0������ Local����� Local Pos �� -1�� �Ǵ� �̻��� �������� ���� 0�ϋ��� ������                                                //0�� �ǰ� ���� 
        if (tempY == 0)                                       //0�� �ǰ� ���� 
            tempMousepos.y = 0.0f;

        blockGo.transform.position = tempMousepos;
        work.addBlockList.Add(blockGo); //workList�� �߰�
        GameManager.instance.undoPushCount++;
    }

    public void DragBuildBlock()
    {
        for(int i = poolingBlockList.Count - 1; i >= 0; i--)
        {
            if (!poolingBlockList[i].isVisible)
                continue;
            if(poolingBlockList[i].gameObject.activeSelf == true)
            {
                Block blockGo = AddBlock(blockType, poolingBlockList[i].transform.position); 

                poolingBlockList[i].gameObject.SetActive(false);
                poolingBlockList[i].isVisible = false;
                if (blockGo.GetComponent<Block>() != null)
                   work.addBlockList.Add(blockGo.GetComponent<Block>());
            }
        }
        GameManager.instance.undoPushCount++;
    }
    public Block AddBlock(int blockType, Vector3 pos)
    {
        Block blockGo = Instantiate(blockModel, addTransform);
        blockGo.transform.position = pos;
        blockGo.blockType = blockType;

        GameManager.instance.UndoPush(blockGo);

        return blockGo;
    }

    public void DragDeleteBlock()
    {
        //����Ž������ ����ȭ �ؾ���
        // int count = 0;
        for (int i = poolingBlockList.Count - 1; i >= 0; i--)
        {
            if (!poolingBlockList[i].gameObject.activeSelf)
                continue;
            if (!poolingBlockList[i].isVisible)
                continue;   
            if (poolingBlockList[i].gameObject.activeSelf == true)
            {
                for (int j = work.addBlockList.Count - 1; j >= 0; j--)
                {
                    Block block = work.addBlockList[j];

                    if (poolingBlockList[i].transform.position == block.transform.position)
                    {
                        Destroy(block.gameObject);
                       // GameManager.instance.UndoPush(block);
                        work.addBlockList.Remove(work.addBlockList[j]);
                        poolingBlockList[i].gameObject.SetActive(false);
                        poolingBlockList[i].isVisible = false;

                    }
                }
            }
        }
       // GameManager.instance.undoPushCount++;
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
                    //GameManager.instance.UndoPush(work.addBlockList[i]);

                    Destroy(work.addBlockList[i].gameObject);
                    work.addBlockList.Remove(work.addBlockList[i]);
                }
            }
        }
      //  GameManager.instance.undoPushCount++;
    }
    public void Undo(int index)
    {
        int count = GameManager.instance.undoRedoList[index].Count - 1;
        for (int i = count; i >= 0; i--)
        {
            Block block = GameManager.instance.undoRedoList[GameManager.instance.undoPushCount][i];
            Destroy(block);
            work.addBlockList.Remove(block);
        }
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
