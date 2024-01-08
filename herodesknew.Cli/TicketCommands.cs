using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocona;
using herodesknew.Application.Tickets.Queries.GetTicket;

namespace herodesknew.Cli
{
    internal class TicketCommands
    {
        private readonly GetTicketQueryHandler getTicketQueryHandler;

        public TicketCommands(GetTicketQueryHandler getTicketQueryHandler)
        {
            this.getTicketQueryHandler = getTicketQueryHandler;
        }
        [Command("list")]   
        
        public async Task ListAsync()
        {
            try
            {
                var result = await getTicketQueryHandler.Handle(new() { TicketId = 768048 });

                Console.WriteLine(result.Value.Description);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
