using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetWorld.Domain.Entities;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Infrastructure.Persistence.Configurations;

public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(message => message.Id);

        builder.Property(message => message.Question)
            .HasConversion(question => question.Value, value => Question.Create(value).Value)
            .HasMaxLength(Question.MaxLength)
            .IsRequired();

        builder.Property(message => message.Answer)
            .HasConversion(answer => answer.Value, value => Answer.Create(value).Value)
            .HasMaxLength(Answer.MaxLength)
            .IsRequired();

        builder.Property(message => message.IterationCount)
            .HasConversion(iteration => iteration.Value, value => IterationCount.Create(value).Value)
            .IsRequired();

        builder.Property(message => message.CreatedAt)
            .IsRequired();

        builder.Property(message => message.UpdatedAt);
    }
}
