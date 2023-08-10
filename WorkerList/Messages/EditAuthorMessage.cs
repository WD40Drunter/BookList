using BookList.Model;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BookList.Messages;
public class EditAuthorMessage : ValueChangedMessage<Author>
{
    public EditAuthorMessage(Author value) : base(value)
    {
    }
}
