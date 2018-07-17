using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniDIDemo
{
    /// <summary>
    /// 精简版di容易
    /// </summary>
    public class SampleDI
    {
        private ConcurrentDictionary<Type, Type> typeMapping = new ConcurrentDictionary<Type, Type>();

        public void Register(Type from, Type to)
        {
            typeMapping[from] = to;
        }

        public object GetService(Type serviceType)
        {
            Type type;
            if (!typeMapping.TryGetValue(serviceType, out type))
            {
                type = serviceType;
            }

            if (type.IsInterface || type.IsAbstract)
            {
                return null;
            }

            ConstructorInfo constructor = this.GetConstructor(type);
            if (constructor == null)
                return null;
            //获取构造函数的所有参数的构造函数
            object[] arguments = constructor.GetParameters().Select(p => this.GetService(p.ParameterType)).ToArray();
            object service = constructor.Invoke(arguments);

            this.InitializeInjectProperties(service);
            this.InvokeInjectedMethods(service);
            return service;
        }

        protected virtual ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            return constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectionAttribute>() != null) ?? constructors.FirstOrDefault();
        }


        protected virtual void InitializeInjectProperties(object service)
        {
            PropertyInfo[] properties = service.GetType().GetProperties().
                Where(p => p.CanWrite && p.GetCustomAttribute<InjectionAttribute>() != null).ToArray();
            Array.ForEach(properties, p => p.SetValue(service, this.GetService(p.PropertyType)));
        }


        protected virtual void InvokeInjectedMethods(object service)
        {
            MethodInfo[] methods = service.GetType().GetMethods().
                Where(m => m.GetCustomAttribute<InjectionAttribute>() != null).ToArray();
            Array.ForEach(methods, m =>
            {
                object[] arguments = m.GetParameters().Select(p => this.GetService(p.ParameterType)).ToArray();
                m.Invoke(service, arguments);
            }
            );
        }

        public void Register<T1, T2>()
        {
            Register(typeof(T1), typeof(T2));
        }

        public T GetService<T>() where T : class
        {
            return this.GetService(typeof(T)) as T;
        }

    }
}
