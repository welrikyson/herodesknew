using herodesknew.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
