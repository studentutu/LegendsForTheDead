// // using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // using Photon.Pun;
// // using Photon.Realtime;
// // using Hashtable = ExitGames.Client.Photon.Hashtable;

// namespace KAUGamesLviv.Services.Multiplayer
// {
//     // THIS IS PHOTON IMPLEMENTATION OF DECORATOR
//     public class MultiplayerImplementation : MonoBehaviourPunCallbacks, MultiplayerCustomKAUDecorator
//     {
//         #region  Public Data

//         [SerializeField] private Vector3 posStart = new Vector3(10, 1, 12);
//         public static MultiplayerCustomKAUDecorator WholeMultiplayerSingleton;

//         #endregion  // end of Public Data


//         #region private  Data 
//         private Dictionary<int, MultiplayerPlayerDecorator> playerListEntries = new Dictionary<int, MultiplayerPlayerDecorator>(); // don't forget to attach to gameobject if you need them
//         private Dictionary<int, GameObject> allGamePlayers = new Dictionary<int, GameObject>();

//         #endregion


//         #region Events
//         public System.Action<bool> DCallbackOnConnectedToServer; // fire true only when Connected to Server - local only
//         public System.Action<string> DCallbackOnDisconectedFromServer; // fire once on Disconecting - local only
//         public System.Action<bool> DCallbackOnCreatedCustomRoom;  // fire once on Creating room - local only. DOnJoinedRoom also will fire
//         public event System.Action<MultiplayerPlayerDecorator> DCallbackAnotherJoinedRoom; // fire each time new Player entered already existed room  - multiple times
//         public event System.Action<MultiplayerPlayerDecorator> DCallbackAnotherLeftRoom;
//         public System.Action DOnJoinedRoom; // fire once on Entering room - local only
//         public System.Action DOnFailingConnectRoom; // fire once on Entering room - local only

//         #endregion


//         #region Initialization Step 


//         #region  Settings
//         [SerializeField] private RoomsCustomSettings AllPVPSettings;
//         public static RoomsCustomSettings staticSettings;

//         /// <summary>
//         ///  Returns a new Room from the Room Settings
//         /// </summary>
//         public Photon.Realtime.RoomOptions CreateRoomOptions(RoomsCustomSettings currentRoomSettings)
//         {
//             var newRoomOptions = new Photon.Realtime.RoomOptions { MaxPlayers = currentRoomSettings.MaxPlayers, IsVisible = currentRoomSettings.IsVisible };
//             ExitGames.Client.Photon.Hashtable listOfDefaultProperties = new ExitGames.Client.Photon.Hashtable();
//             foreach (var item in currentRoomSettings.preloadedRoomIntProperties)
//             {
//                 listOfDefaultProperties.Add(item.key, item.value);
//             }
//             foreach (var item in currentRoomSettings.preloadedRoomV3Properties)
//             {
//                 listOfDefaultProperties.Add(item.key, item.value);
//             }
//             foreach (var item in currentRoomSettings.preloadedRoomQuatProperties)
//             {
//                 listOfDefaultProperties.Add(item.key, item.value);
//             }
//             foreach (var item in currentRoomSettings.preloadedRoomStringProperties)
//             {
//                 listOfDefaultProperties.Add(item.key, item.value);
//             }
//             foreach (var item in currentRoomSettings.preloadedRoomBoolProperties)
//             {
//                 listOfDefaultProperties.Add(item.key, item.value);
//             }

//             foreach (var item in currentRoomSettings.preloadedRoomListintProperties)
//             {
//                 listOfDefaultProperties.Add(item.key, item.value);
//             }
//             if (listOfDefaultProperties.Count != 0)
//             {
//                 newRoomOptions.CustomRoomProperties = listOfDefaultProperties;
//             }
//             return newRoomOptions;
//         }
//         #endregion

//         [SerializeField] private bool PutIntoDontDestroy = false;
//         private void Awake()
//         {
//             WholeMultiplayerSingleton = this;
//             // Always provide multiplayer settings!
//             if (AllPVPSettings == null)
//             {
//                 throw new System.Exception(" There is no Settings! ");
//             }

//             staticSettings = AllPVPSettings;

//             // Always initialize multiplayer settings!
//             if (!RoomsCustomSettings.InitializedMultiplayerSettings)
//             {
//                 AllPVPSettings.Init();
//             }
//             if (PutIntoDontDestroy)
//             {
//                 DontDestroyOnLoad(this.gameObject);
//             }

//             // Uncomment to rely on Photon and load scene from Build-in Scenes
//             // PhotonNetwork.AutomaticallySyncScene = true; 
//             // best To use Player.CustomSetting to check with!
//         }

//         private void OnDestroy()
//         {
//             WholeMultiplayerSingleton = null;
//             D_OnLeaveGame(); // Dicsonnecting from room and from server
//         }

//         #endregion  // end of initialization Step 


//         #region  MultiplayerCustomKAUDecorator Implementation


