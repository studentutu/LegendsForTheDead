using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAUGamesLviv.Services.Multiplayer
{
    #region  serializable item for Multiplayer Keys dictionery
    [System.Serializable]
    public class CustomPropertiesHashEntry
    {
        public RoomsCustomSettings.MultiplayerKeyFull Description;
        public string hashName;
    }

    #endregion // serializable item for Multiplayer Keys dictionery


    [CreateAssetMenu(fileName = "PVPSettings", menuName = "Multiplayer/DefaultSettings", order = 1)]
    public class RoomsCustomSettings : ScriptableObject
    {

        #region  Room Only Settings

        [Header(" Room Settings ")]
        public bool IsVisible = true;
        public byte MaxPlayers = 2;

        [Tooltip(" Those are only for Rooms. For example to notify others that the room is required some Scene")]
        public List<HashEntriesIntPreloaded> preloadedRoomIntProperties = new List<HashEntriesIntPreloaded>();
        [Tooltip(" Those are only for Rooms.")]
        public List<HashEntriesV3Preloaded> preloadedRoomV3Properties = new List<HashEntriesV3Preloaded>();
        [Tooltip(" Those are only for Rooms.")]
        public List<HashEntriesQuetPreloaded> preloadedRoomQuatProperties = new List<HashEntriesQuetPreloaded>();
        [Tooltip(" Those are only for Rooms.")]
        public List<HashEntriesStringPreloaded> preloadedRoomStringProperties = new List<HashEntriesStringPreloaded>();
        [Tooltip(" Those are only for Rooms.")]
        public List<HashEntriesBoolPreloaded> preloadedRoomBoolProperties = new List<HashEntriesBoolPreloaded>();
        [Tooltip(" Those are only for Rooms.")]
        public List<HashEntriesListIntPreloaded> preloadedRoomListintProperties = new List<HashEntriesListIntPreloaded>();

        #endregion // Room Only Settings


        [Header(" Shared Settings ")]

        [SerializeField] private List<CustomPropertiesHashEntry> AllMultiplayerKeys = new List<CustomPropertiesHashEntry>();
        public static Dictionary<string, string> allKeysDictionary = new Dictionary<string, string>();

        // THIS MUST BE THE FULL DESCTIPRION
        public enum MultiplayerKeyFull
        {
            SceneRequired,
            Ready,
            PickCoin,
            Position,
            Rotation,
            CoinsNumber,
            Force
        }

        public static MultiplayerKeyFull ParseEnum(string value)
        {
            return (MultiplayerKeyFull)System.Enum.Parse(typeof(MultiplayerKeyFull), value, ignoreCase: true);
        }
        public static bool InitializedMultiplayerSettings = false;
        public void Init()
        {
            allKeysDictionary.Clear();
            foreach (var item in AllMultiplayerKeys)
            {
                allKeysDictionary.Add(item.Description.ToString(), item.hashName);
                allKeysDictionary.Add(item.hashName, item.Description.ToString());
            }
            InitializedMultiplayerSettings = true;
        }


        [Header("Game visual Player Settings")]
        public List<GameObject> AllPlayerPrefabs = new List<GameObject>();
        public GameObject GetPrefab(int index)
        {
            return AllPlayerPrefabs[index];
        }

    }
    
    
    #region  Serializable Stuff to work on Scriptable Object
    [System.Serializable]
    public class HashEntriesIntPreloaded : HashEntriesPreloaded<int>{}
    [System.Serializable]
    public class HashEntriesStringPreloaded : HashEntriesPreloaded<string>{}

    [System.Serializable]
    public class HashEntriesBoolPreloaded : HashEntriesPreloaded<bool>{}

    [System.Serializable]
    public class HashEntriesV3Preloaded : HashEntriesPreloaded<Vector3>{}
    [System.Serializable]
    public class HashEntriesQuetPreloaded : HashEntriesPreloaded<Quaternion>{}
    [System.Serializable]
    public class HashEntriesListIntPreloaded : HashEntriesPreloaded<List<int>>{}


    [System.Serializable]
    public class HashEntriesPreloaded<T>
    {
        public string key;
        public T value;
    }
    #endregion

}
