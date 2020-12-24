using System;
using System.Collections.Generic;
using System.Text;

namespace SomeObjectTest.SampleTypes
{
    public class GenericClass<T>
    {
        public GenericClass()
        {

        }

        public T SomeProp { get; set; }
    }
}
