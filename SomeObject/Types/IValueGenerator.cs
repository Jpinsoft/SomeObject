using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpinsoft.SomeObject.Types
{
    public interface IValueGenerator
    {
        int GetColletionLength();

        bool GenerateBoolean();

        char GenerateChar();

        sbyte GenerateSByte();

        byte GenerateByte();

        short GenerateInt16();

        ushort GenerateUInt16();

        int GenerateInt32();

        uint GenerateUInt32();

        long GenerateInt64();

        ulong GenerateUInt64();

        float GenerateSingle();

        double GenerateDouble();

        decimal GenerateDecimal();

        DateTime GenerateDateTime();

        /// <summary>
        /// Must be unique becouse strings may be used in Dictionaty keys
        /// </summary>
        /// <returns></returns>
        string GenerateString();
    }
}
