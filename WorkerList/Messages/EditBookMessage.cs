using BookList.Model;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookList.Messages;
public class EditBookMessage : ValueChangedMessage<Book>
{
    public EditBookMessage(Book value) : base(value)
    {
    }
}

