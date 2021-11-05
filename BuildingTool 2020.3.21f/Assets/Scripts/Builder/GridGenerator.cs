using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator instance;
    [Header("Grid Row")]
    public int rowSize;
    [Header("Grid Column")]
    public int columnSize;
    [Header("Detail Grid Row")]
    public int detailRowSize;
    [Header("Detail Grid Column")]
    public int detailColumnSize;
    [Header("Grid Color")]
    public Color gridColor;
    [Header("DetailGrid Color")]
    public Color detailGridColor;

    [Header("Line Number")]
    public int lineNum = 10;
    [Header("Detail Line Number")]
    public int detailLineNum = 100;
    [Header("Line ����")]
    public float lineLength = 100.0f;

    public GameObject bottomFloor;
    public GameObject line;
    public GameObject detailLine;
    
    private GameObject detailGridT;
    private LineRenderer renderLine;
    private Vector3 pos;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bottomFloor = GameObject.Find("BottomFloor");
        detailGridT = GameObject.Find("DetailGrid");
        pos = this.transform.position;

        horizonLines(rowSize, lineNum, transform, false);
        verticalLines(columnSize, lineNum, transform, false);
        horizonLines(detailRowSize, detailLineNum, detailGridT.transform, true);
        verticalLines(detailColumnSize, detailLineNum, detailGridT.transform, true);

        detailGridT.SetActive(false); 
        
    }

    //first line genarte
    public void InstLine(Transform parentTransform, bool isDetail)
    {

        GameObject obj;
       
        obj = Instantiate(line, pos, Quaternion.identity);
        obj.transform.SetParent(parentTransform);
        renderLine = obj.GetComponent<LineRenderer>();
    
        
        
        renderLine.useWorldSpace = false;
        renderLine.SetWidth(0.1f, 0.1f);
        
        renderLine.material = new Material(Shader.Find("Sprites/Default"));
        if (isDetail)
            renderLine.SetColors(Color.gray, Color.gray);
        else
            renderLine.SetColors(Color.green, Color.green);

    }
    //vertical lines genarte,
    //���λ�����, ���� �� ��, �θ�Transform, detailGrid ����
    private void verticalLines(int columnSize, int lineNum,Transform transform, bool isDetail)
    {
        for (int i = 0; i < columnSize; i++)
        {
            InstLine(transform, isDetail);

            if (isDetail && i % this.lineNum == 0)
                continue;

            for (int j = 0; j < 2; j++)
            {
                renderLine.SetPosition(j, new Vector3(pos.x + (i * (lineLength / lineNum)),
                                                      pos.y,
                                                      pos.z + (j * lineLength)));
            }
        }
    }
    //horizontal lines genarate
    //���λ�����, ���� �� ��, �θ�Transform, detailGrid ����
    private void horizonLines(int rowSize, int lineNum, Transform transform, bool isDetail)
    {
        for (int i = 0; i < rowSize; i++)
        {
            InstLine(transform, isDetail);

            if (isDetail && i % this.lineNum == 0) //���� ��ø�Ǿ� �� ������ �ȵǱ⿡ 
                continue;
     
            for (int j = 0; j < 2; j++)
            {
                  
                renderLine.SetPosition(j, new Vector3(pos.x + (j * lineLength),
                                                      pos.y,
                                                      pos.z + (i * (lineLength / lineNum))));

           
            }
            if (!isDetail)
            {
                if (i == rowSize - 1)
                {
                    InstLine(transform, isDetail);
                    for (int j = 0; j < 2; j++)
                    {
                        renderLine.SetPosition(j, new Vector3(pos.x + (j * lineLength),
                                                            pos.y,
                                                            pos.z + (rowSize * (lineLength / lineNum))));
                    }
                }
            }

          
        }
    }
    public void CreateDetailGrid()
    {
        GameObject.Find("World").transform.Find("DetailGrid").gameObject.SetActive(true);
        CameraController.instance.isCreateDetailGrid = true; //if������ CreateDetailGrid ���� ���� ����
    }
    public void RemoveDetailGrid()
    {
        GameObject.Find("World").transform.Find("DetailGrid").gameObject.SetActive(false);
        CameraController.instance.isRemoveDetailGrid = true; //if������ RemoveDetailGrid ���� ���� ����
    }
}