//         #region  Connection To Server
//         public bool D_IsConnectedToServer()
//         {
//             return PhotonNetwork.IsConnected;
//         }
//         /// <summary>
//         /// Only for internal Use, not for UI and EndUser!
//         /// Will return null or the server name (region) - abbreviation!
//         /// </summary>
//         public string D_GetCurrentRegion()
//         {
//             return PhotonNetwork.CloudRegion;
//         }

//         public int D_PingServer()
//         {
//             if (!PhotonNetwork.IsConnected)
//             {
//                 return 99999;
//             }
//             return PhotonNetwork.GetPing();
//         }

//         public void D_EventDisconectedFromServer(System.Action<string> CallbackOnDisconectedFromServer)
//         {
//             DCallbackOnDisconectedFromServer = CallbackOnDisconectedFromServer;
//         }

//         public void D_ConnectToSettingsServer(System.Action<bool> CallbackOnConnectedToServer, string nickNameOfLocalPlayer)
//         {
//             // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
//             if (PhotonNetwork.IsConnected)
//                 return;
//             PhotonNetwork.LocalPlayer.NickName = nickNameOfLocalPlayer;
//             DCallbackOnConnectedToServer = CallbackOnConnectedToServer;

//             Debug.Log("Connecting...");
//             // #Critical, we must first and foremost connect to Photon Online Server.
//             bool isItCorrect = PhotonNetwork.ConnectUsingSettings();
//             if (!isItCorrect)
//             {
//                 if (DCallbackOnConnectedToServer != null)
//                     DCallbackOnConnectedToServer.Invoke(false);
//                 DCallbackOnConnectedToServer = null;
//                 OnDisconnected(DisconnectCause.DisconnectByClientLogic);
//             }


//         }


//         public readonly string[] All_Server_Regions_Photon = new string[]{
//             "asia", "au", "cae", "jp", "ru", "rue", "sa", "kr", "in",  "us", "usw", "eu"
//         };
//         public bool D_ConecToCloudServerRegion(string nameOfRegion, System.Action<bool> CallbackOnConnectedToServer, string nickNameOfLocalPlayer)
//         {

//             if (PhotonNetwork.IsConnected)
//                 return false;
//             PhotonNetwork.LocalPlayer.NickName = nickNameOfLocalPlayer;
//             DCallbackOnConnectedToServer = CallbackOnConnectedToServer;
//             // we check if we are connected or not, we join if we are , else we initiate the connection to the server.

//             PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
//             bool _result = PhotonNetwork.ConnectToRegion(nameOfRegion);
//             // #Critical, we must first and foremost connect to Photon Online Server.
//             if (!_result)
//             {
//                 if (DCallbackOnConnectedToServer != null)
//                     DCallbackOnConnectedToServer.Invoke(false);
//                 DCallbackOnConnectedToServer = null;
//                 OnDisconnected(DisconnectCause.DisconnectByClientLogic);
//             }
//             Debug.Log("ConnectToRegion(" + nameOfRegion + ") : " + _result);
//             return _result;

//         }

//         public void D_ConnectToBestCloudServer(System.Action<bool> CallbackOnConnectedToServer, string nickNameOfLocalPlayer)
//         {
//             if (PhotonNetwork.IsConnected)
//                 return;
//             PhotonNetwork.LocalPlayer.NickName = nickNameOfLocalPlayer;
//             DCallbackOnConnectedToServer = CallbackOnConnectedToServer;

//             PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

//             bool result = PhotonNetwork.ConnectToBestCloudServer();
//             if (!result)
//             {
//                 // Something went wrong
//                 if (DCallbackOnConnectedToServer != null)
//                     DCallbackOnConnectedToServer.Invoke(false);
//                 DCallbackOnConnectedToServer = null;
//                 OnDisconnected(DisconnectCause.DisconnectByClientLogic);
//             }

//         }

//         #endregion // Connection To Server


//         #region  Game Room setup

//         public bool D_IsInsideRoom()
//         {
//             return PhotonNetwork.CurrentRoom != null;
//         }

//         public void D_EventJoinedRoom(System.Action OnJoinedRoom)
//         {
//             DOnJoinedRoom = OnJoinedRoom;
//         }

//         public void D_EventJoinedRoomFailed(System.Action OnJoinedRoomFailed)
//         {
//             DOnFailingConnectRoom = OnJoinedRoomFailed;
//         }

//         public void D_EventAnotherJoinedRoom(System.Action<MultiplayerPlayerDecorator> callbackOnAnotherJoinedRoom)
//         {
//             DCallbackAnotherJoinedRoom += callbackOnAnotherJoinedRoom;
//         }

