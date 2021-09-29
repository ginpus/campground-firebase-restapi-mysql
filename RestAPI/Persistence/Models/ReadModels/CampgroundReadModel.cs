using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class CampgroundReadModel
    {
        public Guid CampgroundId { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public IEnumerable<ImageReadModel> Images { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
