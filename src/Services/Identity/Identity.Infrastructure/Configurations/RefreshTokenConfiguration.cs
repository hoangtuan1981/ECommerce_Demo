using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    //public void Configure_temp(EntityTypeBuilder<RefreshToken> builder)
    //{
    //    builder.ToTable("RefreshTokens");

    //    builder.HasKey(x => x.Id);

    //    builder.Property(x => x.Token)
    //        .HasMaxLength(500)
    //        .IsRequired();

    //    builder.HasIndex(x => x.Token)
    //        .IsUnique();

    //    builder.Property(x => x.ExpireAt)
    //        .IsRequired();

    //    builder.Property(x => x.RowVersion)
    //        .IsRowVersion();

    //    builder.HasOne(x => x.User)
    //        .WithMany(x => x.RefreshTokens)
    //        .HasForeignKey(x => x.UserId);
    //}


    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Token
        builder.Property(x => x.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique();

        // Expiration
        builder.Property(x => x.ExpirationDate)
            .IsRequired();

        //// Revoked
        //builder.Property(x => x.IsRevoked)
        //    .HasDefaultValue(false)
        //    .IsRequired();

        // Optimistic Concurrency
        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        // Relationship
        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query Performance
        builder.HasIndex(x => x.UserId);

        //builder.HasIndex(x => new
        //{
        //    x.UserId,
        //    x.IsRevoked
        //});

        builder.HasIndex(x => x.ExpirationDate);
    }
}