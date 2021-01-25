using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class Network : MonoBehaviourPunCallbacks, ILobbyCallbacks, IInRoomCallbacks
{
    [Header("singleton class reference")]
    public static Network Lobby;

    [Header("Random Room Name")]
    public string RandomRoomName;

    [Header("Button Object")]
    public GameObject MainPNL;
    public GameObject RoomListPNL;
    public GameObject ButtonPrefab;
    public GameObject BackBTNRoomList;
    public GameObject GameModePNL;

    public Transform ContentRef;

    [Header("Output Text Box")]
    public Text OutputTextBox;

    [Header("Input Firld")]
    public InputField CreateRoomName;



    private Dictionary<string, int> RoomDataDictobj = new Dictionary<string, int>();
    private List<RoomInfo> LocalroomList;

    #region Defining singelton
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Lobby = this ;
        
    }
    #endregion

    #region Connecting to Photon Network
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }
    #endregion

    #region after Connected to Master Of Lobby
    public override void OnConnectedToMaster()
    {
        Debug.Log("You are connected to " + PhotonNetwork.CloudRegion);
       MainPNL.SetActive(true);
       
        base.OnConnectedToMaster();
    }
    #endregion

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby........");
       // MainPNL.SetActive(true);
        base.OnJoinedLobby();
    }

    #region connect Button Function
    /*public void ConnectBTN()
    {
        ConnectBtn.SetActive(false);
        CancelBtn.SetActive(true);
        Debug.Log("Connect Btn Pressed");
        PhotonNetwork.JoinRandomRoom();
        
        
    }*/
    #endregion


    /// <summary>
    /// CreateRoom() function room creating k kaam ayega issme hum randon numbrer lenge including a srting "Room" jisse room pechannne me asani ho.
    /// fir hum jo room create kr rehe h usski properties define karenge jese room visibility, is room open and player quantity.
    /// fir PhotonNetwork.createRoom me name and properties pass kr k room create karenge.
    /// </summary>
    #region Room creation Function
    public void CreateRoomBTN()
    {

        PhotonNetwork.ConnectUsingSettings();
        
        if (CreateRoomName.text == "")
        {
            Debug.Log("Plaese Enter Vaid Name");
        }
        else
        {


            Debug.Log("Creating Room");
            RandomRoomName = CreateRoomName.text;
            Debug.Log(RandomRoomName);
            RoomOptions RoomProperties = new RoomOptions();

            RoomProperties.IsVisible = true;
            RoomProperties.IsOpen = true;
            RoomProperties.MaxPlayers = 6;


            PhotonNetwork.CreateRoom(CreateRoomName.text, RoomProperties);
        }
    }
    #endregion
    public override void OnJoinedRoom()
    {
        OutputTextBox.text = "You Joined to:-  " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("You Joined to:-  " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log(" Room Joined");
        MainPNL.SetActive(false);
        base.OnJoinedRoom();
    }

   

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
       

        Debug.Log("Room list updated");
    
       



            //Transform Content = RoomListPNL.transform.Find("Scroll View/Viewport/Content");
            LocalroomList = roomList;
        foreach (RoomInfo a in roomList)
         {
            Debug.Log(a.PlayerCount);
            Debug.Log("Room name" + a.Name);
            foreach(var d in RoomDataDictobj)
            {

                if ( d.Key == a.Name)
                {
                    foreach (Transform t in ContentRef)
                    {

                        if (d.Value > a.PlayerCount || d.Value < a.PlayerCount)
                        {
                            if(t.transform.Find("RoomNameText").GetComponent<Text>().text == d.Key)
                            {
                                Debug.Log("destroied.........." + a.Name);
                                Destroy( t.gameObject );

                            }
                        }
                    }
                }
                
            }
        

            if (a.RemovedFromList)
            {
                foreach (Transform b in ContentRef)
                {
                    if(b.transform.Find("RoomNameText").GetComponent<Text>().text == a.Name)
                    {
                        Debug.Log("Removed Room " + a.Name);
                        Destroy(b.gameObject);
                    }
                    
                }
            }
            else
            {
                GameObject LocalButtonPrefab = Instantiate(ButtonPrefab, ContentRef) as GameObject;
                LocalButtonPrefab.transform.Find("RoomNameText").GetComponent<Text>().text = a.Name;
                LocalButtonPrefab.transform.Find("NumofPlayer").GetComponent<Text>().text = a.PlayerCount + "/" + a.MaxPlayers;
                if (RoomDataDictobj.ContainsKey(a.Name))
                {
                    RoomDataDictobj[a.Name] = a.PlayerCount;
                }
                // Add new room info to cache
                else
                {
                    RoomDataDictobj.Add(a.Name, a.PlayerCount);
                }
               

                LocalButtonPrefab.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(LocalButtonPrefab.transform); });
            }
            
         }
            base.OnRoomListUpdate(roomList);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }

    private void ClearRoomList()
    {
       // Transform Content = RoomListPNL.transform.Find("Scroll View/Viewport/Content");
        foreach(Transform a in ContentRef)
        {

            Destroy(a.gameObject);
        }
    }


    /// <summary>
    /// OnRoomCreationFailed  ye function mostly jab call hoga jab first atempt me room nahi bana ho qki jo random number aaya h uss nam ka room available h Lobby me.
    /// ye function fir se recreate arega or call karegea Create room  function .
    /// if same room milta  h to recall hoga.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    #region Room Creation Fail And recreating room
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create , Trying Once More");
        CreateRoomBTN();
        base.OnJoinRoomFailed(returnCode, message);
    }
    #endregion





    public void JoinRoom(  Transform localBTNPrefabTrans)  
    {
       // PhotonNetwork.JoinRoom(info.Name);
        string LocalRoomName = localBTNPrefabTrans.Find("RoomNameText").GetComponent<Text>().text;
        PhotonNetwork.JoinRoom(LocalRoomName);
        RoomListPNL.SetActive(false);
        GameModePNL.SetActive(true);
        
    }

    #region cancel Button to leave lobby
    public void CancelBTN()
    {
       
       
        PhotonNetwork.LeaveRoom();
        
    }
    #endregion

    #region When player enter In room this Function Called
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Koi to room Me aya h"+"\n Total number Of player:- "+PhotonNetwork.CurrentRoom.PlayerCount);
      
        OutputTextBox.text = "SomeOne entered In the room" + "\n Total number Of player:- " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
    #endregion

    #region When player exits In room this Function Called
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Kisi Ne Room chood diya" + "\n Total number Of player:- " + PhotonNetwork.CurrentRoom.PlayerCount);
        OutputTextBox.text = "SomeOne Left the room" + "\n Total number Of player:- " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
    #endregion

    #region Info or room you Left
    public override void OnLeftRoom()
    {
        Debug.Log("You left the room number:-  " + RandomRoomName);
        //  OutputTextBox.text = "You left the room";
        base.OnLeftRoom();
    }
    #endregion

    public override void OnLeftLobby()
    {
        Debug.Log("you left the Lobby");
        base.OnLeftLobby();
    }

    /// <summary>
    /// All Button Function
    /// </summary>

    #region All Button Functions
    public void ShowRoomListBTN()
    {
        MainPNL.SetActive(false);
        RoomListPNL.SetActive(true);
        ClearRoomList();
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        
    }

    public void BackBTNRoomListFUN()
    {
        MainPNL.SetActive(true);
        RoomListPNL.SetActive(false);
        PhotonNetwork.LeaveLobby();
     
    }

    public void RefreshBTNFun()
    {
        PhotonNetwork.LeaveLobby();

        
            PhotonNetwork.JoinLobby();
        

    }
    #endregion
    public void BackBTNFunGameMode()
    {
        MainPNL.SetActive(true);
        GameModePNL.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}
