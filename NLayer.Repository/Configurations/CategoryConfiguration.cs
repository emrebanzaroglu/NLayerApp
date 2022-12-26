using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id); //id primary key olsun
            builder.Property(x => x.Id).UseIdentityColumn();  //id 1er 1er artsın
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);  //boş olmasın ve max length 50 olsun
            builder.ToTable("Categories");  //tablo ismini belirler

        }
    }
}
