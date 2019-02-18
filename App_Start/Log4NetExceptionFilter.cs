using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Requerimientos.App_Start
{
    public class Log4NetExceptionFilter : IExceptionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;

            Logger.Error(ex.Message);
        }
    }
}