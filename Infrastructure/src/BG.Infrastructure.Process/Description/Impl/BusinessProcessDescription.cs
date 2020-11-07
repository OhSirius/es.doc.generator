using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Description
{
    public class BusinessProcessDescription 
    {
        public const string aCreateDocument = "CreateDocument";
        public const string aInitialState = "InitialState";
        public const string aCreateComponent = "CreateComponent";
        public const string aSaveComponent = "SaveComponent";
        public const string aDeleteOperation = "DeleteOperation";

        public static string ObjectType
        {
            get
            {
                return "BusinessProcess";
            }
        }
    }
}
