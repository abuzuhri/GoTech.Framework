using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework
{
    public class Configuration
    {
        public static bool IsBusinessRuleValidationEnabled
        {
            get
            {
                return ConfigurationSettings.AppSettings["GoTech.Framework.BusinessRuleValidation.Enable"]=="true";
            }

        }
        
        public static string BusinessRuleValidationAssemblyname
        {
            get
            {
                return ConfigurationSettings.AppSettings["GoTech.Framework.BusinessRuleValidation.Assemblyname"];
            }

        }

        public static string BusinessRuleValidationNamespace
        {
            get
            {
                return ConfigurationSettings.AppSettings["GoTech.Framework.BusinessRuleValidation.Namespace"] ;
            }
        }
    }
}
