using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.WhitelistedTP
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [DisallowMultipleComponent, Icon("Packages/com.valenvrc.whitelistedtp/Editor/Resources/whitelistedtpicon.png"), HelpURL("https://docs.valenvrc.com/free-assets/whitelisted-tp")]
    public class WhitelistedTPTrigger : UdonSharpBehaviour
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

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if(!player.isLocal) return;
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

        #if UNITY_EDITOR && !COMPILER_UDONSHARP
        //Gizmos to see the target location in editor
        internal static void DrawTPPoint(Transform point, Color colorCircle, Color colorArrow, Color colorLabel, string label = "")
        {
            if (point == null) return;
            // Set gizmo color
            Gizmos.color = colorCircle;

            // Draw circle at point position
            float circleRadius = 0.5f;
            UnityEditor.Handles.color = colorCircle;
            UnityEditor.Handles.DrawWireDisc(point.position, Vector3.up, circleRadius);

            // Draw arrow pointing in point's forward direction
            Vector3 arrowStart = point.position + Vector3.up * 0.1f;
            Vector3 arrowEnd = arrowStart + point.forward * circleRadius * 1.5f;

            Gizmos.color = colorArrow;
            Gizmos.DrawLine(arrowStart, arrowEnd);

            // Draw arrowhead
            Vector3 arrowRight = arrowEnd - point.right * 0.3f - point.forward * 0.3f;
            Vector3 arrowLeft = arrowEnd + point.right * 0.3f - point.forward * 0.3f;
            Gizmos.DrawLine(arrowEnd, arrowRight);
            Gizmos.DrawLine(arrowEnd, arrowLeft);

            // Draw text label with point name
            UnityEditor.Handles.color = colorLabel;
            Vector3 labelPosition = point.position + Vector3.up;
            UnityEditor.Handles.Label(labelPosition, label);
        }

        private void OnDrawGizmos()
        {
            if (destination == null) return;
            DrawTPPoint(destination, Color.cyan, Color.blue, Color.white, "LTP: "+gameObject.name);
        }
    #endif
    }
}


