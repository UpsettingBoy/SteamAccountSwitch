using Microsoft.Toolkit.Mvvm.Messaging.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitch.Messages
{
    public class GoToSettingsMessage : ValueChangedMessage<bool>
    {
        public GoToSettingsMessage(bool state) : base(state)
        {

        }
    }
}
