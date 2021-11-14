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
    public List<Position> positionList = new List<Position>();
    public static TransPosition instance;
    private void Awake()
    {
        instance = this;
       

    }
  
    public Position TransLocalPosition(Vector3 position)
    {
        Position tempPosition = new Position();
       
        tempPosition.x = Mathf.FloorToInt((position.x / (WorldGenerator.Instance.rowSize / 10.0f))); // 1.0f;
        tempPosition.y = Mathf.FloorToInt((position.y / (WorldGenerator.Instance.rowSize / 10.0f)));
        tempPosition.z = Mathf.FloorToInt((position.z / (WorldGenerator.Instance.columnSize / 10.0f)));
        //Debug.Log("WorldX값 오류 : " + position.x);
        //Debug.Log("LocalX값 오류 :" + tempPosition.x);
        //변환하는거 따로 함수 화 해서 만들기
        return tempPosition;
    }
    public Vector3 TransWorldPosition(Position position)
    {
        //math.tr
        float tempX = WorldGenerator.Instance.rowSize / 10.0f / 2.0f;
        float tempZ = WorldGenerator.Instance.columnSize / 10.0f / 2.0f;
        float tempY = WorldGenerator.Instance.YSize / 10.0f / 2.0f;
        //if (position.x < 0)
        //    tempX = -tempX;
        //if (position.z < 0)
        //    tempZ = -tempZ;
        //if (position.x == 0 && position.isMinusZeroX)
        //    tempX = -tempX;
        //if (position.z == 0 && position.isMinusZeroZ)
        //    tempZ = -tempZ;
        worldPosition = new Vector3((float)position.x + tempX, (float)(position.y + tempY), (float)(position.z + tempZ));
        return worldPosition;
    }
    public Vector3 TranslatePosition(Vector3 position)
    {
        Position localPos = TransLocalPosition(position);
        Vector3 transPosition = TransWorldPosition(localPos);
        return transPosition;
    }
   

}
