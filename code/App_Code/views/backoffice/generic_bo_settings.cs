using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DbConfig;

namespace Views.BackOffice
{
    /// <summary>
    /// Summary description for generic_bo_settings
    /// </summary>
    public class GenericBackOfficeSettings
    {
        /// <summary>
        /// Check if the guy is admin
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfUserAdmin()
        {
            try
            { 
                object uld = db_config_sessions.GetUserAuthentication();

                if (uld != null)
                {
                    return ((UserLoginData)uld).User.AdMIn == 1 ? true : false;
                }
            }
            catch { }
            
            return false;
        }

        /// <summary>
        /// Simplified user check and redirect
        /// </summary>
        public static void RedirectUsersToMainPage()
        {
            if (!CheckIfUserAdmin())
                Generic.PageRedirect("~/frontoffice.aspx");
        }

    }
}