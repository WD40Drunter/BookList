using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookList.Messages
{
    public class StartAddingBookWindowMessage : ValueChangedMessage<string>
    {
        public StartAddingBookWindowMessage(string value) : base(value)
        {
            
        }
    }
}
