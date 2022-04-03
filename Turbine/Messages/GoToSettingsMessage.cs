using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Turbine.Messages
{
    public class GoToSettingsMessage : ValueChangedMessage<bool>
    {
        public GoToSettingsMessage(bool state) : base(state)
        {

        }
    }
}
