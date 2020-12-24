# SomeObject
SomeObject is a simple reflection-based tool designed for generating and populating objects with random values 

## Features/Purpose

 - performance tuning with large amounts of data
 - generating test data for use when developing or testing an application
 - built-in RandomValueGenerator allows you to set the range of values and collections
 - allows to set recursion level for complex types
 - Extensibility: allows creation of custom implementation of IValueGenerator for generating primitive values and strings
 
## Supported types:

 - classes and generic classes with parameterless constructor, structs
 - assigning values to private and public properties(fields are not supported) to reference types and structs 
 - collection types that implements IDictionary, IList, ArrayList, Array[] 
 - primitive types, enums, strings, nullable types
 
## Using SomeObject

```csharp
        [TestMethod]
        public void GenericGenerateTest()
        {
            SOBuilder mBuilder = new SOBuilder();

            string[] stringArrayRes = mBuilder.Generate<string[]>(maxRecursionLevel);
            SamplePocoClass pocoRes = mBuilder.Generate<SamplePocoClass>(maxRecursionLevel);
            SampleStruct structRes = mBuilder.Generate<SampleStruct>(maxRecursionLevel);
            int intRes = mBuilder.Generate<int>(maxRecursionLevel);
            SamplePocoClass[] pocoArray = mBuilder.Generate<SamplePocoClass[]>(maxRecursionLevel);

            Dictionary<string, List<List<SamplePocoClass>>> dictRes =
                mBuilder.Generate<Dictionary<string, List<List<SamplePocoClass>>>>(maxRecursionLevel);

            Trace.WriteLine("-------------- GenericGenerateTest Results ----------------------");
            Trace.WriteLine(string.Format("Total {0}", mBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors {0}", mBuilder.Errors.Count));
        }     
```

```csharp
        [TestMethod]
        public void NonGenericGenerateTest()
        {
            SOBuilder mBuilder = new SOBuilder();

            object stringArrayRes = mBuilder.Generate(typeof(string[]), maxRecursionLevel);
            object pocoRes = mBuilder.Generate(typeof(SamplePocoClass), maxRecursionLevel);
            object structRes = mBuilder.Generate(typeof(SampleStruct), maxRecursionLevel);


            Trace.WriteLine("-------------- NonGenericGenerateTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", mBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", mBuilder.Errors.Count));
        }
```        

```csharp
        [TestMethod]
        public void GeneratePositiveNumbersTest()
        {
            SOBuilder mBuilder = new SOBuilder(new RandomValueGenerator(0, 100, 1, 20, false));

            sbyte[] sbyteValues = mBuilder.Generate<sbyte[]>(maxRecursionLevel);
            short[] shortValues = mBuilder.Generate<short[]>(maxRecursionLevel);
            int[] intResult = mBuilder.Generate<int[]>(maxRecursionLevel);
            float[] floatValues = mBuilder.Generate<float[]>(maxRecursionLevel);
            double[] doubleValues = mBuilder.Generate<double[]>(maxRecursionLevel);
            decimal[] decimalValues = mBuilder.Generate<Decimal[]>(maxRecursionLevel);

            Trace.WriteLine("-------------- GeneratePositiveNumbersTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", mBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", mBuilder.Errors.Count));
        }
```        
```csharp
        [TestMethod]
        public void GenerateLargeCollectionTest()
        {
            SOBuilder mBuilder = new SOBuilder(new RandomValueGenerator(-1000, 1000, 100000, 100000, false));

            List<SampleStruct> structRes = mBuilder.Generate<List<SampleStruct>>(maxRecursionLevel);

            Trace.WriteLine("-------------- GenerateLargeCollectionTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", mBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", mBuilder.Errors.Count));
        }      
 ```
