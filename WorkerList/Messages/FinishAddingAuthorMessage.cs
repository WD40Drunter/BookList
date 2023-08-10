using BookList.Model;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookList.Messages;
public class FinishAddingAuthorMessage : ValueChangedMessage<Author>
{
    public FinishAddingAuthorMessage(Author value) : base(value)
    {
    }
}
