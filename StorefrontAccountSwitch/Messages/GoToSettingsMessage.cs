using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StorefrontAccountSwitch.Messages
{
    public class GoToSettingsMessage : ValueChangedMessage<bool>
    {
        public GoToSettingsMessage(bool state) : base(state)
        {

        }
    }
}
