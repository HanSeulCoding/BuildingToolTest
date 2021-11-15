using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public int x;
    public int y;
    public int z;

}
public class TransPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public Position position = new Position();
    //public Position tempPosition = new Position();
    public Vector3 worldPosition = new Vector3();

    public static TransPosition instance;
    private void Awake()
    {
        instance = this;
    }
  
    public Position TransLocalPosition(Vector3 position)
    {
        Position tempPosition = new Position();
       
        tempPosition.x = Mathf.FloorToInt((position.x / (Builder.instance.rowSize / 10.0f))); // 1.0f;
        tempPosition.y = Mathf.FloorToInt((position.y / (Builder.instance.rowSize / 10.0f)));
        tempPosition.z = Mathf.FloorToInt((position.z / (Builder.instance.columnSize / 10.0f)));
        //Debug.Log("WorldX값 오류 : " + position.x);
        //Debug.Log("LocalX값 오류 :" + tempPosition.x);
        //변환하는거 따로 함수 화 해서 만들기
        return tempPosition;
    }
    public Vector3 TransWorldPosition(Position position)
    {
        float tempX = Builder.instance.rowSize / 10.0f / 2.0f;
        float tempZ = Builder.instance.columnSize / 10.0f / 2.0f;
        float tempY = Builder.instance.YSize / 10.0f / 2.0f;
  
        worldPosition = new Vector3((float)position.x + tempX, (float)(position.y), (float)(position.z + tempZ));
        return worldPosition;
    }
    public Vector3 TranslatePosition(Vector3 position)
    {
        Position localPos = TransLocalPosition(position);
        // Vector3 transPosition = TransWorldPosition(localPos);
        Vector3 transPosition = new Vector3((float)localPos.x, (float)localPos.y, (float)localPos.z);
        return transPosition;
    }
   

}
