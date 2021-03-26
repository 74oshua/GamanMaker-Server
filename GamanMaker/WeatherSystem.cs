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
    }
}