using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public int x;
    public int y;
    public int z;

}
public class Math : MonoBehaviour
{
    // Start is called before the first frame update
    public Position position = new Position();
    //public Position tempPosition = new Position();
    public Vector3 worldPosition = new Vector3();
    public List<Position> positionList = new List<Position>();
    public static Math instance;
    private void Awake()
    {
        instance = this;
        for(int i=0;i<100;i++)
        {

        }
    }
  
    public Position TransLocalPosition(Vector3 position)
    {
        Position tempPosition = new Position();

        tempPosition.x = Mathf.FloorToInt((position.x / (WorldGenerator.Instance.rowSize / 10.0f))); // 1.0f;
        tempPosition.y = Mathf.FloorToInt((position.y / (WorldGenerator.Instance.rowSize / 10.0f)));
        tempPosition.z = Mathf.FloorToInt((position.z / (WorldGenerator.Instance.columnSize / 10.0f)));
        
        //변환하는거 따로 함수 화 해서 만들기
        return tempPosition;
    }
    public Vector3 TransWorldPosition(Position position)
    {
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

}
