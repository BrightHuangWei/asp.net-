namespace OnlineHotelReservationSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class HotelDatabase : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“HotelDatabase”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“OnlineHotelReservationSystem.Models.HotelDatabase”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“HotelDatabase”
        //连接字符串。
        public HotelDatabase()
            : base("name=HotelDatabase")
        {
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<SysAdmin> SysAdmins { get; set; }
        public virtual DbSet<Suite> Suites { get; set; }
        public virtual DbSet<ReserveInfo> ReserveInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sysAdminTable = modelBuilder.Entity<SysAdmin>();
            var suiteTable = modelBuilder.Entity<Suite>();
            var reserveInfoTable = modelBuilder.Entity<ReserveInfo>();

            sysAdminTable.HasKey(o => o.SysAdminId);
            suiteTable.HasKey(o => o.SuiteId);
            reserveInfoTable.HasKey(o => o.OrderId);

            base.OnModelCreating(modelBuilder);
        }

    }
    /// <summary>
    /// 用户表
    /// </summary>
    public class SysAdmin
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        public int SysAdminId { get; set; }
        /// <summary>
        /// 注册校验码
        /// </summary>
        [Required]
        public string SysAdminIdCode { get; set; }
        /// <summary>
        /// 管理员名
        /// </summary>
        [Required]
        public string SysAdminName { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [Required]
        public string SysAdminPassword { get; set; }
    }
    public class Suite
    {
        /// <summary>
        /// 套房id
        /// </summary>
        public int SuiteId { get; set; }
        /// <summary>
        /// 套房类型
        /// </summary>
        [Required]
        public string SuiteType { get; set; }
        /// <summary>
        /// 套房价格
        /// </summary>
        [Required]
        public int SuitePrice { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        [Required]
        public int TotalNumber { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Required]
        public int OddNumber { get; set; }
    }
    public class ReserveInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 预定用户名 ，即 登录账户，从Identity.User表读取
        /// </summary>
        //public int UserId{ get;set;}
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 预订人
        /// </summary>
        [Required]
        public string OrderName { get; set; }
        /// <summary>
        /// 预订人身份证号
        /// </summary>
        [Required]
        public string OrderIdCard { get; set; }
        /// <summary>
        /// 预定时间
        /// </summary>
        [Required]
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 预定套房类型
        /// </summary>
        [Required]
        public int SuiteType { get; set; }
        /// <summary>
        /// 预订数量
        /// </summary>
        [Required]
        public int OrderNunber { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        [Required]
        public DateTime ArrivalTime { get; set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        [Required]
        public DateTime LeaveTime { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}