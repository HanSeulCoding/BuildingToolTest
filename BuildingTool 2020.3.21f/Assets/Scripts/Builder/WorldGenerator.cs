using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work
{
    public List<int> removeBlockTypeList;
    public List<Vector3> removeBlockPosList;

    public List<GameObject> addBlockList;
}
public class WorldGenerator : MonoBehaviour
{
    public string Version = "1.0.0";

    public static WorldGenerator Instance;
    [Header("�� ���� Size")]
    public int rowSize;
    [Header("�� ���� Size")]
    public int columnSize;
    public enum Command
    {
        None,
        Add,        //�߰���
        Remove      //������
    }

    [Header("���� ��1")]
    public GameObject addBlock1;
    [Header("���� ��2")]
    public GameObject addBlock2;
    [Header("���� ��3")]
    public GameObject addBlock3;
    [Header("���� ��4")]
    public GameObject addBlock4;
    [Header("���� ��5")]
    public GameObject addBlock5;
    [Header("���� ��6")]
    public GameObject addBlock6;
    [Header("���� ��7")]
    public GameObject addBlock7;
    [Header("���� ��8")]
    public GameObject addBlock8;
    [Header("���� ��9")]
    public GameObject addBlock9;
    [Header("���� ��10")]
    public GameObject addBlock10;


    [HideInInspector]
    public Vector3 mouseClickPosition;

    private GameObject bottomFloor;
    private Transform addTransform;
    private Transform poolingBlockTransform;
    private List<Block> poolingBlockList = new List<Block>();
    private GameObject poolingBlock;
    private Work work;

    private int count;
    [HideInInspector]
    public List<GameObject> addBlockList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        addBlockList.Add(addBlock1);
        addBlockList.Add(addBlock2);
        addBlockList.Add(addBlock3);
        addBlockList.Add(addBlock4);

        addTransform = transform.Find("Add");
        bottomFloor = GameObject.Find("BottomFloor");
        bottomFloor.transform.localScale = new Vector3(rowSize, 1, columnSize);

        poolingBlock = Resources.Load<GameObject>("Blocks/block"); //pooling block �����
        poolingBlockTransform = GameObject.Find("PoolingBlock").transform;
       

        for(int i=0;i<rowSize;i++) //PoolingBlock Instantiate 
        {
            for(int j =0;j<columnSize;j++)
            {
                poolingBlockList.Add(Instantiate(poolingBlock, poolingBlockTransform).GetComponent<Block>());
            }
        }

        work = new Work();
        work.addBlockList = new List<GameObject>();
        work.removeBlockPosList = new List<Vector3>();
        work.removeBlockTypeList = new List<int>();
    }
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 PositionDivide(Vector3 _pos)
    {
        Vector3 pos = _pos;
        int xInt = (int)pos.x;
        float xFloat = pos.x - xInt;

        if (xFloat > 0)
            xFloat = 0.5f;
        if (xFloat < 0)
            xFloat = -0.5f;

        int zInt = (int)pos.z;
        float zFloat = pos.z - zInt;

        if (zFloat > 0)
            zFloat = 0.5f; 
        if (zFloat < 0)
            zFloat = -0.5f;

        pos.x = xInt + xFloat;
        pos.z = zInt + zFloat;

        return pos + Vector3.up * 0.5f;
    }
    public void VisibleAddBlock(int _blockSelect, Vector3 clickMousePos, Vector3 currentMousePos, bool _isAnimation)
    {

        Position clickPos = Math.instance.TransLocalPosition(clickMousePos);
        Position currentpos = Math.instance.TransLocalPosition(currentMousePos);

        //Debug.Log("click" + clickPos.x.ToString());
        //Debug.Log("currentPos" + currentpos.x.ToString());
        if (clickPos.x < currentpos.x)
        {
            for (int i = clickPos.x; i < currentpos.x; i++)
            {
                poolingBlockList[count].transform.position = currentMousePos;
                poolingBlockList[count].TranslatePosition();
                poolingBlockList[count].TransWorldPosition();
            }
        }
        Vector3 delta = clickMousePos - currentMousePos;
        
        
    }
    public void AddBlock(int _blockSelect, int _layer, Vector3 _pos, Vector3 _normal, bool _isAnimation, bool isVisible)
    {
        //*�ǵ�����(Undo)�� �ٽý���(Redo)�� �ƴϸ� �� �۾����� �ν����� ����

        GameObject blockGo = Instantiate(addBlockList[_blockSelect], addTransform) as GameObject;
        blockGo.transform.rotation = Quaternion.identity;

        switch (_layer)
        {
            //�� ���� ����� ��
            case 7:
                if (isVisible)
                    break;
                blockGo.transform.position = _pos + _normal;
                break;
            //�ٴڿ� ����� ��
            case 6:
                {
                    Vector3 pos = _pos;

                    //Debug.Log(pos);
                    blockGo.transform.position = _pos;
                    Block block = blockGo.GetComponent<Block>();
                    block.TranslatePosition();
                    block.TransWorldPosition();
                }
                break;
        }

        blockGo.name = blockGo.name.Replace("(Clone)", "");

        if (_isAnimation == true)
        {
            Block block = blockGo.GetComponent<Block>();
            block.CreateAnimation();
        }

        work.addBlockList.Add(blockGo);
    }
    public void LookBlockCreate(int blockType)
    {
        GameObject blockObj = addBlockList[blockType];
        
    }
}
