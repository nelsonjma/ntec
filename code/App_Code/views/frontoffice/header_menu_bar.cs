using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DbConfig;

namespace Views.Frontoffice
{
    /// <summary>
    /// Frontoffice Menu Bar
    /// </summary>
    public class MenuBar
    {
        List<MenuBarItem> _menuBarItems;

        public MenuBar()
        {
            _menuBarItems = new List<MenuBarItem>();
        }

        public int GetHeaderPosition(string text)
        {
            try
            {
                return _menuBarItems.FindIndex(x => x.Text == text);
            }
            catch { }

            return -1;
        }

        public int GetMenuPosition(int headerPosition, string text)
        {
            try
            {
                return _menuBarItems[headerPosition].SubItems.FindIndex(x => x.Text == text);
            }
            catch { }

            return -1;
        }

        /* HEADER */
        public void AddHeader(string text, string url)
        {
            _menuBarItems.Add(new MenuBarItem()
            {
                Text = text,
                Url = url,
                SubItems = new List<MenuBarItem>()
            });
        }

        /* MENU */
        public void AddMenuItem(string header, string text, string url)
        {
            _menuBarItems.Single(x => x.Text == header).SubItems.Add(new MenuBarItem()
            {
                Text = text,
                Url = url,
                SubItems = new List<MenuBarItem>()
            });
        }

        public void AddMenuItem(int headerPosition, string text, string url)
        {
            _menuBarItems[headerPosition].SubItems.Add(new MenuBarItem()
            {
                Text = text,
                Url = url,
                SubItems = new List<MenuBarItem>()
            });
        }

        /* SUB MENU */
        public void AddSubMenuItem(string header, string menuHeader, string text, string url)
        {
            _menuBarItems.Single(x => x.Text == header).
                SubItems.Single(y => y.Text == menuHeader).
                    SubItems.Add(new MenuBarItem()
                    {
                        Text = text,
                        Url = url,
                        SubItems = new List<MenuBarItem>()
                    });
        }

        public void AddSubMenuItem(int headerPosition, int menuPosition, string text, string url)
        {
            _menuBarItems[headerPosition].
                SubItems[menuPosition].
                    SubItems.Add(new MenuBarItem()
                    {
                        Text = text,
                        Url = url,
                        SubItems = new List<MenuBarItem>()
                    });
        }

        /* Javascript */
        public string GetLoginJavascript()
        {
            string js = string.Empty;

            js += " document.getElementById('btlogin').onclick = function () { \n";

            js += "     if (document.getElementById('logininputbox').style.display != 'block') { \n";
            js += "         document.getElementById('logininputbox').style.display = 'block'; \n";
            js += "     } \n";
            js += "     else { \n";
            js += "         document.getElementById('logininputbox').style.display = 'none'; \n";
            js += "     } \n";
            js += " }; \n";

            return js;
        }

        /* Get Html Objects */
        private Control GetMenusObj()
        {
            HtmlObjects navbar = new HtmlObjects("ul");
            navbar.AddAttribute("class", "navbar");

            //<img src="url" alt="some_text">
            HtmlObjects liPic = new HtmlObjects("li") {Id = "pic_container"};
            HtmlObjects pic = new HtmlObjects("img");
            pic.AddAttribute("src", "./img/logo/ntec_logo.png");
            pic.AddAttribute("alt", "");
            pic.AddAttribute("align", "middle");
            liPic.AddControls(pic.GetObject);
            navbar.AddControls(liPic.GetObject);
            
             
            foreach (MenuBarItem header in _menuBarItems)
            {
                HtmlObjects liHeader = new HtmlObjects("li");
                HtmlObjects aHeader = new HtmlObjects("a") { Text = header.Text };
                HtmlObjects ulMenu = null;

                if (header.Url != string.Empty) aHeader.AddAttribute("href", header.Url);

                if (header.SubItems.Count > 0)
                {
                    ulMenu = new HtmlObjects("ul");

                    foreach (MenuBarItem menu in header.SubItems)
                    {
                        HtmlObjects liMenu = new HtmlObjects("li");
                        HtmlObjects aMenu = new HtmlObjects("a") { Text = menu.Text };
                        HtmlObjects ulSubMenu = null;

                        if (menu.Url != string.Empty) aMenu.AddAttribute("href", menu.Url);

                        if (menu.SubItems.Count > 0)
                        {
                            ulSubMenu = new HtmlObjects("ul") { };

                            foreach (MenuBarItem subMenu in menu.SubItems)
                            {
                                HtmlObjects liSubMenu = new HtmlObjects("li");
                                HtmlObjects aSubMenu = new HtmlObjects("a") { Text = subMenu.Text };

                                if (subMenu.Url != string.Empty) aSubMenu.AddAttribute("href", subMenu.Url);

                                liSubMenu.AddControls(aSubMenu.GetObject);
                                ulSubMenu.AddControls(liSubMenu.GetObject);
                            }
                        }

                        liMenu.AddControls(aMenu.GetObject);
                        if (ulSubMenu != null) liMenu.AddControls(ulSubMenu.GetObject);

                        ulMenu.AddControls(liMenu.GetObject);
                    }
                }

                liHeader.AddControls(aHeader.GetObject);
                if (ulMenu != null) liHeader.AddControls(ulMenu.GetObject);

                // -------------------------------------------
                navbar.AddControls(liHeader.GetObject);
            }

            return navbar.GetObject;
        }