//         public void D_EventAnotherLeftRoom(System.Action<MultiplayerPlayerDecorator> OnAnotherLeftRoom)
//         {
//             DCallbackAnotherLeftRoom += OnAnotherLeftRoom;
//         }
//         public void D_CreateCustomRoom(System.Action<bool> CallbackOnCreatedCustomRoom, string nameOfTheRoom)
//         {
//             DCallbackOnCreatedCustomRoom = CallbackOnCreatedCustomRoom;
//             if (PhotonNetwork.IsConnected)
//             {
//                 if (PhotonNetwork.CurrentRoom != null)
//                 {
//                     if (DCallbackOnCreatedCustomRoom != null)
//                         DCallbackOnCreatedCustomRoom.Invoke(false);
//                     DCallbackOnCreatedCustomRoom = null;
//                     return;
//                 }
//                 // We can create a new room from our custom Settings.
//                 bool isItCorrect = PhotonNetwork.CreateRoom(nameOfTheRoom, CreateRoomOptions(AllPVPSettings));
//                 if (!isItCorrect)
//                 {
//                     OnJoinRandomFailed(0, "Game Logic : Was not  room Failed!");
//                     if (DCallbackOnCreatedCustomRoom != null)
//                         DCallbackOnCreatedCustomRoom.Invoke(false);
//                     DCallbackOnCreatedCustomRoom = null;
//                     return;
//                 }
//                 // true will be diplayed on Room Created

//             }
//             else
//             {

//                 if (DCallbackOnCreatedCustomRoom != null)
//                     DCallbackOnCreatedCustomRoom.Invoke(false);
//                 DCallbackOnCreatedCustomRoom = null;
//                 return;
//             }
//         }

//         public bool D_JoinNamedRoom(System.Action OnJoinedRoom, System.Action OnJoinFailing, string roomName)
//         {
//             D_EventJoinedRoomFailed(OnJoinFailing);
//             if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
//             {
//                 Debug.Log("<Color=Yellow> Already in Room </Color>");
//                 if (DOnFailingConnectRoom != null)
//                 {
//                     DOnFailingConnectRoom.Invoke();
//                 }
//                 DOnFailingConnectRoom = null;
//                 return false;
//             }
//             if (PhotonNetwork.IsConnected && PhotonNetwork.IsConnectedAndReady)
//             {
//                 D_EventJoinedRoom(OnJoinedRoom);
//                 return PhotonNetwork.JoinRoom(roomName);
//             }

//             if (DOnFailingConnectRoom != null)
//             {
//                 DOnFailingConnectRoom.Invoke();
//             }
//             DOnFailingConnectRoom = null;

//             return false;

//         }

//         /// <summary> returns Dictionary from Hashname and Object </summary>
//         public Dictionary<string, object> D_GetJoinedRoomInfo()
//         {
//             Dictionary<string, object> allCustomPropertiesFromCurrentRoom = new Dictionary<string, object>();
//             if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
//             {
//                 var allHashTable = PhotonNetwork.CurrentRoom.CustomProperties;
//                 foreach (var item in allHashTable.Keys)
//                 {
//                     object customObject;
//                     string fromItem = (string)item;
//                     if (allHashTable.TryGetValue(fromItem, out customObject))
//                     {
//                         allCustomPropertiesFromCurrentRoom.Add(fromItem, customObject);
//                     }
//                 }
//             }

//             return allCustomPropertiesFromCurrentRoom;
//         }

//         #endregion //  Game Room setup


//         #region Leaving
//         public void D_OnLeaveGame()
//         {
//             if (PhotonNetwork.IsConnected)
//             {
//                 PhotonNetwork.Disconnect();
//                 // PhotonNetwork.LeaveRoom();

//                 PhotonNetwork.LocalPlayer.CustomProperties.Clear();
//                 foreach (var item in allGamePlayers.Values)
//                 {
//                     Destroy(item);
//                 }
//                 allGamePlayers.Clear();
//                 playerListEntries.Clear();

//                 Debug.Log("<Color=Yellow> Left the Room </Color>");

//             }

//         }
//         #endregion // Leaving


//         #region  Shared

//         public List<int> D_GetAllCurrentPlayersIds()
//         {
//             List<int> allPlayersIds = new List<int>();
//             foreach (var item in PhotonNetwork.PlayerList)
//             {
//                 allPlayersIds.Add(item.ActorNumber);
//             }
//             return allPlayersIds;
//         }

//         /// <summary> Returns true only when players are in the same scene and state for each player is ready! </summary>
//         public bool D_CheckIfAllPlayersAreReady()
//         {
//             foreach (var item in playerListEntries.Keys)
//             {
//                 if (playerListEntries[item].IsPlayerReady == false)
//                 {
//                     return false;
//                 }
//             }
//             // foreach (Player p in PhotonNetwork.PlayerList)
//             // {
//             //     object isPlayerReady;
//             //     if (p.CustomProperties.TryGetValue(RoomsCustomSettings.allKeysDictionary[RoomsCustomSettings.MultiplayerKeyFull.Ready.ToString()], out isPlayerReady))
//             //     {
//             //         if (!(bool)isPlayerReady)
//             //         {
//             //             return false;
//             //         }
//             //     }
//             //     else
//             //     {
//             //         return false;
//             //     }
//             // }
//             return true;

