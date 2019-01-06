// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

namespace KAUGamesLviv.Services.Multiplayer
{
    
    public class MultiplayerPlayerDecorator
    {
        // Any number of fields to suit your Multiplayer logic
        #region  Multiplayer Fields
        public int CoinsCollectedOnScene = 0; // example of a simple field

        #region  Field for Position
        public bool changedMultiplayerPos = false;
        public Vector3 MultiplayerPosition = Vector3.zero;
        public Vector3 playerGamePosition
        {
            get
            {
                if (attachedTo != null)
                {
                    return attachedTo.transform.position;
                }
                return MultiplayerPosition;
            }
            set
            {
                
                if (attachedTo != null)
                {
                    rigidBodyFromComponent.MovePosition(value);
                }
            }
        }
        #endregion // Field for Position
        

        #region  Field for Rotation
        public bool changedMultiplayerRot = false;
        public Quaternion MultiplayerRotarion = Quaternion.identity;
        public Quaternion playerGameRotation
        {
            get
            { 
                if (attachedTo != null)
                {
                    return attachedTo.transform.rotation;
                }
                return MultiplayerRotarion;
            }
            set
            {
                if (attachedTo != null)
                {
                    rigidBodyFromComponent.MoveRotation(value);
                }
            }
        }

        #endregion // Field for Rotation


        #region  Field for Force
        public bool changedMultiplayerForce = false;
        public Vector3 MultiplayerForce = Vector3.zero;

        public Vector3 playerGameForce
        {
            get
            {
                if (attachedTo != null && rigidBodyFromComponent != null)
                {
                    return rigidBodyFromComponent.velocity;
                }
                return MultiplayerForce;
            }
            set
            {
                if (attachedTo != null)
                {
                    rigidBodyFromComponent.AddForce(value);
                }
            }
        }
        #endregion //  Field for Force

        [HideInInspector] public string Nickname = null;
        [HideInInspector] public int Id = -1;
        #endregion // Multiplayer Fields


        #region  Game Logic Fields
        [HideInInspector] public bool IsPlayerReady = false;
        [HideInInspector] public bool isLocalClient = false;
        public Rigidbody rigidBodyFromComponent;

        [SerializeField] public GameObject attachedTo;
        #endregion // Game Logic Fields
        

        #region Multiplauer Game Logic Methods
        public void Initialize(int id, string nick)
        {
            Id = id;
            Nickname = nick;
        }
        public void SetPlayerReady(bool setValue)
        {
            IsPlayerReady = setValue;
        }

        #endregion // Game Logic Methods


        //  THOSE METHODS MUST ONLY BE SET BY LOCAL USER
        #region  Multiplayer Methods
        public void SetMultiplayerPosition(Vector3 newPos)
        {
            MultiplayerPosition = newPos;
            changedMultiplayerPos = true;
        }

        public void SetMultiplayerRotation(Quaternion newRotation)
        {
            MultiplayerRotarion = newRotation;
            changedMultiplayerRot = true;
        }

        public void SetMultiplayerForce(Vector3 force)
        {
            MultiplayerForce = force;
            changedMultiplayerForce = true;
        }

        #endregion // Multiplayer Methods


    }
}
