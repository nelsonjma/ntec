
using System.Collections.Generic;

using DbConfig;

namespace Views.Page
{
    /// <summary>
    /// Check if user has authorization to access the page
    /// </summary>
    public class PageAuthentication
    {
        private readonly int _pageId;

        private List<int> _userPages;

        public PageAuthentication(int pageId)
        {
            _pageId = pageId;
            _userPages = new List<int>();
        }

        /// <summary>
        /// Check if page is from users
        /// </summary>
        /// <returns></returns>
        private bool IsPagePrivate()
        {
            db_config_users users = new db_config_users();
            users.Open();

            return users.IsPageFromUsers(_pageId);
        }

        /// <summary>
        /// check if user is loged on, and if he is then get user pages
        /// </summary>
        /// <returns></returns>
        private bool IsUserAuthenticated()
        {
            try
            {
                object uld = db_config_sessions.GetUserAuthentication();

                if (uld != null)
                {
                    _userPages = ((UserLoginData) uld).UserPages;

                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// check if page is from user
        /// </summary>
        /// <returns></returns>
        private bool IsUserPage()
        {
            return _userPages.IndexOf(_pageId) != -1;
        }

        public bool IsPageVisible()
        {
            if (!IsPagePrivate()) return true;

            return IsUserAuthenticated() && IsUserPage();
        }
    }
}