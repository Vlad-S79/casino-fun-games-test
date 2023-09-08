using System;
using UnityEngine;

namespace Core.Common
{
    public static class DeviceID
    {
        private static string _deviceId;
        private static string _key = "device_unique_id_";
        
#if UNITY_EDITOR
        public static void SetUserId(int id)
        {
            _key += id.ToString();
        }
#endif
        
        public static string Get()
        {
            if (_deviceId == null)
            {
                _deviceId = GetDeviceId();
            }

            return _deviceId;
        }

        private static string GetDeviceId()
        {
            var deviceId = SystemInfo.deviceUniqueIdentifier;

            if (IsInvalidDeviceId(deviceId))
            {
                if (PlayerPrefs.HasKey(_key))
                {
                    return PlayerPrefs.GetString(_key);
                }

                deviceId = GenerateUUID();
                
                PlayerPrefs.SetString(_key, deviceId);
                PlayerPrefs.Save();

                return deviceId;
            }

            return deviceId;
        }

        private static string GenerateUUID()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var timestampBytes = BitConverter.GetBytes(timestamp);
            
            timestampBytes[7] &= 0x0F;
            timestampBytes[8] &= 0x3F;
            
            var uuid = new Guid(timestampBytes);
            
            return uuid.ToString();
        }

        private static bool IsInvalidDeviceId(string deviceId)
        {
            return string.IsNullOrEmpty(deviceId);
        }
    }
}