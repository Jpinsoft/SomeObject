using System;
using System.Collections.Generic;
using System.Text;

namespace SomeObjectTest.SampleTypes
{
    public struct SampleStruct
    {
        public int IntProp { get; set; }

        public bool BoolProp { get; set; }

        public InnerPoco ClassProp { get; set; }

        public string StringProp { get; set; }

        public short? NullableShortProp { get; set; }

    }
}
