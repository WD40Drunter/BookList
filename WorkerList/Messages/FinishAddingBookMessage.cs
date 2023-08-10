using BookList.Model;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookList.Messages;
public class FinishAddingBookMessage : ValueChangedMessage<Book>
{
    public FinishAddingBookMessage(Book value) : base(value)
    {
    }
}
