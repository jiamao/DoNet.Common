using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DoNet.Common.Reflection
{
    public static class TypeHelper
    {
        /// <summary>
        /// 生成属性
        /// </summary>
        /// <param name="typeBuilder">类型生成器</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyType">属性类型</param>
        private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, new Type[0]);

            // Define "get"
            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    propertyType, Type.EmptyTypes);

            // Get MethodInfo for base class (DataRow)
            MethodInfo getMethodInfo = typeof(BaseType).GetMethod(propertyType == typeof(decimal) ? "GetDecimalValue" : "GetValue", new Type[] { typeof(string) });

            // Generate content
            ILGenerator getILGenerator = getMethodBuilder.GetILGenerator();
            getILGenerator.Emit(OpCodes.Ldarg_0); // Load BindableObject on stack
            getILGenerator.Emit(OpCodes.Ldstr, propertyName); // Load property name
            getILGenerator.Emit(OpCodes.Call, getMethodInfo); // Call GetValue(string) method            
            getILGenerator.Emit(OpCodes.Ret); // Return

            // Define set
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new Type[] { propertyType });

            // Get MethodInfo for base class (DataRow)
            MethodInfo setMethodInfo = null;
            if (propertyType == typeof(decimal))
            {
                setMethodInfo = typeof(BaseType).GetMethod("SetDecimalValue", new Type[] { typeof(string), propertyType });
            }
            else
            {
                setMethodInfo = typeof(BaseType).GetMethod("SetValue", new Type[] { typeof(string), typeof(string) });
            }

            // Generate content
            ILGenerator setILGenerator = setMethodBuilder.GetILGenerator();
            setILGenerator.Emit(OpCodes.Ldarg_0); // Load BindableObject on stack
            setILGenerator.Emit(OpCodes.Ldstr, propertyName); // Load property name
            setILGenerator.Emit(OpCodes.Ldarg_1); // Load value on stack
            setILGenerator.Emit(OpCodes.Call, setMethodInfo); // Call set_Item(string, string) method
            setILGenerator.Emit(OpCodes.Ret);

            // Assign "get" and "set" to "property"
            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

        /// <summary>
        /// Creates DTO type (class)
        /// </summary>
        /// <param name="dataRow">DataRow as a data source</param>
        /// <param name="addSupportProperties">Tru for generating properties with suffixes Required, Error, and HasError</param>
        /// <returns>Newly created type</returns>
        public static Type CreateType(string dtoTypeFullName, IDictionary<string, Type> propertys)
        {
            // Get type builder from newly created assembly
            // Define dynamic assembly
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = dtoTypeFullName + ".dll";
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            // Define type
            TypeBuilder typeBuilder = moduleBuilder.DefineType(dtoTypeFullName
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(BaseType));

            //// Get ConstructorInfo for base class (BindableObject)
            //ConstructorInfo constructorInfo = typeof(OAPBindableObject).GetConstructor(new Type[] { typeof(DataRow) });

            //// Define DTO constructor

            //Type[] constructorArguments = new Type[] { typeof(string) };
            //ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorArguments);

            // Generate DTO constructor
            //ILGenerator getILGenerator = constructorBuilder.GetILGenerator();
            //getILGenerator.Emit(OpCodes.Ldarg_0); // Load BindableObject on stack
            //getILGenerator.Emit(OpCodes.Ldarg_1); // Load DataRow on stack
            ////getILGenerator.Emit(OpCodes.Call, constructorInfo); // Call BindableObject constructor
            //getILGenerator.Emit(OpCodes.Ret);

            // Generate DTO constructor
            var defaultConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
            ConstructorInfo constructorInfo = typeof(BaseType).GetConstructor(new Type[0]);
            var defaultgenerator = defaultConstructor.GetILGenerator();
            defaultgenerator.Emit(OpCodes.Ldarg_0); // Load BindableObject on stack
            //defaultgenerator.Emit(OpCodes.Ldarg_1); // Load DataRow on stack    
            defaultgenerator.Emit(OpCodes.Call, constructorInfo); // Call BindableObject constructor
            defaultgenerator.Emit(OpCodes.Ret);

            foreach (var p in propertys)
            {
                CreateProperty(typeBuilder, p.Key, p.Value);
            }

            return typeBuilder.CreateType();
        }
    }

    /// <summary>
    /// Base class for generated DTO
    /// </summary>
    public class BaseType
    {
        Dictionary<string, string> _stringValues = new Dictionary<string, string>();

        /// <summary>
        /// 列对应的类型
        /// </summary>
        Dictionary<string, Type> _columnType = new Dictionary<string, Type>();


        public BaseType()
        {

        }

        #region Methods

        /// <summary>
        /// 设置列类型
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="t"></param>
        public void SetColumnType(string columnName, Type t)
        {
            _columnType[columnName] = t;
        }

        /// <summary>
        /// Gets value from specified field
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Field value</returns>
        public string GetValue(string key)
        {
            return _stringValues[key];
        }

        /// <summary>
        /// 返回数字类型
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public decimal GetDecimalValue(string columnName)
        {
            var v = this._stringValues[columnName];
            decimal value = 0;
            decimal.TryParse(v, out value);
            return value;
        }

        /// <summary>
        /// 返回数字类型
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public DateTime GetDateTimeValue(string columnName)
        {
            var v = this._stringValues[columnName];
            DateTime value;
            DateTime.TryParse(v, out value);
            return value;
        }


        /// <summary>
        /// Sets value into specified field
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="value">Field value</param>
        public void SetValue(string columnName, string value)
        {
            this._stringValues[columnName] = value;
        }

        public void SetDecimalValue(string columnName, decimal value)
        {
            this._stringValues[columnName] = value.ToString();
        }

        public void SetDateTimeValue(string columnName, DateTime value)
        {
            this._stringValues[columnName] = value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion
    }
}
