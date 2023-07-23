using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static herodesknew.Domain.Erros.DomainErrors;
using herodesknew.Domain.Entities;

namespace herodesknew.Infrastructure.Data.Configurations
{
    internal sealed class PullRequestConfiguration : IEntityTypeConfiguration<PullRequest>
    {
        public void Configure(EntityTypeBuilder<PullRequest> builder)
        {            
            builder.HasKey(x => x.Id);
        }
    }
}