//         }

//         public void D_ConnectPlayersToGameObjects()
//         {

//             // If Using only HashTables
//             // we must  manually create GameObjects  
//             if (allGamePlayers.Count > 0)
//             {
//                 foreach (var item in allGamePlayers.Values)
//                 {
//                     Destroy(item);
//                 }
//                 allGamePlayers.Clear();
//             }
//             foreach (var item in playerListEntries.Keys)
//             {

//                 // Intantiate Game Objects
//                 var go = D_Instantiate(null);
//                 playerListEntries[item].attachedTo = go;
//                 playerListEntries[item].rigidBodyFromComponent = go.GetComponent<Rigidbody>();
//                 allGamePlayers.Add(item, go);

//             }


//             // If using Photon Views on  Game Objects
//             // Just instantiate them if every player is in the right scene and state
//             // if (D_CheckIfAllPlayersAreReady())
//             // {

//             //     var go = D_Instantiate("RollerBall");
//             //     playerListEntries[PhotonNetwork.LocalPlayer.ActorNumber].attachedTo = go;
//             //     playerListEntries[PhotonNetwork.LocalPlayer.ActorNumber].rigidBodyFromComponent = go.GetComponent<Rigidbody>();
//             //     if (!allGamePlayers.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
//             //     {
//             //         allGamePlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber, go);
//             //     }
//             // }

//         }

//         /// <summary> Only Supposed to be used internally </summary>
//         public GameObject D_Instantiate(string doYOuNeedName)
//         {
//             if (doYOuNeedName != null)
//             {
//                 return PhotonNetwork.Instantiate("RollerBall", posStart, Quaternion.identity);
//             }
//             return Instantiate(AllPVPSettings.GetPrefab(0), posStart, Quaternion.identity);
//         }

//         public MultiplayerPlayerDecorator D_GetMyPlayer()
//         {

//             if (playerListEntries.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
//             {
//                 return playerListEntries[PhotonNetwork.LocalPlayer.ActorNumber];
//             }
//             return null;
//         }

//         public MultiplayerPlayerDecorator[] D_GetAllPlayerS()
//         {
//             var listOf = new List<MultiplayerPlayerDecorator>();
//             foreach (var item in playerListEntries.Keys)
//             {
//                 listOf.Add(playerListEntries[item]);
//             }
//             return listOf.ToArray();
//         }

//         /// <summary> returns Dictionary from Hashname and Object </summary>
//         public Dictionary<string, object> D_GetPlayerInfo(int idOfPlayer)
//         {
//             Dictionary<string, object> allCustomPropertiesFromCurrentRoom = new Dictionary<string, object>();
//             if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
//             {
//                 Player found = null;
//                 foreach (var item in PhotonNetwork.PlayerList)
//                 {
//                     if (item.ActorNumber == idOfPlayer)
//                     {
//                         found = item;
//                     }
//                 }
//                 if (found == null)
//                     return allCustomPropertiesFromCurrentRoom;


//                 var allHashTable = found.CustomProperties;

//                 foreach (var item in allHashTable.Keys)
//                 {
//                     object customObject;
//                     string fromItem = (string)item;
//                     if (allHashTable.TryGetValue(fromItem, out customObject))
//                     {
//                         allCustomPropertiesFromCurrentRoom.Add(fromItem, customObject);
//                     }
//                 }

//             }

//             return allCustomPropertiesFromCurrentRoom;
//         }

//         /// <summary> Set properties and update them via multiplayer </summary>
//         public void D_SetMyPlayerChangedProperties(Dictionary<string, object> paramToUpdate)
//         {
//             ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
//             foreach (var item in paramToUpdate.Keys)
//             {
//                 props.Add(item, paramToUpdate[item]);
//             }

//             PhotonNetwork.LocalPlayer.SetCustomProperties(props);

//         }

//         #endregion // Shared


//         #region Optional

//         public void D_ChangePrecisionAmmount(float difference)
//         {
//             if (difference < 0.01f)
//             {
//                 PhotonNetwork.PrecisionForFloatSynchronization = difference;
//             }
//             // PhotonNetwork.PrecisionForVectorSynchronization = 
//         }
//         public void D_ChangeSendRate(int packagesPerSecond)
//         {
//             PhotonNetwork.SendRate = packagesPerSecond;
//             // SerializationRate needs to be <= SendRate

//             int serializationRate = PhotonNetwork.SerializationRate;
//             if (serializationRate >= packagesPerSecond)
//             {
//                 // best to make it half 
//                 PhotonNetwork.SerializationRate = packagesPerSecond / 2;
//             }
//         }
//         public int D_GetSendRate()
//         {
//             return PhotonNetwork.SendRate;
//         }

//         #endregion // Optional


//         #endregion  // end of MultiplayerCustomKAUDecorator


