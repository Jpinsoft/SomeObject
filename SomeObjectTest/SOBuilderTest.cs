using Jpinsoft.SomeObject;
using Jpinsoft.SomeObject.ValueGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomeObjectTest.SampleTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstanceTest
{
    [TestClass]
    public class SOBuilderTest
    {
        private int maxRecursionLevel = 4;

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


        [TestMethod]
        public void GenerateUpperCaseTest()
        {
            SOBuilder mBuilder = new SOBuilder(new RandomValueGenerator(-1000, 1000, 1, 20, true));

            string[] stringArrayRes = mBuilder.Generate<string[]>(maxRecursionLevel);

            Trace.WriteLine("-------------- GenerateUpperCaseTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", mBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", mBuilder.Errors.Count));
        }

        [TestMethod]
        public void GenerateLargeCollectionTest()
        {
            SOBuilder mBuilder = new SOBuilder(new RandomValueGenerator(-1000, 1000, 100000, 100000, false));

            List<SampleStruct> structRes = mBuilder.Generate<List<SampleStruct>>(maxRecursionLevel);

            Trace.WriteLine("-------------- GenerateLargeCollectionTest Results ------------------------");
            Trace.WriteLine(string.Format("TotalCountOfGeneratedObjects {0}", mBuilder.TotalCountOfGeneratedObjects));
            Trace.WriteLine(string.Format("Generating errors            {0}", mBuilder.Errors.Count));
        }

    }
}
