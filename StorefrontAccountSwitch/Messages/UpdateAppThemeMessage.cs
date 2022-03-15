using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml;

namespace StorefrontAccountSwitch.Messages
{
    public class UpdateAppThemeMessage : ValueChangedMessage<ElementTheme>
    {
        public UpdateAppThemeMessage(ElementTheme theme) : base(theme)
        {

        }
    }
}
