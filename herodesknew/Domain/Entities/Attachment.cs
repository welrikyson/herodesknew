using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        //TODO: Usar FIle {Name, Path, Content}
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] FileContent { get; set; }
        public int TicketId { get; set; }
    }
}
