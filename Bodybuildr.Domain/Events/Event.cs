using Bodybuildr.Domain;
using MediatR;

namespace Bodybuildr.CommandStack.Events
{
    public class Event : Message, INotification
    {
        public int Version;
    }
}
