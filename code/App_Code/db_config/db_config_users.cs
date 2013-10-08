using System;
using System.Collections.Generic;
using System.Linq;

namespace DbConfig
{
    /// <summary>
    /// Db to linq users/user pages configuration
    /// </summary>
    public class db_config_users : db_config
    {
        List<Users> _allUsers;

        private readonly string _userName;
        private readonly int _userId;

        public db_config_users()
        {
            _allUsers = new List<Users>();

            _userName = string.Empty;
            _userId = -1;
        }

        public db_config_users(int userId)
        {
            _allUsers = new List<Users>();

            _userName = string.Empty;
            _userId = userId;
        }
        
        public db_config_users(string userName)
        {
            _allUsers = new List<Users>();

            _userName = userName;
            _userId = -1;
        }

        public db_config_users(NtEcConFigs db, string userName)
        {
            _allUsers = new List<Users>();
            _db = db;
            _userName = userName;
            _userId = -1;

            SelectObjectsFromDb();
        }

        public db_config_users(NtEcConFigs db, int userId)
        {
            _allUsers = new List<Users>();
            _db = db;
            _userName = string.Empty;
            _userId = userId;

            SelectObjectsFromDb();
        }

        private Users Copy(Users u)
        {
            Users uNew = new Users
            {
                Name = u.Name,
                Pass = u.Pass,
                AdMIn = u.AdMIn,
                Description = u.Description,
                AdMInOptions = u.AdMInOptions,
                UserOptions = u.UserOptions
            };

            return uNew;
        }

        private PageUsers CopyPage(PageUsers pu)
        {
            PageUsers npu = new PageUsers
            {
                IDPage = pu.IDPage, 
                IDUser = pu.IDUser
            };

            return npu;
        }

        /// <summary>
        /// It will be a generic commit because of 2 pages.
        /// </summary>
        public override void Commit()
        {
            try
            {
                _db.SubmitChanges();
            }
            catch (Exception ex)
            {
                ReOpen();

                throw;
            }
        }

        public void CommitUsers()
        {
            try
            {
                _db.Users.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                ReOpen();

                throw;
            }
        }

        public void CommitPages()
        {
            try
            {
                _db.PageUsers.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                ReOpen();

                throw;
            }
        }

