using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain
{
    public class Command : Message, IRequest
    {
    }
}
