using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SomeObjectTest.SampleTypes
{
    public class SamplePocoClass
    {
        public GenericClass<SamplePocoClass> GenericClassProp { get; set; }

        public Hashtable HashTableProp { get; set; }

        public Array AbstractProp { get; set; }

        public IList InterfaceProp { get; set; }

        public ArrayList ArrayListProp { get; set; }

        public SampleStruct StructProp { get; set; }

        public InnerPoco InnerPocoProp { get; set; }

        public List<SampleStruct> InnerPocoListProp { get; private set; }

        public SamplePocoClass[] ArrayProp { get; set; }

        public Dictionary<SamplePocoClass, SamplePocoClass> DictionaryProp { get; set; }

        public SamplePocoClass RecursiveProp { get; set; }

        // Nullable types

        public int? NUllableIntProp { get; set; }

        public SampleStruct? NUllableStructProp { get; set; }

        public SampleEnum? NUllableEnumProp { get; set; }

        // Primitive types

        public bool BoolProp { get; set; }

        public char CharProp { get; set; }

        public sbyte SByteProp { get; set; }

        public byte ByteProp { get; set; }

        public short ShortProp { get; set; }

        public ushort UShortProp { get; set; }

        public int IntProp { get; set; }

        public uint UintProp { get; set; }

        public long LongProp { get; set; }

        public ulong ULongProp { get; set; }

        public float FloatProp { get; set; }

        public double DoubleProp { get; set; }

        public Decimal DecimalProp { get; set; }

        public DateTime DateTimeProp { get; set; }

        public string StringProp { get; set; }

        public SampleEnum EnumProp { get; set; }

        // Unsupported types - parameterless-constructor required

        public Tuple<int, object> TupleProp { get; set; }
    }
}
