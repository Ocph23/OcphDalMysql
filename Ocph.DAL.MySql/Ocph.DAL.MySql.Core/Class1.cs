using System;

namespace Ocph.DAL.MySql.Core
{
    [TableName("Ocph")]
    public class Class1
    {
        private int myVar;
        [DbColumn("test")]
        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

    }
}
