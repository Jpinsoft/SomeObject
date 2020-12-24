
/********************************************************************************************************************************* 

SomeObject is a simple reflection-based tool designed for generating and populating objects with random 
values (classes and generic classes with parameterless constructor, value types, strings, collections ...)

Features/Purpose

 - performance tuning with large amounts of data
 - creating test data for use when developing or testing an application
 - built-in RandomValueGenerator whom you can set up generated values and collections ranges
 - allows to set recursion level for complex types
 - Extensibility: allows creation of custom implementation of IValueGenerator for generating primitive values and strings

Supported types:

 - classes and generic classes with parameterless constructor, structs
 - for reference types and structs assigning values to private and public properties(fields are not supported) 
 - collection types that implements IDictionary, IList, ArrayList, Array[] 
 - primitive types, enums, strings, nullable types


  SomeObject is released under the GPL
  Copyright (c) 2017, Jozef Bátora
  http://jpinsoft.net

*********************************************************************************************************************************/

using Jpinsoft.SomeObject.Types;
using Jpinsoft.SomeObject.ValueGenerators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jpinsoft.SomeObject
{
    public class SOBuilder
    {
        Random rnd = new Random();

        public int MaxRecuriosnLevel { get; private set; }

        IValueGenerator valueGenerator;

        /// <summary>
        /// All suppressed errors during the generating process
        /// </summary>
        public List<GeneratorErrorInfo> Errors { get; set; }

        /// <summary>
        /// Primitive/Nullable values and string values (int, datetime, string...)
        /// </summary>
        public int CountOfGeneratedValues { get; set; }

        /// <summary>
        /// ICollection types (List, Dictionary, Array...)
        /// </summary>
        public int CountOfGeneratedCollections { get; set; }

        /// <summary>
        /// Classes or structs
        /// </summary>
        public int CountOfGeneratedObjects { get; set; }

        /// <summary>
        /// CountOfGeneratedValues + CountOfGeneratedCollections + CountOfGeneratedObjects
        /// </summary>
        public int TotalCountOfGeneratedObjects { get { return CountOfGeneratedValues + CountOfGeneratedCollections + CountOfGeneratedObjects; } }

        public SOBuilder()
        {
            Init(new RandomValueGenerator());
        }

        public SOBuilder(IValueGenerator valueGenerator)
        {
            Init(valueGenerator);
        }

        private void Init(IValueGenerator valueGenerator)
        {
            this.valueGenerator = valueGenerator;
            this.Errors = new List<GeneratorErrorInfo>();
        }

        public object Generate(Type type, int maxRecurionLevel)
        {
            this.MaxRecuriosnLevel = maxRecurionLevel;

            object instance = GenerateObjectInstanceRecursive(type, 0);

            if (instance == null)
                Errors.Add(new GeneratorErrorInfo(string.Format("Type {0} is not supported", type), 0, null));

            return instance;
        }

        public T Generate<T>(int maxRecurionLevel)
        {
            var res = this.Generate(typeof(T), maxRecurionLevel);

            return res != null ? (T)res : default(T);
        }

        private object GenerateObjectInstanceRecursive(Type type, int currentRecursionLevel)
        {
            if (type.IsAbstract || type.IsInterface)
                return null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            object primitiveVlue = GeneratePrimitiveValueOrString(type);

            if (primitiveVlue != null)
            {
                CountOfGeneratedValues++;
                return primitiveVlue;
            }

            if (currentRecursionLevel < MaxRecuriosnLevel)
            {
                try
                {
                    ICollection colletionValue = GenerateCollection(currentRecursionLevel, type);

                    if (colletionValue != null)
                    {
                        CountOfGeneratedCollections++;
                        return colletionValue;
                    }
                }
                catch (Exception ex)
                {
                    Errors.Add(new GeneratorErrorInfo(string.Format("GenerateCollection Error: Unable to generate collection of type '{0}'", type), currentRecursionLevel, ex));
                    return null;
                }

                // Single Object
                if (type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null)
                {
                    object singleObjectInstance = Activator.CreateInstance(type);

                    PropertyInfo[] props = type.GetProperties();

                    foreach (PropertyInfo propInfo in props)
                    {
                        if (propInfo.CanWrite)
                        {
                            try
                            {
                                object value = GenerateObjectInstanceRecursive(propInfo.PropertyType, currentRecursionLevel + 1);

                                if (value != null)
                                {
                                    CountOfGeneratedObjects++;
                                    propInfo.SetValue(singleObjectInstance, value);
                                }

                            }
                            catch (Exception ex)
                            {
                                Errors.Add(new GeneratorErrorInfo(string.Format("GenerateSingleObject Error: Unable to generate and set property '{0}', Property type '{1}', Declaring type '{2}'.", propInfo.Name, propInfo.PropertyType, propInfo.DeclaringType), currentRecursionLevel, ex));
                                continue;
                            }
                        }
                    }

                    return singleObjectInstance;
                }
            }

            return null;
        }

        public ICollection GenerateCollection(int currentRecursionLevel, Type type)
        {
            int rank = valueGenerator.GetColletionLength();

            IDictionary dictionatyValue = GenerateDictionary(currentRecursionLevel, type, rank);

            if (dictionatyValue != null)
                return dictionatyValue;

            IList arrayValue = GenerateArray(currentRecursionLevel, type, rank);

            if (arrayValue != null)
                return arrayValue;

            IList listValue = GenerateList(currentRecursionLevel, type, rank);

            if (listValue != null)
                return listValue;

            return null;
        }

        private Array GenerateArray(int currentRecursionLevel, Type type, int rank)
        {
            if (type.IsArray)
            {
                Type genericArgument = GetEnumerableType(type);

                Array array = Array.CreateInstance(genericArgument, rank);

                for (int i = 0; i < rank; i++)
                {
                    object innerListObject = GenerateObjectInstanceRecursive(genericArgument, currentRecursionLevel + 1);

                    if (innerListObject != null)
                        array.SetValue(innerListObject, i);
                }

                return array;
            }

            return null;
        }

        private IList GenerateList(int currentRecursionLevel, Type type, int rank)
        {
            IList list = null;

            // ArrayList
            if (!type.IsGenericType && typeof(IList).IsAssignableFrom(type))
            {
                list = (IList)Activator.CreateInstance(type);

                for (int i = 0; i < rank; i++)
                {
                    object innerListObject = GenerateObjectInstanceRecursive(typeof(string), currentRecursionLevel + 1);

                    if (innerListObject != null)
                        list.Add(innerListObject);
                }

                return list;
            }

            // Generic implementation IList
            if (type.IsGenericType && typeof(IList).IsAssignableFrom(type))
            {
                Type genericArgument = GetEnumerableType(type);
                list = (IList)Activator.CreateInstance(type);

                for (int i = 0; i < rank; i++)
                {
                    object innerListObject = GenerateObjectInstanceRecursive(genericArgument, currentRecursionLevel + 1);

                    if (innerListObject != null)
                        list.Add(innerListObject);
                }

                return list;
            }

            return null;
        }

        private IDictionary GenerateDictionary(int currentRecursionLevel, Type type, int rank)
        {
            if (!type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type))
            {
                IDictionary dictionayValue = (IDictionary)Activator.CreateInstance(type);

                for (int i = 0; i < rank; i++)
                {
                    object keyInstance = GenerateObjectInstanceRecursive(typeof(string), currentRecursionLevel + 1);
                    object valueInstance = GenerateObjectInstanceRecursive(typeof(string), currentRecursionLevel + 1);

                    if (keyInstance != null && valueInstance != null)
                        dictionayValue.Add(keyInstance, valueInstance);
                }

                return dictionayValue;
            }

            if (type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type) && type.GetGenericArguments().Length == 2)
            {
                Type keyType = type.GetGenericArguments()[0];
                Type valueType = type.GetGenericArguments()[1];

                IDictionary dictionayValue = (IDictionary)Activator.CreateInstance(type);

                for (int i = 0; i < rank; i++)
                {
                    object keyInstance = GenerateObjectInstanceRecursive(keyType, currentRecursionLevel + 1);
                    object valueInstance = GenerateObjectInstanceRecursive(valueType, currentRecursionLevel + 1);

                    if (keyInstance != null && valueInstance != null)
                        dictionayValue.Add(keyInstance, valueInstance);
                }

                return dictionayValue;
            }

            return null;
        }

        private Type GetEnumerableType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        private object GeneratePrimitiveValueOrString(Type type)
        {
            if (type.IsEnum)
            {
                Array enumValues = Enum.GetValues(type);
                return enumValues.GetValue(rnd.Next(0, enumValues.Length));
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DBNull:
                    return DBNull.Value;

                case TypeCode.Boolean:
                    return valueGenerator.GenerateBoolean();

                case TypeCode.Char:
                    return valueGenerator.GenerateChar();

                case TypeCode.SByte:
                    return valueGenerator.GenerateSByte();

                case TypeCode.Byte:
                    return valueGenerator.GenerateByte();

                case TypeCode.Int16:
                    return valueGenerator.GenerateInt16();

                case TypeCode.UInt16:
                    return valueGenerator.GenerateUInt16();

                case TypeCode.Int32:
                    return valueGenerator.GenerateInt32();

                case TypeCode.UInt32:
                    return valueGenerator.GenerateUInt32();

                case TypeCode.Int64:
                    return valueGenerator.GenerateInt64();

                case TypeCode.UInt64:
                    return valueGenerator.GenerateUInt64();

                case TypeCode.Single:
                    return valueGenerator.GenerateSingle();

                case TypeCode.Double:
                    return valueGenerator.GenerateDouble();

                case TypeCode.Decimal:
                    return valueGenerator.GenerateDecimal();

                case TypeCode.DateTime:
                    return valueGenerator.GenerateDateTime();

                case TypeCode.String:
                    return valueGenerator.GenerateString();
            }

            return null;
        }
    }
}
