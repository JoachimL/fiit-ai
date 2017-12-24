using Bodybuildr.Domain;
using MediatR;

namespace Bodybuildr.Domain
{
    public class Event : Message, INotification
    {
        public int Version;
    }
}
