using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbConfig
{
    /// <summary>
    /// Database sessions
    /// </summary>
    public class db_config_sessions
    {
        /************** User Authentication **************/
        static public object GetUserAuthentication()
        {
            return HttpContext.Current.Session["user_authentication"];
        }

        static public void SetUserAuthentication(UserLoginData uld)
        {
            HttpContext.Current.Session["user_authentication"] = uld;
        }
    }
}