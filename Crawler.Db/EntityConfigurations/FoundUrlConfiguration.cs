using Crawler.UrlRepository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.UrlRepository.EntityConfigurations
{
    public class FoundUrlConfiguration : IEntityTypeConfiguration<FoundUrl>
    {
        public void Configure(EntityTypeBuilder<FoundUrl> builder)
        {
            builder.HasOne(u => u.InitialUrl)
                .WithMany(x => x.FoundUrls)
                .HasForeignKey(u => u.InitialUrlId);
        }
    }
}
