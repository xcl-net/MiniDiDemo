using System;

namespace MiniDIDemo
{
    /// <summary>
    /// 标记特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor |
                    AttributeTargets.Property |
                    AttributeTargets.Method,
                    AllowMultiple = false)]
    public class InjectionAttribute:Attribute{ }
}