//         #region Multiplayer Implementation via Photon (MonoBehaviourPunCallbacks)

//         /// <summary>Called after the connection to the master is established and authenticated (Default Photon auth) </summary>
//         public override void OnConnectedToMaster()
//         {
//             // we don't want to do anything if we are not attempting to join a room. 
//             // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
//             // we don't want to do anything.
//             Debug.Log("<Color=Green>OnConnectedToMaster</Color> " + "Now this client is connected to Cloud/Master server");


//             if (DCallbackOnConnectedToServer != null)
//                 DCallbackOnConnectedToServer.Invoke(true);
//             DCallbackOnConnectedToServer = null;

//         }

//         /// <summary> Called after disconnecting from the Photon server.</summary>
//         public override void OnDisconnected(DisconnectCause cause)
//         {
//             Debug.Log("<Color=Red>OnDisconnected</Color> " + cause);

//             if (DCallbackOnDisconectedFromServer != null)
//                 DCallbackOnDisconectedFromServer.Invoke(cause.ToString());
//             DCallbackOnDisconectedFromServer = null;
//         }

//         /// <summary>
//         /// Called when this client created a room and entered it. OnJoinedRoom() will be called as well.
//         /// </summary>
//         /// <remarks>
//         /// This callback is only called on the client which created a room (see OpCreateRoom).
//         ///
//         /// As any client might close (or drop connection) anytime, there is a chance that the
//         /// creator of a room does not execute OnCreatedRoom.
//         ///
//         /// If you need specific room properties or a "start signal", implement OnMasterClientSwitched()
//         /// and make each new MasterClient check the room's state.
//         /// </remarks>
//         public override void OnCreatedRoom()
//         {
//             if (DCallbackOnCreatedCustomRoom != null)
//                 DCallbackOnCreatedCustomRoom.Invoke(true);
//             DCallbackOnCreatedCustomRoom = null;
//         }

//         // internal check Photon to get the OnCreatedRoomFire true all the time
//         private bool CheckLocalPlayersCreatedRoomReady()
//         {
//             if (PhotonNetwork.IsMasterClient)
//             {
//                 return true;
//             }

//             return false;
//         }
        
//         /// <summary>
//         /// Called after switching to a new MasterClient when the current one leaves.
//         /// </summary>
//         /// <remarks>
//         /// This is not called when this client enters a room.
//         /// The former MasterClient is still in the player list when this method get called.
//         /// 
//         /// As any client might close (or drop connection) anytime, there is a chance that the
//         /// creator of a room does not execute OnCreatedRoom.
//         ///
//         /// If you need specific room properties or a "start signal", implement OnMasterClientSwitched()
//         /// and make each new MasterClient check the room's state.
//         /// </remarks>
//         public override void OnMasterClientSwitched(Player newMasterClient)
//         {

//             // Only if the number of players is exactly one
//             Debug.Log(" On Swithing masterClient  or OnCreatedRoom");
//             if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
//             {
//                 bool isReady = CheckLocalPlayersCreatedRoomReady();
//                 if (isReady && newMasterClient.IsLocal)
//                 {
//                     // You have been assigned with the room
//                     if (DCallbackOnCreatedCustomRoom != null)
//                         DCallbackOnCreatedCustomRoom.Invoke(true);
//                     DCallbackOnCreatedCustomRoom = null;
//                 }
//             }

//             if (PhotonNetwork.CurrentRoom != null && (PhotonNetwork.CurrentRoom.MaxPlayers + 1) == PhotonNetwork.PlayerList.Length)
//             {
//                 PhotonNetwork.CurrentRoom.IsOpen = true;
//                 PhotonNetwork.CurrentRoom.IsVisible = AllPVPSettings.IsVisible;
//             }

//         }

//         /// <summary>
//         /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
//         /// works only for  Local Player!
//         /// </summary>
//         /// <remarks>
//         /// This method is commonly used to instantiate player characters.
//         /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
//         ///
//         /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
//         /// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
//         /// enough players are in the room to start playing.
//         /// </remarks>
//         public override void OnJoinedRoom()
//         {
//             Debug.Log("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
//             Debug.Log("Now this client is in a room.");


//             if (playerListEntries == null)
//             {
//                 playerListEntries = new Dictionary<int, MultiplayerPlayerDecorator>();
//             }

//             MultiplayerPlayerDecorator entry = null;
//             foreach (Player p in PhotonNetwork.PlayerList)
//             {
//                 // Check if the gameObject is already there
//                 if (!playerListEntries.ContainsKey(p.ActorNumber))
//                 {
//                     entry = new MultiplayerPlayerDecorator();
//                     entry.Initialize(p.ActorNumber, p.NickName);
//                     playerListEntries.Add(p.ActorNumber, entry);
//                 }
//                 else
//                 {
//                     entry = playerListEntries[p.ActorNumber];
//                     entry.Initialize(p.ActorNumber, p.NickName);
//                 }

