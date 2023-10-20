using System;
namespace MultilingualClient.Messages;

public class StartEditMessage : ValueChangedMessage<string>
{
    public StartEditMessage(string mess) : base(mess)
    {
    }
}

