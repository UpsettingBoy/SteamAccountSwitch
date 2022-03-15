using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace SteamAccountSwitch.Messages
{
    public class GoToSettingsMessage : ValueChangedMessage<bool>
    {
        public GoToSettingsMessage(bool state) : base(state)
        {

        }
    }
}
