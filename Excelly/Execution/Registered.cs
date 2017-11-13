using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Excelly.Execution
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Registered : Attribute
    {
        public static Dictionary<string, Func<object[], object>> RegisteredFunctions { get; private set; } = new Dictionary<string, Func<object[], object>>();
        public static Dictionary<string, object> RegisteredVariable { get; set; } = new Dictionary<string, object>();

        public string Name { get; private set; }

        public Registered(string name)
        {
            Name = name;
        }

        public static void RegisterAll(Type _class)
        {
            foreach (var method in _class.
                GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                RegisterMethod(method);
            }
        }

        public static void RegisterMethod(MethodInfo method)
        {
            var attr = method.GetCustomAttribute<Registered>();
            if (attr == null) return;
            if (RegisteredFunctions.ContainsKey(attr.Name)) throw new Exception("Key Duplicated");
            if (RegisteredVariable.ContainsKey(attr.Name)) throw new Exception("Key Duplicated");
            Func<object[], object> func = objs => method.Invoke(null, objs);
            RegisteredFunctions.Add(attr.Name, func);
            RegisteredVariable.Add(attr.Name, func);
        }
    }
}