//                 object isPlayerReady;
//                 if (p.CustomProperties.TryGetValue(RoomsCustomSettings.allKeysDictionary[RoomsCustomSettings.MultiplayerKeyFull.Ready.ToString()], out isPlayerReady))
//                 {
//                     entry.SetPlayerReady((bool)isPlayerReady);
//                 }
//                 if (PhotonNetwork.LocalPlayer.ActorNumber == p.ActorNumber)
//                 {
//                     playerListEntries[p.ActorNumber].isLocalClient = true;
//                 }

//             }


//             ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
//             {
//                 { RoomsCustomSettings.allKeysDictionary[RoomsCustomSettings.MultiplayerKeyFull.Ready.ToString()], false }
//             };

//             ExitGames.Client.Photon.Hashtable expected = new ExitGames.Client.Photon.Hashtable
//             {
//                 { RoomsCustomSettings.allKeysDictionary[RoomsCustomSettings.MultiplayerKeyFull.PickCoin.ToString()], new int[]{-1,-1} } // [0] will be the player, [1] will be the item number
//             };

//             // Expected properties are unique to the Room, not to players!
//             PhotonNetwork.LocalPlayer.SetCustomProperties(props, expected);


//             if (PhotonNetwork.CurrentRoom != null && (PhotonNetwork.CurrentRoom.MaxPlayers) == PhotonNetwork.PlayerList.Length)
//             {
//                 PhotonNetwork.CurrentRoom.IsOpen = false;
//                 PhotonNetwork.CurrentRoom.IsVisible = false;
//             }


//             if (DOnJoinedRoom != null)
//                 DOnJoinedRoom.Invoke();
//             DOnJoinedRoom = null;

//         }

//         public override void OnJoinRoomFailed(short returnCode, string message)
//         {
//             Debug.Log("<Color=Red>OnJoinRoomFailed</Color>  with Code : " + returnCode + " : " + message);
//             playerListEntries.Clear();
//             foreach (var item in allGamePlayers.Values)
//             {
//                 Destroy(item);
//             }
//             allGamePlayers.Clear();

//             if (DOnFailingConnectRoom != null)
//                 DOnFailingConnectRoom.Invoke();
//             DOnFailingConnectRoom = null;
//         }


//         public override void OnPlayerEnteredRoom(Player newPlayer)
//         {
//             Debug.Log("<Color=Green> OnPlayerEnteredRoom </Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
//             Debug.Log("OnPlayerEnteredRoom is in a room.");


//             if (playerListEntries == null)
//             {
//                 playerListEntries = new Dictionary<int, MultiplayerPlayerDecorator>();
//             }

//             MultiplayerPlayerDecorator entry = null;

//             // Check if the gameObject is already there
//             if (!playerListEntries.ContainsKey(newPlayer.ActorNumber))
//             {
//                 entry = new MultiplayerPlayerDecorator();
//                 entry.Initialize(newPlayer.ActorNumber, newPlayer.NickName);
//                 playerListEntries.Add(newPlayer.ActorNumber, entry);
//             }
//             else
//             {
//                 entry = playerListEntries[newPlayer.ActorNumber];
//                 entry.Initialize(newPlayer.ActorNumber, newPlayer.NickName);
//             }

//             // If Using only Hashtables
//             if (allGamePlayers.Count > 0)
//             {
//                 // Already In Game
//                 GameObject gameObj = null;
//                 if (allGamePlayers.ContainsKey(newPlayer.ActorNumber))
//                 {
//                     gameObj = allGamePlayers[newPlayer.ActorNumber];
//                 }
//                 else
//                 {
//                     // Intantiate Game Objects
//                     gameObj = D_Instantiate(null);
//                     allGamePlayers.Add(newPlayer.ActorNumber, gameObj);
//                 }

//                 entry.attachedTo = gameObj;
//                 entry.rigidBodyFromComponent = gameObj.GetComponent<Rigidbody>();

//                 foreach (var item in playerListEntries.Keys)
//                 {
//                     if (playerListEntries[item].attachedTo != null)
//                     {
//                         playerListEntries[item].attachedTo.GetComponent<Rigidbody>().useGravity = true;
//                     }
//                 }
//             }


//             // If Using PhotonView  -- new entering managing Script Goes Into Prefab itself!

//             object isPlayerReady;
//             if (newPlayer.CustomProperties.TryGetValue(RoomsCustomSettings.allKeysDictionary[RoomsCustomSettings.MultiplayerKeyFull.Ready.ToString()], out isPlayerReady))
//             {
//                 entry.SetPlayerReady((bool)isPlayerReady);
//             }


//             if (PhotonNetwork.CurrentRoom != null && (PhotonNetwork.CurrentRoom.MaxPlayers) == PhotonNetwork.PlayerList.Length)
//             {
//                 PhotonNetwork.CurrentRoom.IsOpen = false;
//                 PhotonNetwork.CurrentRoom.IsVisible = false;
//             }


