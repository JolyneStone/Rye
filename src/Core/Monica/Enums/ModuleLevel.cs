namespace Monica.Enums
{
    /// <summary>
    /// 模块级别，级别最小，优先级越大
    /// </summary>
    public enum ModuleLevel
    {
        /// <summary>
        /// 核心级别，表示系统的核心模块
        /// 现版本只有MonicaCoreModule、AspNetCoreModule等两个模块
        /// </summary>
        Core = 1,
        /// <summary>
        /// 框架级别，表示框架的功能性模块 
        /// </summary>
        FrameWork = 10,
        /// <summary>
        /// 应用级别，表示用户架构层的基础模块
        /// </summary>
        Application = 20,
        /// <summary>
        /// 业务级别，表示用户业务处理的模块
        /// </summary>
        Buiness = 30
    }
}