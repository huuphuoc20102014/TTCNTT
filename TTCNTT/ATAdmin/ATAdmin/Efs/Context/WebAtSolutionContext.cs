using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ATAdmin.Efs.Entities
{
    public partial class WebAtSolutionContext : DbContext
    {
        internal string LoginUserId;

        public virtual DbSet<AboutCustomer> AboutCustomer { get; set; }
        public virtual DbSet<AboutUs> AboutUs { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Faq> Faq { get; set; }
        public virtual DbSet<ImageSlide> ImageSlide { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsType> NewsType { get; set; }
        public virtual DbSet<OperationHistory> OperationHistory { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductComment> ProductComment { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectImage> ProjectImage { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<SettingType> SettingType { get; set; }
        public virtual DbSet<TableVersion> TableVersion { get; set; }
        public virtual DbSet<ViewUserRole> ViewUserRole { get; set; }

        public WebAtSolutionContext(DbContextOptions<WebAtSolutionContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=115.78.100.42,8899;Database=ATSOLUTION;User Id=sa;Password=1@qweQAZ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AboutCustomer>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageSlug)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.LongDescriptionHtml)
                    .HasColumnName("LongDescription_Html")
                    .HasColumnType("ntext");

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ShortDescriptionHtml)
                    .HasColumnName("ShortDescription_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AboutUs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ImageSlug)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.LongDescriptionHtml)
                    .HasColumnName("LongDescription_Html")
                    .HasColumnType("ntext");

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ShortDescriptionHtml)
                    .HasColumnName("ShortDescription_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.Skill).HasMaxLength(500);

                entity.Property(e => e.SlugTitle)
                    .IsRequired()
                    .HasColumnName("Slug_Title")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Img).HasMaxLength(100);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkCategoryId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.InverseFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Adress).HasMaxLength(1000);

                entity.Property(e => e.Body).IsRequired();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FkProductCommentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Link).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQ");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Faqquestion)
                    .IsRequired()
                    .HasColumnName("FAQQuestion")
                    .HasMaxLength(1000);

                entity.Property(e => e.Faqreply)
                    .IsRequired()
                    .HasColumnName("FAQReply")
                    .HasMaxLength(2000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();
            });

            modelBuilder.Entity<ImageSlide>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.Thumbnail).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.YoutubeLink).HasMaxLength(500);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ActionName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AnotherLink)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ControlerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CssClass).HasMaxLength(200);

                entity.Property(e => e.FkMenuId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IconSlug).HasMaxLength(200);

                entity.Property(e => e.ImageSlug).HasMaxLength(200);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Coment)
                    .HasColumnName("coment")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkNewsTypeId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageSlug).HasMaxLength(100);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.LongDescriptionHtml)
                    .HasColumnName("LongDescription_Html")
                    .HasColumnType("ntext");

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ShortDescriptionHtml)
                    .HasColumnName("ShortDescription_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.SlugTitle)
                    .IsRequired()
                    .HasColumnName("Slug_Title")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkNewsType)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.FkNewsTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_News_NewsType");
            });

            modelBuilder.Entity<NewsType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<OperationHistory>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HistoryDescription)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<People>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BirthDay).HasColumnType("datetime");

                entity.Property(e => e.Gmail).HasMaxLength(100);

                entity.Property(e => e.Img)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Job)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.JobIntroduction)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ccy)
                    .HasColumnName("CCY")
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkProductId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageSlug)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.LongDescriptionHtml)
                    .HasColumnName("LongDescription_Html")
                    .HasColumnType("ntext");

                entity.Property(e => e.Material).HasMaxLength(50);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.Producer).HasMaxLength(500);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ShortDescriptionHtml)
                    .HasColumnName("ShortDescription_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.Size).HasMaxLength(50);

                entity.Property(e => e.Sku)
                    .HasColumnName("SKU")
                    .HasMaxLength(50);

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SpecificationHtml)
                    .HasColumnName("Specification_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Style).HasMaxLength(50);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkProduct)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.FkProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<ProductComment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FkProductCommentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FkProductId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkProductComment)
                    .WithMany(p => p.InverseFkProductComment)
                    .HasForeignKey(d => d.FkProductCommentId)
                    .HasConstraintName("FK_ProductComment_ProductComment");

                entity.HasOne(d => d.FkProduct)
                    .WithMany(p => p.ProductComment)
                    .HasForeignKey(d => d.FkProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductComment_Product");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FkProductId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.Thumbnail).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.YoutubeLink).HasMaxLength(500);

                entity.HasOne(d => d.FkProduct)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(d => d.FkProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImage_Product");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkProjectTypeId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageSlug).HasMaxLength(100);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.LongDescriptionHtml)
                    .HasColumnName("LongDescription_Html")
                    .HasColumnType("ntext");

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ShortDescriptionHtml)
                    .HasColumnName("ShortDescription_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkProjectType)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.FkProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectType");
            });

            modelBuilder.Entity<ProjectImage>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FkProjectId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.Thumbnail).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.YoutubeLink).HasMaxLength(500);
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Icon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImageSlug).HasMaxLength(100);

                entity.Property(e => e.KeyWord).HasMaxLength(1000);

                entity.Property(e => e.LongDescriptionHtml)
                    .HasColumnName("LongDescription_Html")
                    .HasColumnType("ntext");

                entity.Property(e => e.MetaData).HasMaxLength(1000);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShortDescriptionHtml)
                    .HasColumnName("ShortDescription_Html")
                    .HasMaxLength(1000);

                entity.Property(e => e.SlugName)
                    .IsRequired()
                    .HasColumnName("Slug_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tags).HasMaxLength(1000);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.ImageSlug).HasMaxLength(200);

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.Property(e => e.Style).HasMaxLength(200);

                entity.Property(e => e.Value).IsRequired();

                entity.HasOne(d => d.StyleNavigation)
                    .WithMany(p => p.Setting)
                    .HasForeignKey(d => d.Style)
                    .HasConstraintName("FK_Setting_SettingType");
            });

            modelBuilder.Entity<SettingType>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();
            });

            modelBuilder.Entity<TableVersion>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModify).HasColumnType("datetime");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();
            });

            modelBuilder.Entity<ViewUserRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_User_Role");

                entity.Property(e => e.IdRole)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.TenNguoiDung).HasMaxLength(256);

                entity.Property(e => e.TenQuyen).HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
