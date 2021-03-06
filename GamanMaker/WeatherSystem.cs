using System;
using UnityEngine;

namespace GamanMaker
{
    public class WeatherSystem
    {
        // code borrowed from https://github.com/Valheim-Modding/Wiki/wiki/Server-Validated-RPC-System and modified
        public static void RPC_RequestSetWeather(long sender, ZPackage pkg)
        {
            if (pkg != null && pkg.Size() > 0)
            { // Check that our Package is not null, and if it isn't check that it isn't empty.
                ZNetPeer peer = ZNet.instance.GetPeer(sender); // Get the Peer from the sender, to later check the SteamID against our Adminlist.
                if (peer != null)
                { // Confirm the peer exists
                    string peerSteamID = ((ZSteamSocket)peer.m_socket).GetPeerID().m_SteamID.ToString(); // Get the SteamID from peer.
                    if (
                        ZNet.instance.m_adminList != null &&
                        ZNet.instance.m_adminList.Contains(peerSteamID)
                    )
                    { // Check that the SteamID is in our Admin List.
                        string msg = pkg.ReadString(); // Read the message from the user.
                        pkg.SetPos(0); // Reset the position of our cursor so the client's can re-read the package.
                        ZRoutedRpc.instance.InvokeRoutedRPC(0L, "EventSetWeather", new object[] { pkg }); // Send our Event to all Clients. 0L specifies that it will be sent to everybody
                        EnvMan.instance.m_debugEnv = msg;
                    }
                } 
                else
                {
                    ZPackage newPkg = new ZPackage(); // Create a new ZPackage.
                    newPkg.Write("You aren't an Admin!"); // Tell them what's going on.
                    ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequestMsg", new object[] { newPkg }); // Send the error message.
                }
            }
        }
        
        public static void RPC_EventSetWeather(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_RequestSetTime(long sender, ZPackage pkg)
        {
            if (pkg != null && pkg.Size() > 0)
            { // Check that our Package is not null, and if it isn't check that it isn't empty.
                ZNetPeer peer = ZNet.instance.GetPeer(sender); // Get the Peer from the sender, to later check the SteamID against our Adminlist.
                if (peer != null)
                { // Confirm the peer exists
                    string peerSteamID = ((ZSteamSocket)peer.m_socket).GetPeerID().m_SteamID.ToString(); // Get the SteamID from peer.
                    if (
                        ZNet.instance.m_adminList != null &&
                        ZNet.instance.m_adminList.Contains(peerSteamID)
                    )
                    { // Check that the SteamID is in our Admin List.
                        // set server tod
                        String tod_str = pkg.ReadString();
                        if (tod_str == "sync")
                        {
                            pkg.Clear();
                            tod_str = EnvMan.instance.m_debugTime.ToString();
                            pkg.Write(tod_str);
                        }
                        float tod = float.Parse(tod_str);
                        
                        if (tod < 0f)
                        {
                            EnvMan.instance.m_debugTimeOfDay = false;
                        }
                        else
                        {
                            EnvMan.instance.m_debugTimeOfDay = true;
                            EnvMan.instance.m_debugTime = Mathf.Clamp01(tod);
                        }
                        pkg.SetPos(0);
                        string msg = pkg.ReadString(); // Read the message from the user.
                        pkg.SetPos(0); // Reset the position of our cursor so the client's can re-read the package.
                        ZRoutedRpc.instance.InvokeRoutedRPC(0L, "EventSetTime", new object[] { pkg }); // Send our Event to all Clients. 0L specifies that it will be sent to everybody
                        UnityEngine.Debug.Log("changing time of day...");
                    }
                } 
                else
                {
                    ZPackage newPkg = new ZPackage(); // Create a new ZPackage.
                    newPkg.Write("You aren't an Admin!"); // Tell them what's going on.
                    ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequestMsg", new object[] { newPkg }); // Send the error message.
                }
            }
        }
        
        public static void RPC_EventSetTime(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_RequestSync(long sender, ZPackage pkg)
        {
            ZNetPeer peer = ZNet.instance.GetPeer(sender); // Get the Peer from the sender
            if (peer != null)
            { // Confirm the peer exists
                ZPackage timePkg = new ZPackage();
                String tod_str = EnvMan.instance.m_debugTime.ToString();
                timePkg.Write(tod_str);

                ZPackage weatherPkg = new ZPackage();
                String env_str = EnvMan.instance.m_debugEnv;
                weatherPkg.Write(env_str);
                
                UnityEngine.Debug.Log("syncing with clients...");
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "EventTestConnection", new object[] { new ZPackage() });
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "EventSetTime", new object[] { timePkg });
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "EventSetWeather", new object[] { weatherPkg });
                foreach (string name in GamanMaker.invisible_players)
                {
                    pkg.Write(name);
                    pkg.Write(false);
                    ZRoutedRpc.instance.InvokeRoutedRPC(sender, "EventSetVisible", new object[] { pkg });
                }
            } 
            else
            {
                ZPackage newPkg = new ZPackage(); // Create a new ZPackage.
                newPkg.Write("Peer doesn't exist"); // Tell them what's going on.
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequestMsg", new object[] { newPkg }); // Send the error message.
            }
        }
        