        protected override void SelectObjectsFromDb()
        {
            try
            {
                if (_userName != string.Empty)
                {
                    _allUsers = (from u in _db.Users
                                 where u.Name == _userName
                                 select u).ToList();
                }
                else if (_userId >= 0)
                {
                    _allUsers = (from u in _db.Users
                                 where u.ID == _userId
                                 select u).ToList();
                }
                else
                {
                    _allUsers = (from u in _db.Users
                                 select u).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: select object from db - " + ex.Message + " ...");
            } 
        }

        /* Get user data */
        public List<Users> AllUsers
        {
            get { return _allUsers; }
            set { _allUsers = value; }
        }

        public Users Get(string username)
        {
            try
            {
                return _allUsers.Single(x => x.Name == username);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return user with userName " + username + " - " + ex.Message + " ...");
            }
        }

        public Users Get(int id)
        {
            try
            {
                return _allUsers.Single(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return user with userName " + id + " - " + ex.Message + " ...");
            }
        }

        public List<Page> GetPages(string username)
        {
            try
            {
                using (_db)
                {
                    return (from u in _db.Users
                            join up in _db.PageUsers on u.ID equals up.IDUser
                            join p in _db.Page on up.IDPage equals p.ID
                            where u.Name == username
                            select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: in pages of user " + username + " - " + ex.Message + " ...");
            }
        }

        public List<Page> GetPages()
        {
            try
            {
                using (_db)
                {
                    if (_userName == string.Empty && _userId < 0)
                        throw new Exception("error: no user selected...");

                    Users user = _userName != string.Empty
                                                ? _db.Users.Single(x => x.Name == _userName)
                                                : _db.Users.Single(x => x.ID == _userId);

                    return (from u in _db.Users
                            join up in _db.PageUsers on u.ID equals up.IDUser
                            join p in _db.Page on up.IDPage equals p.ID
                            where u.ID == user.ID
                            select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: in pages of user " + _userName + " - " + ex.Message + " ...");
            }
        }

        /* Insert */
        public void AddPageToUser(int pageId)
        {
            try
            {
                if (_userName == string.Empty && _userId < 0)
                    throw new Exception("error: no user selected...");

                int userId = (_userName != string.Empty
                                                ? _db.Users.Single(x => x.Name == _userName)
                                                : _db.Users.Single(x => x.ID == _userId)).ID;

                PageUsers pu = new PageUsers
                {
                    IDUser = userId,
                    IDPage = pageId
                };

                _db.PageUsers.InsertOnSubmit(pu);

                CommitPages();
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new page to user " + _userName + " - " + ex.Message + " ...");
            }
        }

        public void AddPageToUser(string username, int pageId)
        {
            try
            {
                using (_db)
                {
                    int userId = _db.Users.Single(u => u.Name == username).ID;

                    PageUsers pu = new PageUsers
                    {
                        IDUser = userId,
                        IDPage = pageId
                    };

                    _db.PageUsers.InsertOnSubmit(pu);

                    CommitPages();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new page to user " + _userName + " - " + ex.Message + " ...");
            }
        }

        public void AddUser(Users user)
        {
            try
            {
                using (_db)
                {
                    _db.Users.InsertOnSubmit(user);

                    CommitUsers();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new user " + user.Name + " - " + ex.Message + " ...");
            }
        }

        /* Delete User */
        /// <summary>
        /// Delete user if you define username or userId in constructor
        /// </summary>
        public void DeleteUser()
        {
            try
            {
                using (_db)
                {
                    if (_userName == string.Empty && _userId < 0)
                        throw new Exception("error: no user selected...");

                    Users u = _userName != string.Empty
                                                ? _db.Users.Single(x => x.Name == _userName)
                                                : _db.Users.Single(x => x.ID == _userId);

                    DeleteAllUserPage(u.ID);

                    _db.Users.DeleteOnSubmit(u);

                    CommitUsers();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting userName " + _userName + " - " + ex.Message + " ...");
            }
        }

        /// <summary>
        /// Delete user by username
        /// </summary>
        public void DeleteUser(string userName)
        {
            try
            {
                using (_db)
                {
                    Users u = _db.Users.Single(x => x.Name == userName);

                    DeleteAllUserPage(u.ID);

                    _db.Users.DeleteOnSubmit(u);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting userName " + userName + " - " + ex.Message + " ...");
            }
        }

        /// <summary>
        /// Delete user by user rowid
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(int userId)
        {
            try
            {
                using (_db)
                {
                    Users u = _db.Users.Single(x => x.ID == userId);

                    DeleteAllUserPage(u.ID);

                    _db.Users.DeleteOnSubmit(u);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting userName " + userId + " - " + ex.Message + " ...");
            }
        }

        /* Delete user Pages */
        public void DeleteUserPage(string userName, int pageId)
        {
            try
            {
                using (_db)
                {
                    int userId = _db.Users.Single(x => x.Name == userName).ID;

                    PageUsers pu = _db.PageUsers.Single(x => x.IDUser == userId && x.IDPage == pageId);

                    _db.PageUsers.DeleteOnSubmit(pu);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting page " + pageId + " access from user " + userName + " - " + ex.Message + " ...");
            }
        }

        public void DeleteUserPage(int userId, int pageId)
        {
            try
            {
                using (_db)
                {
                    PageUsers pu = _db.PageUsers.Single(x => x.IDUser == userId && x.IDPage == pageId);

                    _db.PageUsers.DeleteOnSubmit(pu);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting page " + pageId + " access from user " + userId + " - " + ex.Message + " ...");
            }
        }

        public void DeleteUserPage(int pageId)
        {
            try
            {
                using (_db)
                {
                    if (_userName == string.Empty && _userId < 0)
                        throw new Exception("error: no user selected...");

                    int userId = (_userName != string.Empty
                                                    ? _db.Users.Single(x => x.Name == _userName)
                                                    : _db.Users.Single(x => x.ID == _userId)).ID;

                    PageUsers pu = _db.PageUsers.Single(x => x.IDUser == userId && x.IDPage == pageId);

                    _db.PageUsers.DeleteOnSubmit(pu);

                    Commit();

                    SelectObjectsFromDb();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting page " + pageId + " access from user " + _userName + " - " + ex.Message + " ...");
            }
        }

        private void DeleteAllUserPage(int userId)
        {
            try
            {
                List<PageUsers> pageUsers = (from pu in _db.PageUsers
                                             where pu.IDUser == userId
                                             select pu).ToList();

                foreach (PageUsers pu in pageUsers)
                {
                    _db.PageUsers.DeleteOnSubmit(pu);

                    CommitPages();
                }
            }
            catch { }
        }

        /* Clone */
        /// <summary>
        ///  Clone user if you define username or userId in constructor
        /// </summary>
        public void Clone(string newUserName)
        {
            try
            {
                using (_db)
                {
                    if (_userName == string.Empty && _userId == -1)
                        throw new Exception("error: no user selected...");

                    Users u = _userName != string.Empty
                                                        ? _db.Users.Single(x => x.Name == _userName)
                                                        : _db.Users.Single(x => x.ID == _userId);

                    Users uNew = Copy(u);

                    uNew.Name = newUserName;

                    _db.Users.InsertOnSubmit(uNew);

                    CommitUsers();

                    var pageUsers = from pu in _db.PageUsers where pu.IDUser == u.ID select pu;

                    int newUserId = _db.Users.Single(x => x.Name == uNew.Name).ID;

                    foreach (PageUsers pu in pageUsers)
                    {
                        PageUsers npu = CopyPage(pu);
                        npu.IDUser = newUserId;

                        _db.PageUsers.InsertOnSubmit(npu);

                        CommitPages();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: cloning user " + ex.Message + "...");
            }
        }

        public void Clone(string username, string newUserName)
        {
            try
            {
                using (_db)
                {
                    Users u = _db.Users.Single(x => x.Name == username);

                    Users uNew = Copy(u);

                    uNew.Name = newUserName;

                    _db.Users.InsertOnSubmit(uNew);

                    CommitUsers();

                    var pageUsers = from pu in _db.PageUsers
                                    where pu.IDUser == u.ID
                                    select pu;

                    int newUserId = _db.Users.Single(x => x.Name == uNew.Name).ID;

                    foreach (PageUsers pu in pageUsers)
                    {
                        PageUsers npu = CopyPage(pu);
                        npu.IDUser = newUserId;

                        _db.PageUsers.InsertOnSubmit(npu);

                        CommitPages();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: cloning user " + ex.Message + "...");
            }

        }

        /* Authenticate User */
        public UserLoginData IsUserAuthenticated(string pass)
        {
            try
            {
                using (_db)
                {
                    if (_userName == string.Empty)
                        throw new Exception("error: no user selected...");

                    Users u = Get(_userName);

                    if (pass == u.Pass)
                        return GetUserLoginData(u);
                }
            }
            catch {}


            return null;
        }

        public UserLoginData IsUserAuthenticated(string username, string pass)
        {
            try
            {
                using (_db)
                {
                    Users u = Get(username);
                    
                    if (pass == u.Pass)
                        return GetUserLoginData(u);
                }
            }
            catch { }

            return null;
        }

        private UserLoginData GetUserLoginData(Users u)
        {
            UserLoginData uld = new UserLoginData
            {
                User = u, 
                UserPages = new List<int>()
            };

            foreach (PageUsers pu in u.PageUsers)
            {
                uld.UserPages.Add(pu.IDPage);
            }

            return uld;
        }    
    }

    public class UserLoginData
    {
        public Users User { get; set; }
        public List<int> UserPages { get; set; }
    }
}