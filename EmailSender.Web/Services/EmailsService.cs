using System.Collections.Generic;
using EmailSender.Web.Dtos;

namespace EmailSender.Web.Services
{
    public class EmailsService
    {
        private readonly LinkedList<Destination> _destinations = new();

        public void Add(Destination destination)
        {
            _destinations.AddLast(destination);
        }

        public ICollection<Destination> Get()
        {
            return _destinations;
        }
    }
}