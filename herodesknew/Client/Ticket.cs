using System;
using System.Net.Mail;

namespace herodesknew.Client
{
    public class Ticket
    {
        public int Id { get; set; }
        public required int IdDepartment { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int SlaUsed { get; set; }
        public bool IsClosed { get; set; }
       
    }
}

