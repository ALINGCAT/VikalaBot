// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VikalaBot.Database;

#nullable disable

namespace VikalaBot.Migrations
{
    [DbContext(typeof(VikalaDbContext))]
    partial class VikalaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("VikalaBot.Database.Entities.DrawInfo", b =>
                {
                    b.Property<long>("QQ")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("Crystal")
                        .HasColumnType("int");

                    b.Property<int>("Draw")
                        .HasColumnType("int");

                    b.Property<int>("DrawTen")
                        .HasColumnType("int");

                    b.HasKey("QQ");

                    b.ToTable("DrawInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
