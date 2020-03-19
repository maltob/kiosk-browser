using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Lockdown
{
    class WindowsLockdown
    {
        Support.KeyboardHook kbd;
        Support.GPOSettings gpos;
        public WindowsLockdown(bool DisableTaskManager = true, bool DisableAltTab=true)
        {

            //If we want Alt Tab disabled, setup the hook to do so
            if(DisableAltTab)
            {
                kbd = new Support.KeyboardHook();
                kbd.SetHook();
            }

            if(DisableTaskManager)
            {
                gpos = new Support.GPOSettings
                {
                    DisableTaskMgr = true
                };
            }
            
        }
        ~WindowsLockdown()
        {
            if(kbd != null)
                kbd.Dispose();

        }
    }
}