//             if (DCallbackAnotherJoinedRoom != null)
//                 DCallbackAnotherJoinedRoom.Invoke(entry);

//         }


//         /// <summary>
//         /// Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
//         /// </summary>
//         /// <remarks>
//         /// If another player leaves the room or if the server detects a lost connection, this callback will
//         /// be used to notify your game logic.
//         ///
//         /// Depending on the room's setup, players may become inactive, which means they may return and retake
//         /// their spot in the room. In such cases, the Player stays in the Room.Players dictionary.
//         ///
//         /// If the player is not just inactive, it gets removed from the Room.Players dictionary, before
//         /// the callback is called.
//         /// </remarks>
//         public override void OnPlayerLeftRoom(Player otherPlayer)
//         {

//             MultiplayerPlayerDecorator entry = null;
//             if (playerListEntries.ContainsKey(otherPlayer.ActorNumber))
//             {
//                 entry = playerListEntries[otherPlayer.ActorNumber];
//                 playerListEntries.Remove(otherPlayer.ActorNumber);
//             }

//             if (allGamePlayers.ContainsKey(otherPlayer.ActorNumber))
//             {
//                 Destroy(allGamePlayers[otherPlayer.ActorNumber].gameObject);
//             }

//             if (PhotonNetwork.CurrentRoom != null)
//             {
//                 PhotonNetwork.CurrentRoom.IsOpen = true;
//                 PhotonNetwork.CurrentRoom.IsVisible = AllPVPSettings.IsVisible;
//             }

//             if (DCallbackAnotherLeftRoom != null)
//             {
//                 if (entry == null)
//                 {
//                     entry = new MultiplayerPlayerDecorator();
//                     entry.Initialize(otherPlayer.ActorNumber, otherPlayer.NickName);
//                 }
//                 DCallbackAnotherLeftRoom.Invoke(entry);
//             }

//         }


//         /// <summary>
//         /// Called when custom player-properties are changed. Player and the changed properties are passed as object[].
//         /// Longer than with simple Object Synchronization! (this can be controlled manually! - values are set by Players )
//         /// 
//         /// Use PhotonView for frequent updates!
//         /// 
//         /// RPC  [PUNRPC] attribute, on each call on the client will run Reflexion code - overhead on top of the CustomProperties
//         /// 
//         /// Player custom properties are always stored with player!
//         /// 
//         /// Another option to serialize data is to use  IPunObservable inreface and pass the Object into PhotonView
//         /// </summary>
//         /// <remarks>
//         /// Changing properties must be done by Player.SetCustomProperties, which causes this callback locally, too.
//         /// </remarks>
//         ///
//         /// <param name="targetPlayer">Contains Player that changed.</param>
//         /// <param name="changedProps">Contains the properties that changed.</param>
//         public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
//         {
//             if (playerListEntries == null)
//             {
//                 playerListEntries = new Dictionary<int, MultiplayerPlayerDecorator>();
//             }
//             if (allGamePlayers == null)
//             {
//                 allGamePlayers = new Dictionary<int, GameObject>();
//             }



//             MultiplayerPlayerDecorator entry;
//             // Get the MultiplayerPlayer From Dictionary
//             if (playerListEntries.TryGetValue(target.ActorNumber, out entry))
//             {

//                 foreach (var item in changedProps.Keys)
//                 {
//                     object someObejctData;
//                     string fromItem = (string)item; // hashName


//                     // now parse into Enum - Description Name
//                     RoomsCustomSettings.MultiplayerKeyFull toEnum = RoomsCustomSettings.ParseEnum(RoomsCustomSettings.allKeysDictionary[fromItem]);
//                     switch (toEnum)
//                     {
//                         case RoomsCustomSettings.MultiplayerKeyFull.Ready:

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 entry.SetPlayerReady((bool)someObejctData);
//                             }
//                             break;
//                         case RoomsCustomSettings.MultiplayerKeyFull.Position:
//                             // this ensures the Position is the exact. Faster then with PhotonView. PhotonView is hard to manage!

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 entry.playerPosition = (Vector3)someObejctData;
//                             }
//                             break;
//                         case RoomsCustomSettings.MultiplayerKeyFull.Rotation:
//                             // this ensures the Position is the exact. Faster then with PhotonView. PhotonView is hard to manage!

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 entry.playerRotation = (Quaternion)someObejctData;
//                             }
//                             break;
//                         case RoomsCustomSettings.MultiplayerKeyFull.PickCoin:

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 int[] whoHavePickedItUp = (int[])someObejctData; // x will contain the Actor, y the Coins in Array
//                                 if (playerListEntries.ContainsKey(whoHavePickedItUp[0]))
//                                 {
//                                     playerListEntries[whoHavePickedItUp[1]].CoinsCollectedOnScene += 1;
//                                 }
//                                 // Destroy Coin
//                             }
//                             break;
//                         case RoomsCustomSettings.MultiplayerKeyFull.CoinsNumber:

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 entry.CoinsCollectedOnScene = (int)someObejctData;
//                             }
//                             break;
//                         case RoomsCustomSettings.MultiplayerKeyFull.SceneRequired:

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 string neededScene = (string)someObejctData;

