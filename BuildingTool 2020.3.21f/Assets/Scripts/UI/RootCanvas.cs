using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootCanvas : MonoBehaviour
{
    public static RootCanvas instance;
    [Header("빌드 블록 목록")]
    public List<RectTransform> rectTransformList = new List<RectTransform>();
    private Text modeText;
    
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        modeText = GameObject.Find("ModeTxt").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBlock(int _index)
    {
        foreach (var rt in rectTransformList)
            rt.localScale = Vector3.one * 0.90f;

        rectTransformList[_index].localScale = Vector3.one;
    }

    public void PrintMode(string modeTxt)
    {
        modeText.text = modeTxt;
    }
}
