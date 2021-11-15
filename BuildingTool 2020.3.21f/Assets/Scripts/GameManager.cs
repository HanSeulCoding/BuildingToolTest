using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    

    private void Awake()
    {
        instance = this;
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
    void Update()
    {
        
    }
}