        private Control GetLoginObj(bool isUserLogOn, bool isUserAdmin)
        {
/*
            <div id="login" class="loginbox">
                <span id="btlogin">Login</span> OR <span id="btlogout">Logout</span>
                <span id="bthelp">About</span>
                <span id="btcustom">Customize</span>
                <span id="btbo">BO</span>

                <div id="logininputbox">
                    <span>user:</span>
                    <input id="btUser" type="text" runat="server" class="logintextbox"/>
                    <span>pass:</span>
                    <input id="btPass" type="password" runat="server"  class="logintextbox"/> 
                    <input id="run" type="button" runat="server" value="login" class="loginbutton" onserverclick="LoginButton_ServerClick"  /> 
                </div>
            </div>
*/

            // <div id="login" class="loginbox">
            HtmlObjects divLogin = new HtmlObjects("div") { Id = "login" };
            divLogin.AddAttribute("class", "loginbox");

            if (isUserLogOn)
            {
                // <span id="btlogout">Logout</span>
                Button btLogout = new Button() { Text = "Logout", ID = "btlogout", CssClass = "spanbutton" };
                btLogout.Click += LogoutButton_Click;
                divLogin.AddControls(btLogout);
            }

            // <span id="btabout">About</span>
            HtmlObjects btabout = new HtmlObjects("span") { Id = "bthelp", Text = "About" };
            btabout.AddAttribute("onclick", "$('#pageContainer').attr('src','help/about.html');");
            divLogin.AddControls(btabout.GetObject);

            if (isUserLogOn)
            {
                // <span id="btcustom">Customize</span>
                HtmlObjects btcustom = new HtmlObjects("span") { Id = "btcustom", Text = "Customize" };
                btcustom.AddAttribute("href", "bo/site/cfg_user_configs.aspx");
                divLogin.AddControls(btcustom.GetObject);
            }

            if (isUserAdmin)
            {
                // <span id="btconfig">BO</span>
                HtmlObjects btBo = new HtmlObjects("span") { Id = "btbo", Text = "BO" };
                btBo.AddAttribute("onclick", "window.location='bo/backoffice.aspx';");
                divLogin.AddControls(btBo.GetObject);
            }
            
            // <span id="btlogin">Login</span>
            divLogin.AddControls(new HtmlObjects("span") { Id = (isUserLogOn ? "welcome" : "btlogin"), Text = (isUserLogOn ? "Welcome" : "Login") }.GetObject);

            if (!isUserLogOn)
            {
                // <div id="logininputbox">
                HtmlObjects divLoginInputBox = new HtmlObjects("div") { Id = "logininputbox" };

                //<span>user:</span>
                //<input id="btUser" type="text" runat="server" class="logintextbox"/>
                HtmlObjects spanUser = new HtmlObjects("span") { Text = "user:" };
                TextBox tbUser = new TextBox() { ID = "tbUser", CssClass = "logintextbox" };
                
                divLoginInputBox.AddControls(spanUser.GetObject);
                divLoginInputBox.AddControls(tbUser);

                //<span>pass:</span>
                //<input id="btPass" type="password" runat="server"  class="logintextbox"/> 
                HtmlObjects spanPass = new HtmlObjects("span") { Text = "pass:" };
                TextBox tbPass = new TextBox() { ID = "tbPass", CssClass = "logintextbox", TextMode = TextBoxMode.Password };

                divLoginInputBox.AddControls(spanPass.GetObject);
                divLoginInputBox.AddControls(tbPass);

                //<input id="run" type="button" runat="server" value="login" class="loginbutton" onserverclick="LoginButton_ServerClick"  /> 
                Button btRun = new Button() { Text = "login", ID = "run", CssClass = "loginbutton" };
                btRun.Click += LoginButton_Click;

                divLoginInputBox.AddControls(btRun);

                // ----------------------------------------------------------------------
                divLogin.AddControls(divLoginInputBox.GetObject);
            }

            return divLogin.GetObject;
        }

        public Control Get()
        {
            UserLoginData uld = null; try { uld = (UserLoginData)db_config_sessions.GetUserAuthentication(); } catch {}

            bool isUserLogOn = uld != null ? true : false;
            bool isUSerAdmin = isUserLogOn && uld.User.AdMIn == 1 ? true : false;

            HtmlObjects headerBar = new HtmlObjects("div");
            headerBar.AddAttribute("class", "headerbar");

            headerBar.AddControls(GetMenusObj()); // menus
            headerBar.AddControls(GetLoginObj(isUserLogOn, isUSerAdmin)); // login

            return headerBar.GetObject;
        }

        /* Button login click event */
        void LoginButton_Click(object sender, EventArgs e)
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;

            string user = ((TextBox)page.FindControl("tbUser")).Text;
            string pass = ((TextBox)page.FindControl("tbPass")).Text;

            DbConfig.db_config_users dcu = new DbConfig.db_config_users(user);
            dcu.Open();

            DbConfig.UserLoginData uld = dcu.IsUserAuthenticated(pass);

            if (uld != null)
            {
                DbConfig.db_config_sessions.SetUserAuthentication(uld);

                Generic.PageRefresh();
            }
            else
                Generic.JavaScriptInjector("loginFail", "alert('login failed')");
        }

        /* Button logout click event */
        void LogoutButton_Click(object sender, EventArgs e)
        {
            db_config_sessions.SetUserAuthentication(null);

            Generic.PageRefresh();
        }
    }

    public class MenuBarItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public List<MenuBarItem> SubItems { get; set; }
    }
}