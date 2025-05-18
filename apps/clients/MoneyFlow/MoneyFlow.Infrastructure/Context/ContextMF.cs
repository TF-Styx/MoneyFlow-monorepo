using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoneyFlow.Infrastructure.EntityModel;

namespace MoneyFlow.Infrastructure.Context;

public partial class ContextMF : DbContext
{
    public ContextMF()
    {
    }

    public ContextMF(DbContextOptions<ContextMF> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<CatLinkSub> CatLinkSubs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FinancialRecord> FinancialRecords { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=STYX;Database=MoneyFlowDB;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount);

            entity.Property(e => e.IdAccount).HasColumnName("id_account");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("balance");
            entity.Property(e => e.IdAccountType).HasColumnName("id_account_type");
            entity.Property(e => e.IdBank).HasColumnName("id_bank");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.NumberAccount).HasColumnName("number_account");

            entity.HasOne(d => d.IdAccountTypeNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdAccountType)
                .HasConstraintName("FK_Accounts_Account_types");

            entity.HasOne(d => d.IdBankNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdBank)
                .HasConstraintName("FK_Accounts_Banks");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Accounts_User");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.IdAccountType);

            entity.ToTable("Account_types");

            entity.Property(e => e.IdAccountType).HasColumnName("id_account_type");
            entity.Property(e => e.AccountTypeName)
                .HasMaxLength(50)
                .HasColumnName("account_type_name");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.IdBank);

            entity.Property(e => e.IdBank).HasColumnName("id_bank");
            entity.Property(e => e.BankName).HasColumnName("bank_name");
        });

        modelBuilder.Entity<CatLinkSub>(entity =>
        {
            entity.HasKey(e => new { e.IdCategory, e.IdSubcategory }).HasName("PK_Cat_Link_Sub_1");

            entity.ToTable("Cat_Link_Sub");

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdSubcategory).HasColumnName("id_subcategory");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.CatLinkSubs)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cat_Link_Sub_Categories");

            entity.HasOne(d => d.IdSubcategoryNavigation).WithMany(p => p.CatLinkSubs)
                .HasForeignKey(d => d.IdSubcategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cat_Link_Sub_Subcategories");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Image).HasColumnName("image");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Categories)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categories_User");
        });

        modelBuilder.Entity<FinancialRecord>(entity =>
        {
            entity.HasKey(e => e.IdFinancialRecord);

            entity.ToTable("FInancial_records");

            entity.Property(e => e.IdFinancialRecord).HasColumnName("id_financial_record");
            entity.Property(e => e.Ammount)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("ammount");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdAccount).HasColumnName("id_account");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdSubcategory).HasColumnName("id_subcategory");
            entity.Property(e => e.IdTransactionType).HasColumnName("id_transaction_type");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.RecordName)
                .HasMaxLength(50)
                .HasColumnName("record_name");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.FinancialRecords)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK_FInancial_records_Accounts");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.FinancialRecords)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_FInancial_records_Categories1");

            entity.HasOne(d => d.IdSubcategoryNavigation).WithMany(p => p.FinancialRecords)
                .HasForeignKey(d => d.IdSubcategory)
                .HasConstraintName("FK_FInancial_records_Subcategories");

            entity.HasOne(d => d.IdTransactionTypeNavigation).WithMany(p => p.FinancialRecords)
                .HasForeignKey(d => d.IdTransactionType)
                .HasConstraintName("FK_FInancial_records_Transaction_types");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FinancialRecords)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_FInancial_records_User");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.IdGender);

            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.GenderName)
                .HasMaxLength(50)
                .HasColumnName("gender_name");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.IdSubcategory);

            entity.Property(e => e.IdSubcategory).HasColumnName("id_subcategory");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.SubcategoryName)
                .HasMaxLength(50)
                .HasColumnName("subcategory_name");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.IdTransactionType);

            entity.ToTable("Transaction_types");

            entity.Property(e => e.IdTransactionType).HasColumnName("id_transaction_type");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(50)
                .HasColumnName("transaction_type_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("User");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");

            entity.HasOne(d => d.IdGenderNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdGender)
                .HasConstraintName("FK_User_Genders");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
