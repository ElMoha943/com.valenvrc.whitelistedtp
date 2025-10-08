using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace valenvrc.WhitelistedTP
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
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

        public override void Interact(){
            if(!useWhitelist || (MasterByPass && Networking.IsMaster)){
                Networking.LocalPlayer.TeleportTo(destination.position, destination.rotation);
                return;
            }
            foreach(string name in whitelist){
                if(name == Networking.LocalPlayer.displayName){
                    Networking.LocalPlayer.TeleportTo(destination.position, destination.rotation);
                    break;
                }
            }
        }
    }
}


