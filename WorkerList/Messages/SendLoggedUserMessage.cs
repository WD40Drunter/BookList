using BookList.Model;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookList.Messages;
public class SendLoggedUserMessage : ValueChangedMessage<User>
{
    public SendLoggedUserMessage(User value) : base(value)
    {
    }
}