        public static void RPC_EventSync(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_RequestAdminSync(long sender, ZPackage pkg)
        {
            ZNetPeer peer = ZNet.instance.GetPeer(sender); // Get the Peer from the sender, to later check the SteamID against our Adminlist.
            if (peer != null)
            { // Confirm the peer exists
                string peerSteamID = ((ZSteamSocket)peer.m_socket).GetPeerID().m_SteamID.ToString(); // Get the SteamID from peer.
                if (
                    ZNet.instance.m_adminList != null &&
                    ZNet.instance.m_adminList.Contains(peerSteamID)
                )
                { // Check that the SteamID is in our Admin List.
                    // set server tod
                    ZRoutedRpc.instance.InvokeRoutedRPC(sender, "EventAdminSync", new object[] { pkg });
                }
            } 
            else
            {
                ZPackage newPkg = new ZPackage(); // Create a new ZPackage.
                newPkg.Write("You aren't an Admin!"); // Tell them what's going on.
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequestMsg", new object[] { newPkg }); // Send the error message.
            }
        }
        
        public static void RPC_EventAdminSync(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_RequestSetVisible(long sender, ZPackage pkg)
        {
            ZNetPeer peer = ZNet.instance.GetPeer(sender); // Get the Peer from the sender, to later check the SteamID against our Adminlist.
            if (peer != null)
            { // Confirm the peer exists
                string peerSteamID = ((ZSteamSocket)peer.m_socket).GetPeerID().m_SteamID.ToString(); // Get the SteamID from peer.
                if (
                    ZNet.instance.m_adminList != null &&
                    ZNet.instance.m_adminList.Contains(peerSteamID)
                )
                { // Check that the SteamID is in our Admin List.
                    // set server tod
                    string name = pkg.ReadString();
                    bool vis = pkg.ReadBool();

                    if (!vis && !GamanMaker.invisible_players.Contains(name))
                    {
                        GamanMaker.invisible_players.Add(name);
                    }
                    else if (vis && GamanMaker.invisible_players.Contains(name))
                    {
                        GamanMaker.invisible_players.Remove(name);
                    }

                    pkg.SetPos(0);
                    ZRoutedRpc.instance.InvokeRoutedRPC(0L, "EventSetVisible", new object[] { pkg });
                }
            } 
            else
            {
                ZPackage newPkg = new ZPackage(); // Create a new ZPackage.
                newPkg.Write("You aren't an Admin!"); // Tell them what's going on.
                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequestMsg", new object[] { newPkg }); // Send the error message.
            }
        }
        
        public static void RPC_EventSetVisible(long sender, ZPackage pkg)
        {
            return;
        }
    }
}