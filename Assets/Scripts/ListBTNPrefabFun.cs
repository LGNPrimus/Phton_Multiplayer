using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListBTNPrefabFun : MonoBehaviour
{
	public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		info = _info;
		
	}
	public void OnClick()
	{
		//Network.Lobby.JoinRoom(info);
	}
}
