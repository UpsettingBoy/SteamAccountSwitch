using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml;

namespace Turbine.Messages
{
    public class UpdateAppThemeMessage : ValueChangedMessage<ElementTheme>
    {
        public UpdateAppThemeMessage(ElementTheme theme) : base(theme)
        {

        }
    }
}
