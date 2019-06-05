using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DoNet.Common.Reflection
{
    /// <summary>
    /// 快速反射执行方法
    /// </summary>
    public static class FastInvoke
    {
        //缓存缓托  
        static Dictionary<MethodInfo, FastInvokeHandler> invokeCache = new Dictionary<MethodInfo, FastInvokeHandler>();
        //同步锁  
        static object synLock = new object();

        /// <summary>  
        /// 方法调用委托  
        /// </summary>  
        /// <param name="target">对象</param>  
        /// <param name="paramters">参数</param>  
        /// <returns></returns>  
        public delegate object FastInvokeHandler(object target,
                                           object[] paramters);

        /// <summary>  
        /// 获取方法快速调用委托  
        /// </summary>  
        /// <param name="methodInfo">方法对象</param>  
        /// <returns></returns>  
        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            if (methodInfo == null) return null;

            if (!invokeCache.ContainsKey(methodInfo))
            {
                lock (synLock)
                {
                    if (!invokeCache.ContainsKey(methodInfo))
                    {
                        DynamicMethod dynamicMethod = new DynamicMethod(string.Empty,
                                     typeof(object), new Type[] { typeof(object),   
                     typeof(object[]) },
                                     methodInfo.DeclaringType.Module);
                        ILGenerator il = dynamicMethod.GetILGenerator();
                        ParameterInfo[] ps = methodInfo.GetParameters();
                        Type[] paramTypes = new Type[ps.Length];
                        for (int i = 0; i < paramTypes.Length; i++)
                        {
                            paramTypes[i] = ps[i].ParameterType;
                        }
                        LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];
                        for (int i = 0; i < paramTypes.Length; i++)
                        {
                            locals[i] = il.DeclareLocal(paramTypes[i]);
                        }
                        for (int i = 0; i < paramTypes.Length; i++)
                        {
                            il.Emit(OpCodes.Ldarg_1);
                            EmitFastInt(il, i);
                            il.Emit(OpCodes.Ldelem_Ref);
                            EmitCastToReference(il, paramTypes[i]);
                            il.Emit(OpCodes.Stloc, locals[i]);
                        }
                        il.Emit(OpCodes.Ldarg_0);
                        for (int i = 0; i < paramTypes.Length; i++)
                        {
                            il.Emit(OpCodes.Ldloc, locals[i]);
                        }
                        il.EmitCall(OpCodes.Call, methodInfo, null);
                        if (methodInfo.ReturnType == typeof(void))
                            il.Emit(OpCodes.Ldnull);
                        else
                            EmitBoxIfNeeded(il, methodInfo.ReturnType);
                        il.Emit(OpCodes.Ret);
                        FastInvokeHandler invoder =
                          (FastInvokeHandler)dynamicMethod.CreateDelegate(
                          typeof(FastInvokeHandler));

                        invokeCache.Add(methodInfo, invoder);

                        return invoder;
                    }
                }
            }

            return invokeCache[methodInfo];
        }

        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }  
    }

    /// <summary>
    /// lambada表达式快速反射
    /// </summary>
    public class DynamicMethodExecutor
    {
        private Func<object, object[], object> m_execute;

        public DynamicMethodExecutor(MethodInfo methodInfo)
        {
            this.m_execute = this.GetExecuteDelegate(methodInfo);
        }

        public object Execute(object instance, object[] parameters)
        {
            return this.m_execute(instance, parameters);
        }

        private Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            // parameters to execute
            ParameterExpression instanceParameter =
                Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter =
                Expression.Parameter(typeof(object[]), "parameters");

            // build parameter list
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }

            // non-instance for static method, or ((TInstance)instance)
            Expression instanceCast = methodInfo.IsStatic ? null :
                Expression.Convert(instanceParameter, methodInfo.ReflectedType);

            // static invoke or ((TInstance)instance).Method
            MethodCallExpression methodCall = Expression.Call(
                instanceCast, methodInfo, parameterExpressions);

            // ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            if (methodCall.Type == typeof(void))
            {
                Expression<Action<object, object[]>> lambda =
                    Expression.Lambda<Action<object, object[]>>(
                        methodCall, instanceParameter, parametersParameter);

                Action<object, object[]> execute = lambda.Compile();
                return (instance, parameters) =>
                {
                    execute(instance, parameters);
                    return null;
                };
            }
            else
            {
                UnaryExpression castMethodCall = Expression.Convert(
                    methodCall, typeof(object));
                Expression<Func<object, object[], object>> lambda =
                    Expression.Lambda<Func<object, object[], object>>(
                        castMethodCall, instanceParameter, parametersParameter);

                return lambda.Compile();
            }
        }
    }
}
