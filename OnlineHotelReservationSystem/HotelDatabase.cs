namespace OnlineHotelReservationSystem
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class HotelDatabase : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“HotelDatabase”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“OnlineHotelReservationSystem.HotelDatabase”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“HotelDatabase”
        //连接字符串。
        public HotelDatabase()
            : base("name=HotelDatabase")
        {
        }
        // DefaultConnection HotelDatabase
        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<SysAdminLogin> SysAdminLogins { get; set; }
        public virtual DbSet<SysAdmin> SysAdmins { get; set; }
        public virtual DbSet<Suite> Suites { get; set; }
        public virtual DbSet<ReserveInfo> ReserveInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var customerTable = modelBuilder.Entity<Customer>();
            var sysAdminLoginTable = modelBuilder.Entity<SysAdminLogin>();
            var sysAdminTable = modelBuilder.Entity<SysAdmin>();
            var suiteTable = modelBuilder.Entity<Suite>();
            var reserveInfoTable = modelBuilder.Entity<ReserveInfo>();

            sysAdminTable.HasKey(o => o.SysAdminId);
            sysAdminLoginTable.HasKey( o => o.SysAdminName);
            suiteTable.HasKey(o => o.SuiteType);
            reserveInfoTable.HasKey(o => o.OrderId);

            base.OnModelCreating(modelBuilder);
        }

    }

    /// <summary>
    /// 用户等级表
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        //[Required(ErrorMessage = "会员等级不能为空！")]
        public string CustomerLevel { get; set; }
    }
    /// <summary>
    /// 管理员登录状态表
    /// </summary>
    public class SysAdminLogin
    {
        public string SysAdminName { get; set; }
        public string SysAdminLoginState { get; set; }
    }

    /// <summary>
    /// 管理员表
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
        ///[Required(ErrorMessage ="必须填写内部校验码！")]
        public string SysAdminIdCode { get; set; }
        /// <summary>
        /// 管理员名
        /// </summary>
        ///[Required(ErrorMessage ="管理员名不能为空！")]
        public string SysAdminName { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        ///[Required(ErrorMessage = "管理员密码不能为空！")]
        public string SysAdminPassword { get; set; }
    }
    public class Suite
    {
        
        /// <summary>
        /// 套房类型
        /// </summary>
        //[Required(ErrorMessage ="套房类型不能为空！")]
        public string SuiteType { get; set; }
        /// <summary>
        /// 套房价格
        /// </summary>
        //[Required(ErrorMessage = "套房价格不能为空！")]
        public int SuitePrice { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        //[Required(ErrorMessage = "套房总数量不能为空！")]
        public int TotalNumber { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        //[Required]
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
        //[Required]
        public string UserName { get; set; }
        /// <summary>
        /// 预订人
        /// </summary>
        //[Required(ErrorMessage ="预订人不能为空！")]
        public string OrderName { get; set; }
        /// <summary>
        /// 预订人联系电话
        /// </summary>
        //[Required(ErrorMessage = "联系电话不能为空！")]
        //[StringLength(maximumLength:15, MinimumLength =7)]
        public string Tel { get; set; }
        /// <summary>
        /// 预订人身份证号
        /// </summary>
        //[Required(ErrorMessage = "身份证号不能为空！")]
        //[StringLength(maximumLength:18,MinimumLength =18)]
        public string OrderIdCard { get; set; }
        /// <summary>
        /// 预定时间
        /// </summary>
        //[Required]
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 预定套房类型
        /// </summary>
        //[Required(ErrorMessage = "套房类型不能为空！")]
        public string SuiteType { get; set; }
        /// <summary>
        /// 预订数量
        /// </summary>
        //[Required(ErrorMessage = "预订不能为空！")]
        public int OrderNunber { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        //[Required(ErrorMessage = "到店时间不能为空！")]
        public string ArrivalTime { get; set; }
        /// <summary>
        /// 退房时间
        /// </summary>
        //[Required(ErrorMessage = "退房时间不能为空！")]
        public string LeaveTime { get; set; }
        /// <summary>
        /// 预定总价
        /// </summary>
        //[Required]
        public int TotalPrice { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}