//                                 if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(neededScene).IsValid())
//                                 {
//                                     Debug.Log(" All Scene Requirements are met. Now Load the Scene. After Scene is Loaded SetProperies  RoomsCustomSettings.AllKeys.Ready = true");
//                                 }
//                                 else
//                                 {
//                                     Debug.Log("<Color=Red>Scene MIssing</Color>");
//                                 }
//                             }
//                             break;
//                         case RoomsCustomSettings.MultiplayerKeyFull.Force:
//                             // this ensures the Position is the exact. Faster then with PhotonView. PhotonView is hard to manage!

//                             if (changedProps.TryGetValue(fromItem, out someObejctData))
//                             {
//                                 Vector3 newForce = (Vector3)someObejctData;
//                                 entry.Force = newForce;

//                             }
//                             break;
//                     }


//                 }
//             }

//         }

//         #endregion  // end of  Photon (MonoBehaviourPunCallbacks)


//         #region Pun Synchronization via PhotonView

//         // Only if it  used with PhotonView
//         public void ConnectGameObjectToPlayer(GameObject whichGameObject)
//         {
//             // It is Already In Game

//             Player newPlayer = whichGameObject.GetComponent<PhotonView>().Owner;
//             MultiplayerPlayerDecorator entry = null;
//             if (!playerListEntries.ContainsKey(newPlayer.ActorNumber))
//             {
//                 entry = new MultiplayerPlayerDecorator();
//                 entry.Initialize(newPlayer.ActorNumber, newPlayer.NickName);
//                 playerListEntries.Add(newPlayer.ActorNumber, entry);
//             }
//             else
//             {
//                 entry = playerListEntries[newPlayer.ActorNumber];
//                 entry.Initialize(newPlayer.ActorNumber, newPlayer.NickName);
//             }

//             if (allGamePlayers.ContainsKey(newPlayer.ActorNumber))
//             {
//                 // var checkOwner = allGamePlayers[newPlayer.ActorNumber].GetComponent<PhotonView>().Owner;
//                 // if (checkOwner != newPlayer)
//                 // {
//                 //     Debug.Log( " Cleaned Up lost players !");
//                 //     PhotonNetwork.Destroy(allGamePlayers[newPlayer.ActorNumber]);
//                 // }
//                 allGamePlayers.Remove(newPlayer.ActorNumber);
//             }

//             Debug.Log(" New Player : " + newPlayer.ActorNumber);
//             allGamePlayers.Add(newPlayer.ActorNumber, whichGameObject);


//             entry.attachedTo = whichGameObject;
//             entry.rigidBodyFromComponent = whichGameObject.GetComponent<Rigidbody>();

//         }
//         // For PhotonNetwork.Instantiation - Photon requirement : PhotonNetwork.Instantiation(prefabs)  - prefabs are within a Resources folder!
//         // PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);
//         // PhotonNetwork.InstantiateSceneObject("BigAsteroid", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);


//         // #Critical PhotonView
//         // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
//         // PhotonNetwork.AutomaticallySyncScene = true;


//         // #Critical: PhotonView
//         // We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
//         // Load the Room Level. 
//         // PhotonNetwork.LoadLevel("PunBasics-Room for 1");
//         #endregion

//         #region Remote Calls (RPC) / Remote Events
//         // [PunRPC] void ChatMessage(string a, string b){}
//         // to run it you need PhotonView, default from MonoBehaviourPun
//         // PhotonView.Get(this).RPC("ChatMessage",PhotonTargets.All, "I Was", "there" );


//         // Raise Event , implement IOnEventCallback interface

//         // public void OnEvent(EventData photonEvent)
//         // {
//         // 	  ProcessOnEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
//         // }

//         //  And Subsribe this Event to Photon
//         // public void OnEnable()
//         // {
//         //     PhotonNetwork.OnEventCall += OnEvent;
//         // }

//         // public void OnDisable()
//         // {
//         //     PhotonNetwork.OnEventCall -= OnEvent;
//         // }


//         // byte evCode = 0; // Custom Event 0: Used as "MoveUnitsToTargetPosition" event, only [0 - 199]
//         // object[] content = new object[] { new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10 }; // Array contains the target position and the IDs of the selected units
//         // bool reliable = true;
//         // RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
//         // PhotonNetwork.RaiseEvent(evCode, content, reliable, raiseEventOptions);
//         // 
//         // the server won't send the event back to the origin (by default). to get the event, call it locally
//         // (note: the order of events might be mixed up as we do this locally)
//         // ProcessOnEvent(evCode, moveHt, PhotonNetwork.LocalPlayer.ActorNumber);
//         #endregion
//     }
// }

