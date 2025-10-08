using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.WhitelistedTP
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class WhitelistedTP : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("Should the teleport use a whitelist?")]
        bool useWhitelist = true;
        [SerializeField, Tooltip("List of names that can use the teleport")]
        string[] whitelist;
        [SerializeField, Tooltip("Where to teleport the player to")]
        Transform destination;
        [SerializeField, Tooltip("Should the master of the instance always be able to teleport?")]
        bool MasterByPass = true;
        [SerializeField, Tooltip("Should the owner of the instance always be able to teleport?")]
        bool OwnerByPass = true;

        VRCPlayerApi localPlayer;
        
        void Start()
        {
            localPlayer = Networking.LocalPlayer;
        }

        public override void Interact()
        {
            if (!useWhitelist || (MasterByPass && Networking.IsMaster) || (OwnerByPass && Networking.IsOwner(gameObject)))
            {
                localPlayer.TeleportTo(destination.position, destination.rotation);
                return;
            }
            foreach (string name in whitelist)
            {
                if (name == localPlayer.displayName)
                {
                    localPlayer.TeleportTo(destination.position, destination.rotation);
                    break;
                }
            }
        }
    }
}


