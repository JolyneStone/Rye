using System;

namespace Demo.DataAccess
{
    [Serializable]
    public partial class UserInfo
    {
		/// <summary>
		/// 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 昵称
		/// </summary>
		public string Nickame { get; set; }
		/// <summary>
		/// 手机号
		/// </summary>
		public string Phone { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public byte Status { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime RegisterTime { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UpdateTime { get; set; }
		/// <summary>
		/// 是否锁定
		/// </summary>
		public bool Lock { get; set; }
		/// <summary>
		/// 锁定时间
		/// </summary>
		public DateTime? LockTime { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// 应用Id
		/// </summary>
		public int AppId { get; set; }
		/// <summary>
		/// 头像地址
		/// </summary>
		public string ProfilePicture { get; set; }
    }
}