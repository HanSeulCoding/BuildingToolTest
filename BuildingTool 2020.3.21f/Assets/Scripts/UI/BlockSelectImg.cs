using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelectImg : MonoBehaviour
{
    public static BlockSelectImg instance;
    // Start is called before the first frame update
    private Transform blockSelectParent;
    private GameObject blockSIMG;
    private List<GameObject> blockSIMGs = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        blockSIMG = Resources.Load<GameObject>("BlockSIMG");
        blockSelectParent = GameObject.Find("BlockSelect").transform;
        for (int i = 0; i < 8; i++)
        {
            Material UIMaterial = new Material(Shader.Find("UI/Default"));

            //blockSIMG
            blockSIMGs.Add(Instantiate(blockSIMG, transform));
            
        }
        blockSelectParent.gameObject.SetActive(false);
    }

    public void SetActive(bool active)
    {
        
        if (!active)
            transform.gameObject.SetActive(false);
        if (active)
           GameObject.Find("Canvas").transform.Find("BlockSelect").gameObject.SetActive(true);
    }
    public void SetPosition(List<Vector3> pos)
    {
        int count = 0;
        foreach (GameObject selectImg in blockSIMGs)
        {
            selectImg.transform.position = pos[count];
            count++;
        }
    }
}
