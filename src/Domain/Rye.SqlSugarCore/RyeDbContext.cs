using SqlSugar;

namespace Rye.SqlSugarCore
{
    public class RyeDbContext
    {
        /// <summary>
        /// 数据库上下文
        /// <para>用来处理事务多表查询和复杂的操作</para>
        /// </summary>
        public SqlSugarClientExtend Db;

        #region 数据库连接字符串

        /// <summary>
        /// 是否为调试模式
        /// </summary>
        private readonly bool _isDebug = false;

        /// <summary>
        /// 定义一个私有锁对象
        /// </summary>
        protected static readonly object _lockObject = new();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected static Dictionary<string, string> ConnectionDict = new();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public virtual string GetConnectionString(string connectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                return null;
            }

            //校验字典是否存在的连接字符串
            if (!ConnectionDict.ContainsKey(connectionName))
            {
                var connectionString = string.Empty;
                //校验字典是否存在的连接字符串
                if (!ConnectionDict.ContainsKey(connectionName))
                {
                    //从配置表读取连接字符串
                    connectionString = App.GetConfig<string>($"ConnectionStrings:{connectionName}");
                    if (!string.IsNullOrWhiteSpace(connectionString))
                    {
                        lock (_lockObject)
                        {
                            if (!ConnectionDict.ContainsKey(connectionName))
                                ConnectionDict.Add(connectionName, connectionString);
                        }
                    }
                }
                return connectionString;
            }

            return ConnectionDict[connectionName];
        }


        #endregion 数据库连接字符串

        #region 数据库上下文构造

        /// <summary>
        /// 数据库上下文构造
        /// </summary>
        public RyeDbContext() : this("Default", "Reading")
        {
            
        }

        /// <summary>
        /// 数据库上下文构造
        /// </summary>
        public RyeDbContext(string connectionName, string readConnectionName = null)
        {
            _isDebug = string.Equals(GetConnectionString("Debug"), "true", StringComparison.InvariantCultureIgnoreCase);
            //主从数据库信息
            string ConnectionString = GetConnectionString(connectionName);
            var myDbType = GetDbType(ConnectionString);
            string ReadConnectionString = GetConnectionString(readConnectionName);
            if (ReadConnectionString.IsNullOrEmpty())
            {
                //如果只有一个数据库，就把主从数据库配置成一个
                ReadConnectionString = ConnectionString;
            }

            Db = new SqlSugarClientExtend(new ConnectionConfig()
            {
                //主连接
                ConnectionString = ConnectionString,
                DbType = GetDbType(ConnectionString),
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
                MoreSettings = new ConnMoreSettings()
                {
                    IsNoReadXmlDescription = true,
                    IsWithNoLockQuery = true,
                    IsWithNoLockSubquery = true,
                    DisableNvarchar = true,
                },
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityNameService = (type, entity) =>
                    {
                        entity.IsDisabledDelete = true;
                    },
                    //表当中的每个字段配置信息
                    EntityService = (c, p) =>
                    {
                        //oracle 数据库相关配置
                        if (myDbType == SqlSugar.DbType.Oracle)
                        {
                            //长度修改
                            //if (p.Length > 4000 && p.UnderType == typeof(string))
                            //{
                            //    //p.DataType = "CLOB";
                            //    p.Length = 4000;
                            //}

                            if (p.DbColumnName == "ID" && (p.UnderType == typeof(Int32) || p.UnderType == typeof(long)))
                            {
                                //手动触发器，只触发一次，不会在数据库中生成触发器
                                p.OracleSequenceName = p.DbTableName + "_SEQ_ID";
                            }

                            //timestamp
                            if (p.DbColumnName == "TIMESTAMPS")
                            {
                                p.IsIgnore = true;
                            }

                            //if (p.DbColumnName == "DATE")
                            //{
                            //    p.DbColumnName = p.DbTableName+"."+p.DbColumnName;
                            //}
                        }
                        else if (myDbType == SqlSugar.DbType.SqlServer)
                        {
                            if (p.DbColumnName == "SIGNATURE")
                            {
                                p.IsIgnore = true;
                            }
                        }
                    }
                },
                //从库
                SlaveConnectionConfigs = new List<SlaveConnectionConfig>()
                {
                    //这里可以配置多个从库
                    new SlaveConnectionConfig()
                    {
                        HitRate = 10,
                        ConnectionString = ReadConnectionString
                    }
                }
            });

            //SQL调试代码设置
            OnLogExecutingSetting();
            //SQL执行前事件
            DataExecutingSetting();
            //查询过滤器设置
            QueryFilterSetting();
            //删除过滤器设置
            QueryFilterIsDeleted();
        }

        #endregion 数据库上下文构造

        #region 按照数据库链接字符串返回数据库链接类型

        /// <summary>
        /// 按照数据库链接字符串返回数据库链接类型
        /// </summary>
        /// <param name="connstr">数据库链接字符串</param>
        /// <returns></returns>
        protected virtual SqlSugar.DbType GetDbType(string connstr)
        {
            var dbType = SqlSugar.DbType.SqlServer;
            if (connstr.StartsWith("DataSource=", StringComparison.Ordinal) && connstr.Contains("sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                dbType = SqlSugar.DbType.Sqlite;
            }
            else if (connstr.StartsWith("Data Source=", StringComparison.InvariantCultureIgnoreCase))
            {
                dbType = SqlSugar.DbType.Oracle;
            }
            else if (connstr.Contains("Database=", StringComparison.InvariantCultureIgnoreCase) && connstr.Contains("Uid=", StringComparison.Ordinal) && connstr.Contains("Pwd=", StringComparison.Ordinal))
            {
                dbType = SqlSugar.DbType.MySql;
            }
            return dbType;
        }

        #endregion 按照数据库链接字符串返回数据库链接类型

        #region 调式代码用来打印SQL

        /// <summary>
        /// 调式代码用来打印SQL
        /// </summary>
        public virtual void OnLogExecutingSetting()
        {
            if (!_isDebug) { return; }

            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }

        #endregion 调式代码用来打印SQL

        #region SQL执行前事件

        /// <summary>
        /// SQL执行前事件
        /// </summary>
        public virtual void DataExecutingSetting()
        {
        }

        #endregion SQL执行前事件

        #region 查询过滤器设置

        /// <summary>
        /// 查询过滤器设置
        /// </summary>
        public virtual void QueryFilterSetting()
        {
        }

        #endregion 查询过滤器设置

        #region 返回字段格式化

        /// <summary>
        /// 返回字段格式化
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected virtual string GetFieldFormat(string field)
        {
            switch (Db.CurrentConnectionConfig.DbType)
            {
                case SqlSugar.DbType.MySql:
                    return $"`{field}`";
                case SqlSugar.DbType.Oracle:
                    return $"{field}";
                case SqlSugar.DbType.SqlServer:
                    return $"[{field}]";
                case SqlSugar.DbType.PostgreSQL:
                    return $"\"{field}\"";
                case SqlSugar.DbType.Sqlite:
                    return $"\"{field}\"";
                default:
                    return $"{field}";
            }
        }

        #endregion 返回字段格式化

        #region 删除过滤器设置

        /// <summary>
        /// 删除过滤器设置
        /// </summary>
        public virtual void QueryFilterIsDeleted(bool isDeleted = false)
        {
            //Db.QueryFilter.Add(new SqlSugar.TableFilterItem<xxx>(it => it.IsDeleted == isDeleted));
        }

        #endregion 删除过滤器设置


    }
}
