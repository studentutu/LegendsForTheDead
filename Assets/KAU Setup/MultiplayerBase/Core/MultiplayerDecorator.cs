// using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

namespace KAUGamesLviv.Services.Multiplayer
{
    public interface MultiplayerCustomKAUDecorator
    {
        
        #region  Connection To Server
        bool D_IsConnectedToServer();
        string D_GetCurrentRegion();
        int D_PingServer();
        /// <summary>
        /// Decorator, here you should add your callback to listen for Succesfull operation
        /// This will fire true if the We have connected to the server!
        /// </summary>
        void D_EventDisconectedFromServer(System.Action<string> CallbackOnDisconectedFromServer);
        void D_ConnectToSettingsServer(System.Action<bool> CallbackOnConnectedToServer, string Nickname);
        bool D_ConecToCloudServerRegion(string regionInternal, System.Action<bool> CallbackOnConnectedToServer, string nickNameOfLocalPlayer);

        void D_ConnectToBestCloudServer(System.Action<bool> CallbackOnConnectedToServer, string nickNameOfLocalPlayer);

        #endregion // Connection to Server



        #region  Game Room setup
        bool D_IsInsideRoom();
        void D_EventJoinedRoom(System.Action OnJoinedRoom);
        void D_EventJoinedRoomFailed(System.Action OnJoinedRoomFailed);
        void D_EventAnotherJoinedRoom(System.Action<MultiplayerPlayerDecorator> OnAnotherJoinedRoom);
        void D_EventAnotherLeftRoom(System.Action<MultiplayerPlayerDecorator> OnAnotherLeftRoom);

        void D_CreateCustomRoom(System.Action<bool> CallbackOnCreatedCustomRoom, string nameOfTheRoom);
        bool D_JoinNamedRoom(System.Action OnJoinedRoom, System.Action OnJoinedRoomFailed, string roomName);

        /// <summary> Decorator, should return Dictionary of hashname, object</summary>
        Dictionary<string, object> D_GetJoinedRoomInfo();
        #endregion // Game Room setup



        #region  Leaving
        void D_OnLeaveGame();

        #endregion // Local user Events



        #region Shared 
        List<int> D_GetAllCurrentPlayersIds();
        bool D_CheckIfAllPlayersAreReady();
        void D_ConnectPlayersToGameObjects();
        UnityEngine.GameObject D_Instantiate(string named);
        MultiplayerPlayerDecorator D_GetMyPlayer();
        MultiplayerPlayerDecorator[] D_GetAllPlayerS();
        Dictionary<string, object> D_GetPlayerInfo(int identifier);

        /// <summary> Optimization, clear your Dictionary after every time you use this function </summary>
        void D_SetMyPlayerChangedProperties(Dictionary<string, object> paramToUpdate);
        #endregion  // Shared



        #region  Optional

        void D_ChangePrecisionAmmount(float precision);

        void D_ChangeSendRate(int packagesPerSecond);
        int D_GetSendRate();

        #endregion // Optional


    }
}
