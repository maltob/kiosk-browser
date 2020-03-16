using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kiosky.Lockdown.Support
{
    /// <summary>
    /// Used to configure GPO settings such as disabling windows features
    /// </summary>
    class GPOSettings
    {
        int? _initialDisableTaskMgr;
        public GPOSettings()
        {
            RecordInitialSettings();

        }

        private void RecordInitialSettings()
        {
            //Check if any policies are set
            try
            {

                _initialDisableTaskMgr = GetRegistryInt(systemSubkey, "DisableTaskMgr");

            }
            catch (Exception e)
            {
                Logger.DefaultLogger.LogWarning(String.Format("Error recording taskmanager policy\r\n{0}\r\n{1}", e.StackTrace, e.Message));
            }
        }
        /// <summary>
        /// Restores the GPO settings back to what they were when the program launched
        /// </summary>
        private void RestoreSettings()
        {

            if (_initialDisableTaskMgr != null)
            {
                DisableTaskMgr = _initialDisableTaskMgr == 1 ? true : false;
            }
        }

        /// <summary>
        /// Set to true to disable taskmgr, false to enable
        /// </summary>
        public bool DisableTaskMgr
        {
            get
            {
                return (GetRegistryInt(systemSubkey, "DisableTaskMgr") == 1);
            }
            set
            {
                SetRegistryInt(systemSubkey, "DisableTaskMgr", value ? 1 : 0);
            }
        }

        //Restore registry settings
        ~GPOSettings()
        {
            RestoreSettings();
        }


        private const string systemSubkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

        /// <summary>
        /// Returns the value of a dword in the registry
        /// </summary>
        /// <param name="path">HKCU path to open</param>
        /// <param name="keyname">The key to read </param>
        /// <returns></returns>
        private int? GetRegistryInt(string path, string keyname)
        {
            try
            {
                var SystemPolicies = Registry.CurrentUser.OpenSubKey(systemSubkey);
                if (SystemPolicies != null)
                {
                    var registryValueO = SystemPolicies.GetValue(keyname);
                    int tmpValue;
                    if ((registryValueO != null) && Int32.TryParse(registryValueO.ToString(), out tmpValue))
                    {
                        return tmpValue;
                    }
                    SystemPolicies.Close();
                };
            }
            catch (UnauthorizedAccessException uae)
            {
                Logger.DefaultLogger.LogWarning(String.Format("Failed to read registry key for lockdown policy.\r\n{0}", uae.Message));
            }
            catch (Exception e)
            {
                Logger.DefaultLogger.LogWarning(String.Format("{0}\r\n{1}", e.StackTrace, e.Message));
            }
            return null;
        }
        /// <summary>
        /// Set a HKCU dword value
        /// </summary>
        /// <param name="path">The key path under HKCU</param>
        /// <param name="keyname">Name of the value to set</param>
        /// <param name="value">The int value to set the dword</param>
        private void SetRegistryInt(string path, string keyname, int value)
        {

            try
            {
                // Open or create the key
                var SystemPolicies = Registry.CurrentUser.OpenSubKey(path, true);
                if (SystemPolicies == null)
                {
                    SystemPolicies = Registry.CurrentUser.CreateSubKey(path, true);
                }

                //Restore the value
                if (SystemPolicies != null)
                {
                    SystemPolicies.SetValue(keyname, value, RegistryValueKind.DWord);
                    SystemPolicies.Close();
                }

            
            }catch(UnauthorizedAccessException uae) {
                Logger.DefaultLogger.LogError(String.Format("Failed to set registry key for lockdown policy.\r\n{0}",uae.Message));
            }
            catch (Exception e)
            {
                Logger.DefaultLogger.LogWarning(String.Format("{0}\r\n{1}",e.StackTrace,e.Message));
            }
        }
    }
}
