using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Todo.Mobile.Infrastructure.Common;
using System.Reflection;
using System.Dynamic;
using System.Collections.Concurrent;
using Xamarin.Forms;

[assembly: Dependency(typeof(Todo.Mobile.Droid.ReflectionFactory))]
namespace Todo.Mobile.Droid
{
    public class ReflectionFactory : IPrivateReflectionFactory
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        public object CallMethod(object target, string methodName, params object[] parms)
        {
            // get the type of the target object
            var type = target.GetType();
            // build up arguments type array
            var argtypes = new Type[parms.Length];
            for (var i = 0; i < parms.Length; i++)
                argtypes[i] = parms[i].GetType();

            // create tuple for lookup
            var tuple = new Tuple<Type, string, Type[]>(type, methodName, argtypes);

            // iterate from top down to check for member types
            while (true)
            {
                // find the member
                var member = FindMember(type, tuple);
                // if its found then invoke it
                if (member != null) return member.Invoke(target, parms);
                // if the base type is null then stop
                if (type.BaseType == null) return null;
                // otherwise set the base type to check 
                // and continue loop until BaseType == null
                type = type.BaseType;
            }
        }

        readonly static ConcurrentDictionary<Tuple<Type, string, Type[]>, MethodInfo> methodCache
            = new ConcurrentDictionary<Tuple<Type, string, Type[]>, MethodInfo>();

        private MethodInfo FindMember(Type inputType, Tuple<Type, string, Type[]> lookup)
        {
            MethodInfo info;

            // try to find the method in cache
            if (!methodCache.TryGetValue(lookup, out info))
            {
                // get the input types methods
                var methods = inputType.GetMethods();
                info = null;
                // try to look it up
                foreach (var method in methods)
                {
                    if (method.Name != lookup.Item2)
                        continue;
                    var methodParamsArray = method.GetParameters().Select(a => a.ParameterType).ToArray();
                    if (!methodParamsArray.SequenceEqual(lookup.Item3)) continue;

                    info = method;
                    break;

                }

                // if we found it
                if (info != null)
                {
                    // add it to the cache
                    methodCache.TryAdd(lookup, info);
                }
            }

            // found it so return it
            return info;
        }

        public dynamic AsDynamic(object o)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }

        internal class PrivateReflectionDynamicObject : DynamicObject
        {
            public object RealObject { get; set; }
            private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            internal static object WrapObjectIfNeeded(object o)
            {
                // Don't wrap primitive types, which don't have many interesting internal APIs
                if (o == null || o.GetType().IsPrimitive || o is string)
                    return o;

                return new PrivateReflectionDynamicObject { RealObject = o };
            }

            // Called when a method is called
            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                result = InvokeMemberOnType(RealObject.GetType(), RealObject, binder.Name, args);

                // Wrap the sub object if necessary. This allows nested anonymous objects to work.
                result = WrapObjectIfNeeded(result);

                return true;
            }

            private static object InvokeMemberOnType(Type type, object target, string name, object[] args)
            {
                var argtypes = new Type[args.Length];
                for (var i = 0; i < args.Length; i++)
                    argtypes[i] = args[i].GetType();
                while (true)
                {
                    var member = type.GetMethod(name, bindingFlags, null, argtypes, null);
                    if (member != null) return member.Invoke(target, args);
                    if (type.BaseType == null) return null;
                    type = type.BaseType;
                }
            }
        }
    }
}