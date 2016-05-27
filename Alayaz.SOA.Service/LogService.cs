using JR.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SZ.Aisino.CMS.DbEntity;
using Alayaz.SOA.IService;
using UN = Microsoft.Practices.Unity;

namespace Alayaz.SOA.Service
{
    public class LogService : ILogService
    {
      

        
        public void LogInfo(string msg)
        {
            
            //this.Log.Log(msg);
            var logger = LogManager.GetLogger("");
            logger.Info(msg);
         }
 
    }
}
