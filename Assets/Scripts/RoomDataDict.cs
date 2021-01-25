using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDataDict 
{
    public string RoomName;
    public int NumOfPlayer;

    public RoomDataDict(string newRoomName , int newNumOfPlayer)
    {
        RoomName = newRoomName;
        NumOfPlayer = newNumOfPlayer;
    }
}
