using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public required int IdDepartment { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int SlaUsed { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public required StatusEnum Status { get; set; }
        public IEnumerable<Attachment>? Attachments { get; set; }
        public IEnumerable<Solution>? Solutions { get; set; }
    }
}
