using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public void ClickScaleBtn()
    {
        InputBlockPos.Instance.mouseOnClickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
               -Camera.main.transform.position.z));
        PlayerInputManager.instance.isTranslateScale = true;
        Debug.Log("Click");

    }
    public void EndDrag()
    {
        PlayerInputManager.instance.isTranslateScale = false;
        Debug.Log("EndClick");
    }
}
