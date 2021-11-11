using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public void ClickScaleBtn()
    {
        Builder.Instance.mouseOnClickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
               -Camera.main.transform.position.z));
        PlayerManager.instance.isTranslateScale = true;
        Debug.Log("Click");

    }
    public void EndDrag()
    {
        PlayerManager.instance.isTranslateScale = false;
        Debug.Log("EndClick");
    }
}
