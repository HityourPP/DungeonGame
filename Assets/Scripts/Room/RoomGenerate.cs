using System.Collections.Generic;
using UnityEngine;

public class RoomGenerate : MonoBehaviour
{
    public enum Direction
    {
        Up,Down,Left,Right
    }
    [System.Serializable]
    public class WallType
    {
        public GameObject Wall_U, Wall_D, Wall_L, Wall_R, Wall_UL, Wall_UR, Wall_UD,
            Wall_RD, Wall_LD, Wall_LR, Wall_ULR, Wall_ULD, Wall_URD, Wall_LRD, Wall_ULRD;
    }
    
    [Header("Room Info")] 
    [SerializeField] private GameObject roomPrefabs;
    [SerializeField] private int roomAmount;
    [SerializeField] private Color startColor, endColor;

    [Header("Position Info")] 
    [SerializeField] private Transform point;        //房间生成位置
    [SerializeField] private float xOffset;          //位置偏移量
    [SerializeField] private float yOffset;
    [SerializeField] private LayerMask roomLayer;
    public WallType wallType;

    private GameObject endRoom;
    private Direction direction;
    private List<Room> roomList = new List<Room>();
    private List<GameObject>farRoom = new List<GameObject>();
    private List<GameObject>lessFarRoom = new List<GameObject>();
    private List<GameObject>oneWayRoom = new List<GameObject>();
    private int maxStep;
    
    private void Start()
    {
        xOffset = 18f;
        yOffset = 10f;
        for (int i = 0; i < roomAmount; i++)
        {
            roomList.Add(Instantiate(roomPrefabs, point.position, Quaternion.identity).GetComponent<Room>());
            ChangePoint();
        }

        roomList[0].GetComponent<SpriteRenderer>().color = startColor;
        roomList[0].roomNum = -3;
        endRoom = roomList[0].gameObject;
        foreach (var room in roomList)
        {
            SetUpRoom(room, room.transform.position);
        }
        FindEndRoom();
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
        endRoom.GetComponent<Room>().roomNum = -1;
        SetChestRoom();
        SetEnemyRoom();
    }

    private void ChangePoint()  //改变生成位置
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);
            switch (direction)//随机选择上下左右方向进行位置移动
            {
                case Direction.Up:
                    point.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.Down:
                    point.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.Left:
                    point.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.Right:
                    point.position += new Vector3(xOffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(point.position, 1f, roomLayer));
    }
    private void FindEndRoom()
    {
        //获取最大数值
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].stepToStart > maxStep)
            {
                maxStep = roomList[i].stepToStart;
            }
        }
        //获取最大数值房间及次大数值房间
        foreach(Room room in roomList)
        {
            if(room.stepToStart == maxStep)
            {
                farRoom.Add(room.gameObject);
            }
            if(room.stepToStart == maxStep - 1)
            {
                lessFarRoom.Add(room.gameObject);
            }
        }
        //寻找只有一个门的房间
        for(int i = 0; i < farRoom.Count; i++)
        {
            if (farRoom[i].GetComponent<Room>().doorAmount == 1)
            {
                oneWayRoom.Add(farRoom[i]);
            }
        }
        for (int i = 0; i < lessFarRoom.Count; i++)
        {
            if (lessFarRoom[i].GetComponent<Room>().doorAmount == 1)
            {
                oneWayRoom.Add(lessFarRoom[i]);
            }
        }
        //最后判断，若有，随机找一个，若无，在最远房间列表随机找一个
        if(oneWayRoom.Count != 0)
        {
            endRoom = oneWayRoom[Random.Range(0, oneWayRoom.Count)];
        }
        else
        {
            endRoom = farRoom[Random.Range(0, farRoom.Count)];
        }
    }

    private void SetChestRoom()
    {
        int chestRoomNum = -1;
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].stepToStart == 2 && roomList[i].roomNum != -1)
            {
                if (chestRoomNum == -1)
                {
                    chestRoomNum = i;                    
                }
                else
                {
                    if (Random.Range(0, 3) > 1f)
                    {
                        chestRoomNum = i;
                    }
                }
            }
        }
        roomList[chestRoomNum].roomNum = -2;
    }

    private void SetEnemyRoom()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].roomNum < 0)
            {
                continue;
            }
            roomList[i].roomNum = Random.Range(1, 10);
        }
    }
    private void SetUpRoom(Room room,Vector3 roomPosition)
    {   
        //向对应方向发射射线，检测对应方向是否有房间
        room.isLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 1f, roomLayer);
        room.isRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 1f, roomLayer);
        room.isUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 1f, roomLayer);
        room.isDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 1f, roomLayer);
        room.UpdateRoom(xOffset,yOffset);
        
        switch (room.doorAmount)
        {
            case 1:
                if (room.isLeft)
                    Instantiate(wallType.Wall_L,roomPosition,Quaternion.identity);
                if (room.isRight)
                    Instantiate(wallType.Wall_R, roomPosition, Quaternion.identity);
                if (room.isUp)
                    Instantiate(wallType.Wall_U, roomPosition, Quaternion.identity);
                if (room.isDown)
                    Instantiate(wallType.Wall_D, roomPosition, Quaternion.identity);
                break;
            case 2:
                if(room.isLeft && room.isRight)
                    Instantiate(wallType.Wall_LR, roomPosition, Quaternion.identity);
                if (room.isUp && room.isRight)
                    Instantiate(wallType.Wall_UR, roomPosition, Quaternion.identity);
                if (room.isLeft && room.isUp)
                    Instantiate(wallType.Wall_UL, roomPosition, Quaternion.identity);
                if (room.isLeft && room.isDown)
                    Instantiate(wallType.Wall_LD, roomPosition, Quaternion.identity);
                if (room.isDown && room.isRight)
                    Instantiate(wallType.Wall_RD, roomPosition, Quaternion.identity);
                if (room.isUp && room.isDown)
                    Instantiate(wallType.Wall_UD, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (room.isUp && room.isDown && room.isLeft)
                    Instantiate(wallType.Wall_ULD, roomPosition, Quaternion.identity);
                if (room.isUp && room.isDown && room.isRight)
                    Instantiate(wallType.Wall_URD, roomPosition, Quaternion.identity);
                if (room.isRight && room.isDown && room.isLeft)
                    Instantiate(wallType.Wall_LRD, roomPosition, Quaternion.identity);
                if (room.isRight && room.isUp && room.isLeft)
                    Instantiate(wallType.Wall_ULR, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (room.isUp && room.isDown && room.isLeft && room.isRight)
                    Instantiate(wallType.Wall_ULRD, roomPosition, Quaternion.identity);
                break;
        }
    }

}
