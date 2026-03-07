using System;

namespace CabinetOS.App.Services
{
    public static class NetworkManager
    {
        // -----------------------------
        // PLACEHOLDER STATE
        // -----------------------------
        private static bool _directLink = false;
        private static bool _lan = false;
        private static bool _wifi = true;
        private static int _wifiStrength = 3; // 0–4

        // -----------------------------
        // PUBLIC API (used by MainWindow)
        // -----------------------------
        public static bool IsDirectLinkConnected()
        {
            return _directLink;
        }

        public static bool IsLanConnected()
        {
            return _lan;
        }

        public static bool IsWifiConnected()
        {
            return _wifi;
        }

        public static int GetWifiStrength()
        {
            return _wifiStrength;
        }

        // -----------------------------
        // OPTIONAL: Allow updates
        // -----------------------------
        public static void SetDirectLink(bool active)
        {
            _directLink = active;
        }

        public static void SetLan(bool active)
        {
            _lan = active;
        }

        public static void SetWifi(bool active, int strength = 0)
        {
            _wifi = active;
            _wifiStrength = strength;
        }
    }
}