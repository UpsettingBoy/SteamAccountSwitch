using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitch.Messages
{
    public class UpdateAppThemeMessage : ValueChangedMessage<ElementTheme>
    {
        public UpdateAppThemeMessage(ElementTheme theme) : base(theme)
        {

        }
    }
}
