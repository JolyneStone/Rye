# Rye Framework

## Rye框架简介

Rye是基于.NET 5开发的一个快速开发框架，对AOP、依赖注入、日志、缓存、EF Core、身份认证、消息队列、代码生成器等模块进行了封装，使.NET 5能够更方便的应用到实际项目开发中。


### 框架组织结构

* Rye【核心组件】：框架的核心组件，包含框架各个组件的核心接口定义，部分核心功能如日志、AOP等功能模块的实现，以及开发中经常用到的辅助类和扩展方法。
* Rye.Web【AspNetCore组件】：提供AspNetCore的服务端功能的封装
* Rye.Jwt【JWT组件】：提供jwt生成、验证、过期、刷新等操作的的封装
* Rye.Authorization【身份认证、权限授权组件】：基于JWT和Authorization实现的权限认证授权组件
* Rye.Cache.Redis【Redis组件】：使用CSRedisCore封装了Redis的相关操作，并结合MemoryCache提供了二级缓存机制
* Rye.DataAccess【数据访问层组件】： 定义并实现数据访问层的核心接口，包括数据库连接、事物、读写分离、数据库连接池等功能。
* Rye.MySql【MySql数据访问层组件】：依赖Rye.DataAccess，提供针对的MySQL的Dapper操作的方法
* Rye.SqlServer【SqlServer数据访问层组件】：依赖Rye.DataAccess，提供针对的SqlServer的Dapper操作的方法
* Rye.EntityFrameworkCore【EF Core组件】：封装EntityFrameworkCore数据访问功能的实现，提供了支持多数据库、多上下文的功能
* Rye.EntityFrameworkCore.MySQL【EF Core MySQL组件】：封装针对的MySQL的EntityFrameworkCore数据访问功能的实现
* Rye.EntityFrameworkCore.SqlServer【EF Core SqlServer组件】：封装针对的SqlServer的EntityFrameworkCore数据访问功能的实现
* Rye.CodeGenerator【代码生成器】：使用Roslyn和Razor模板引擎实现的代码生成器
* Rye.EventBus.Redis【Redis事件总线】：基于Redis的事件总线功能
* Rye.EventBus.RabbitMQ【RabbitMQ事件总线】：基于RabbitMQ的事件总线功能
* Rye.Business【业务功能组件】：提供二维码、验证码、多语言等业务相关功能的封装实现

## Rye功能特性

### 1. 模块化的组件设计

参考ABP，设计了一个模块（Module）系统，所有实现了IStartupModule接口的类都被视为一个独立的模块，一个模块可以独立配置服务（ConfigueServices方法），并可在初始化时应用服务（Use方法）进行模块初始化。

### 2. 自定义的AOP实现

使用Emit技术，提供了CallingInterceptAttribute、CalledInterceptAttribute、ExceptionInterceptAttribute分别对应方法执行前、执行后、抛出异常时的切入方法，继承这几个特性类即可。基于AOP，提供了自动化的依赖注入、缓存等功能。

### 3. 读写分离的数据访问模式

无论是使用Dapper还是EF Core，都可以方便的进行读写分离。EF Core的上下文类通过UnitOfWork工作单元类获取，在分库的情况下，可以方便的获取数据上下文。

### 4. 多级缓存

结合MemoryCache和Redis实现了二级缓存，缓存更新、删除时，通过Redis订阅广播给所有客户端更新本地的内存缓存。数据获取的通道由从数据库直接读取改为内存->Redis->数据库，减少数据库压力

### 5. 事件总线

为减少业务间的耦合，事件总线是一种解决方案，Rye封装了多种事件总线，提供统一的接口。
* 本地事件总线，使用Disruptor.NET（Disruptor是一个无锁结构的高性能并发队列）实现
* Redis事件总线，使用BLPOP + LPUSH（非争抢）实现
* RabbitMQ事件总线，使用topic模式实现

## 快速启动

* 模块式风格
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services = services.UseDynamicProxyService(); // 使用支持动态代理功能的ServiceCollection
        services.AddWebModule() // 增加AspNetCore模块
                .AddRedisCacheModule(options =>  // 增加Redis模块
                    Configuration.GetSection("Framework:Redis").GetChildren().FirstOrDefault().Bind(options)) // 配置Redis
                .AddMySqlModule<MyDbConnectionProvider>()  // 增加MySql数据访问层模块
                .AddMySqlEFCoreModule(builder => // 增加EF Core模块
                {
                    builder.AddDbContext<DefaultDbContext>(DbConfig.DbRye.GetDescription()); // 配置默认数据上下文名称
                    builder.AddDbContext<DefaultDbContext>(DbConfig.DbRye_Read.GetDescription());
                })
                .AddAuthorizationModule<int>() // 增加身份认证模块
                .AddModule<DemoModule>() // 增加自定义模块
                .ConfigureModule(); // 配置模块注入
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
        app.UseModule(); // 模块功能初始化
    }
}
```

* 配置式风格
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services = services.UseDynamicProxyService(); // 使用支持动态代理功能的ServiceCollection
        services.AddRye()      // 配置Rye核心组件
                .AddWebRye()   // 配置AspNetCore组件
                .AddRedisCache(options =>  // 配置Redis组件
                    Configuration.GetSection("Framework:Redis").GetChildren().FirstOrDefault().Bind(options))
                .AddMySqlDbConnectionProvider<MyDbConnectionProvider>() // 配置MySql数据库连接提供者
                .AddDbBuillderOptions(builder =>  // 配置DbBuillderOptions
                {
                    builder.AddDbContext<DefaultDbContext>(DbConfig.DbRye.GetDescription());
                    builder.AddDbContext<DefaultDbContext>(DbConfig.DbRye_Read.GetDescription());
                })
                .AddMySqlEFCore()  // 配置MySql EF Core组件
                .AddJwt()          // 配置JWT组件
                .AddRyeAuthorization<int>(); // 配置身份认证组件
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
```