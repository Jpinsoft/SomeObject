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
            SOBuilder objBuilder = new SOBuilder();

            string[] stringArrayRes = objBuilder.Generate<string[]>(maxRecursionLevel);
            SamplePocoClass pocoRes = objBuilder.Generate<SamplePocoClass>(maxRecursionLevel);
            SampleStruct structRes = objBuilder.Generate<SampleStruct>(maxRecursionLevel);
            int intRes = objBuilder.Generate<int>(maxRecursionLevel);
            SamplePocoClass[] pocoArray = objBuilder.Generate<SamplePocoClass[]>(maxRecursionLevel);

            Dictionary<string, List<List<SamplePocoClass>>> dictRes =
                objBuilder.Generate<Dictionary<string, List<List<SamplePocoClass>>>>(maxRecursionLevel);

            Trace.WriteLine("-------------- GenericGenerateTest Results ----------------------");
            Trace.WriteLine(string.Format("Total {0}", objBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors {0}", objBuilder.Errors.Count));
        }     
```

```csharp
        [TestMethod]
        public void NonGenericGenerateTest()
        {
            SOBuilder objBuilder = new SOBuilder();

            object stringArrayRes = objBuilder.Generate(typeof(string[]), maxRecursionLevel);
            object pocoRes = objBuilder.Generate(typeof(SamplePocoClass), maxRecursionLevel);
            object structRes = objBuilder.Generate(typeof(SampleStruct), maxRecursionLevel);


            Trace.WriteLine("-------------- NonGenericGenerateTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", objBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", objBuilder.Errors.Count));
        }
```        

```csharp
        [TestMethod]
        public void GeneratePositiveNumbersTest()
        {
            SOBuilder objBuilder = new SOBuilder(new RandomValueGenerator(0, 100, 1, 20, false));

            sbyte[] sbyteValues = objBuilder.Generate<sbyte[]>(maxRecursionLevel);
            short[] shortValues = objBuilder.Generate<short[]>(maxRecursionLevel);
            int[] intResult = objBuilder.Generate<int[]>(maxRecursionLevel);
            float[] floatValues = objBuilder.Generate<float[]>(maxRecursionLevel);
            double[] doubleValues = objBuilder.Generate<double[]>(maxRecursionLevel);
            decimal[] decimalValues = objBuilder.Generate<Decimal[]>(maxRecursionLevel);

            Trace.WriteLine("-------------- GeneratePositiveNumbersTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", objBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", objBuilder.Errors.Count));
        }
```        
```csharp
        [TestMethod]
        public void GenerateLargeCollectionTest()
        {
            SOBuilder objBuilder = new SOBuilder(new RandomValueGenerator(-1000, 1000, 100000, 100000, false));

            List<SampleStruct> structRes = objBuilder.Generate<List<SampleStruct>>(maxRecursionLevel);

            Trace.WriteLine("-------------- GenerateLargeCollectionTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", objBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", objBuilder.Errors.Count));
        }      
 ```
