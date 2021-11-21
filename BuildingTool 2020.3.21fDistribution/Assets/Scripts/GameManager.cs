using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
[System.Serializable]
public class BlockInfo
{
    public float[] pos = new float[3];
    public int dataType;
    public BlockInfo(float []pos,int dataType)
    {
        this.pos = pos;
        this.dataType = dataType;
    }

}
[System.Serializable]
public class BlockDatas
{
    public int dataNum;
    public List<BlockInfo> blockInfos = new List<BlockInfo>();
}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;

    private List<BlockInfo> blockData = new List<BlockInfo>();
    string path;
    [HideInInspector]
    public bool isLoad1;

    [Header("undoSize")]
    public int undoSize;

    [HideInInspector]
    public int undoPushCount;
    int count;
    [HideInInspector]
    public List<Block>[] undoRedoList;
    

    private void Awake()
    {
        instance = this;
        path = Application.dataPath + "/blockData" + ".dat";
        undoRedoList = new List<Block>[5];
        for(int i=0;i<5;i++)
        {
            undoRedoList[i] = new List<Block>();
        }
    }
    void Start()
    {


    }
    private void SetMaterials()
    {
        //for (int i = 1; i < 4; i++)
        //{
        //    materials.Add(Resources.Load<Material>("Blocks/BlockMaterial/Block" + i + "Mat"));
        //}
    }
    // Update is called once per frame
    public void Save()
    {
        List<Block> addBlockList = Builder.instance.work.addBlockList;
        for (int i = addBlockList.Count - 1; i >= 0; i--)
        {
            float[] pos = new float[3];
            pos[0] = addBlockList[i].transform.position.x;
            pos[1] = addBlockList[i].transform.position.y;
            pos[2] = addBlockList[i].transform.position.z;
            BlockInfo blockInfo = new BlockInfo(pos, addBlockList[i].blockType);
            blockData.Add(blockInfo);

        }
        string jdata = JsonConvert.SerializeObject(blockData);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        string format = System.Convert.ToBase64String(bytes);

        File.WriteAllText(Application.dataPath + "/BlockData.json", format);

        Debug.Log(jdata);
    }
    public void UndoRedoInit()
    {
        for(int i=4;i>0;i--)
        {
            undoRedoList[i - 1] = undoRedoList[i];
        }
    }
    public void UndoPush(Block block)
    {
        undoPushCount = undoPushCount % 5;
        undoRedoList[undoPushCount].Add(block);
        if (undoPushCount == 4)
            UndoRedoInit();
    }
    public void Load()
    {

        string jdata = File.ReadAllText(Application.dataPath + "/BlockData.json");
        byte[] bytes = System.Convert.FromBase64String(jdata);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes);
        blockData = JsonConvert.DeserializeObject<List<BlockInfo>>(reformat);
        int count = blockData.Count - 1;
        StartCoroutine(Load_Coroutine(count,count-3000));
    }
    private IEnumerator Load_Coroutine(int count, int min)
    {
        if (min < 0)
            min = 0;
        if (count < 0)
            StopCoroutine(Load_Coroutine(0, 0));
        for (int i = count; i >= min; i--)
        {
            Vector3 position = new Vector3(blockData[i].pos[0], blockData[i].pos[1],
            blockData[i].pos[2]);
            Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData[i].dataType, position));

        }
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(Load_Coroutine(count - 3000, min - 3000));
    }
  
    void Update()
    {
        
    }
}
