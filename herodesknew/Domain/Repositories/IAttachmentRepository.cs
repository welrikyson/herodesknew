﻿using herodesknew.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Repositories;

public interface IAttachmentRepository
{
    Task<Attachment?> GetAttachmentBy(int attachmentID);
}
