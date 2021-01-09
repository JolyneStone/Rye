using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Demo.DataAccess.EFCore.Models;

#nullable disable

namespace Demo.DataAccess.EFCore.DbContexts
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppInfo> AppInfo { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;database=MonicaDemo;uid=root;pwd=Mysql_zzq123;pooling=false;sslmode=None;charset=utf8mb4;port=3306", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppInfo>(entity =>
            {
                entity.HasKey(e => e.AppId)
                    .HasName("PRIMARY");

                entity.ToTable("appInfo");

                entity.Property(e => e.AppId).HasColumnName("appId");

                entity.Property(e => e.AppKey)
                    .IsRequired()
                    .HasColumnType("varchar(2000)")
                    .HasColumnName("appKey")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.AppSecret)
                    .IsRequired()
                    .HasColumnType("varchar(2000)")
                    .HasColumnName("appSecret")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("createTime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasColumnType("varchar(2000)")
                    .HasColumnName("remark")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId)
                    .HasColumnName("appId")
                    .HasComment("应用Id");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasColumnType("varchar(1024)")
                    .HasColumnName("icon")
                    .HasComment("图标")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("name")
                    .HasComment("菜单名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentId")
                    .HasComment("父级菜单Id");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("path")
                    .HasComment("路径")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Sort)
                    .HasColumnName("sort")
                    .HasComment("排序");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("状态");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("updateTime")
                    .HasComment("更新时间");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permission");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menuId")
                    .HasComment("菜单Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("name")
                    .HasComment("权限名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("权限状态");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("updateTime")
                    .HasComment("更新时间");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createTime")
                    .HasComment("创建时间");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("name")
                    .HasComment("角色名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasColumnType("varchar(512)")
                    .HasColumnName("remarks")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("userInfo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId)
                    .HasColumnName("appId")
                    .HasComment("应用Id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasColumnName("email")
                    .HasDefaultValueSql("''")
                    .HasComment("邮箱")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Lock)
                    .HasColumnType("bit(1)")
                    .HasColumnName("lock")
                    .HasComment("是否锁定");

                entity.Property(e => e.LockTime)
                    .HasColumnType("datetime")
                    .HasColumnName("lockTime")
                    .HasComment("锁定时间");

                entity.Property(e => e.Nickame)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("nickame")
                    .HasComment("昵称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("password")
                    .HasComment("密码")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Phome)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("phome")
                    .HasDefaultValueSql("''")
                    .HasComment("手机号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ProfilePicture)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasColumnName("profilePicture")
                    .HasComment("头像地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisterTime)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("registerTime")
                    .HasComment("注册时间");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("状态");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updateTime")
                    .HasComment("更新时间");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("userRole");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasComment("用户Id");

                entity.Property(e => e.RoleId)
                    .HasColumnName("roleId")
                    .HasComment("角色Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
