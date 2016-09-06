using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelly
{
    public class Variable
    {
        public static FMain MainForm;
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string Argument { get; set; }
        public string[] Args { get { return FMain.GetArgValue(Argument, MainForm); } }

        public Variable(string name, int len, string type, string arg)
        {
            Name = name;
            Length = len;
            Type = type;
            Argument = arg;
        }

        public string[] CalcVar()
        {
            string[] result = new string[Length];
            for(int i = 0; i < Length; i++)
            {
                result[i] = Calc();
            }

            return result;
        }

        private string Calc()
        {
            return FMain.CalcFunc(Type, Args);
        }
    }
}
