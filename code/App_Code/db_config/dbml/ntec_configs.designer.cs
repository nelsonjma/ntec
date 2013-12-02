// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from NtEcConFigs on 2013-07-15 20:09:36Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
namespace DbConfig
{
    using System;
    using System.ComponentModel;
    using System.Data;
#if MONO_STRICT
	using System.Data.Linq;
#else   // MONO_STRICT
    using DbLinq.Data.Linq;
    using DbLinq.Vendor;
#endif  // MONO_STRICT
    using System.Data.Linq.Mapping;
    using System.Diagnostics;


    public partial class NtEcConFigs : DataContext
    {

        #region Extensibility Method Declarations
        partial void OnCreated();
        #endregion


        public NtEcConFigs(string connectionString) :
            base(connectionString)
        {
            this.OnCreated();
        }

        public NtEcConFigs(string connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            this.OnCreated();
        }

        public NtEcConFigs(IDbConnection connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            this.OnCreated();
        }

        public Table<Frame> Frame
        {
            get
            {
                return this.GetTable<Frame>();
            }
        }

        public Table<FrontOffice> FrontOffice
        {
            get
            {
                return this.GetTable<FrontOffice>();
            }
        }

        public Table<MasterPage> MasterPage
        {
            get
            {
                return this.GetTable<MasterPage>();
            }
        }

        public Table<Options> Options
        {
            get
            {
                return this.GetTable<Options>();
            }
        }

        public Table<Page> Page
        {
            get
            {
                return this.GetTable<Page>();
            }
        }

        public Table<PageUsers> PageUsers
        {
            get
            {
                return this.GetTable<PageUsers>();
            }
        }

        public Table<Users> Users
        {
            get
            {
                return this.GetTable<Users>();
            }
        }
    }

    #region Start MONO_STRICT
#if MONO_STRICT

	public partial class NtEcConFigs
	{
		
		public NtEcConFigs(IDbConnection connection) : 
				base(connection)
		{
			this.OnCreated();
		}
	}
    #region End MONO_STRICT
    #endregion
#else     // MONO_STRICT

    public partial class NtEcConFigs
    {

        public NtEcConFigs(IDbConnection connection) :
            base(connection, new DbLinq.Sqlite.SqliteVendor())
        {
            this.OnCreated();
        }

        public NtEcConFigs(IDbConnection connection, IVendor sqlDialect) :
            base(connection, sqlDialect)
        {
            this.OnCreated();
        }

        public NtEcConFigs(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) :
            base(connection, mappingSource, sqlDialect)
        {
            this.OnCreated();
        }
    }
    #region End Not MONO_STRICT
    #endregion
#endif     // MONO_STRICT
    #endregion

    [Table(Name = "main.frame")]
    public partial class Frame : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private string _frameType;

        private int _height;

        private int _id;

        private int _idpAge;

        private int _isActive;

        private string _lastUpd;

        private string _options;

        private System.Nullable<int> _scheduleInterval;

        private string _scroll;

        private string _title;

        private int _width;

        private int _x;

        private int _y;

        private EntityRef<Page> _page = new EntityRef<Page>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnFrameTypeChanged();

        partial void OnFrameTypeChanging(string value);

        partial void OnHeightChanged();

        partial void OnHeightChanging(int value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnIDPageChanged();

        partial void OnIDPageChanging(int value);

        partial void OnIsActiveChanged();

        partial void OnIsActiveChanging(int value);

        partial void OnLastUpdChanged();

        partial void OnLastUpdChanging(string value);

        partial void OnOptionsChanged();

        partial void OnOptionsChanging(string value);

        partial void OnScheduleIntervalChanged();

        partial void OnScheduleIntervalChanging(System.Nullable<int> value);

        partial void OnScrollChanged();

        partial void OnScrollChanging(string value);

        partial void OnTitleChanged();

        partial void OnTitleChanging(string value);

        partial void OnWidthChanged();

        partial void OnWidthChanging(int value);

        partial void OnXChanged();

        partial void OnXChanging(int value);

        partial void OnYChanged();

        partial void OnYChanging(int value);
        #endregion


        public Frame()
        {
            this.OnCreated();
        }

        [Column(Storage = "_frameType", Name = "frame_type", DbType = "VARCHAR(100)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string FrameType
        {
            get
            {
                return this._frameType;
            }
            set
            {
                if (((_frameType == value)
                            == false))
                {
                    this.OnFrameTypeChanging(value);
                    this.SendPropertyChanging();
                    this._frameType = value;
                    this.SendPropertyChanged("FrameType");
                    this.OnFrameTypeChanged();
                }
            }
        }

        [Column(Storage = "_height", Name = "height", DbType = "INT", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if ((_height != value))
                {
                    this.OnHeightChanging(value);
                    this.SendPropertyChanging();
                    this._height = value;
                    this.SendPropertyChanged("Height");
                    this.OnHeightChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_idpAge", Name = "id_page", DbType = "INT", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int IDPage
        {
            get
            {
                return this._idpAge;
            }
            set
            {
                if ((_idpAge != value))
                {
                    this.OnIDPageChanging(value);
                    this.SendPropertyChanging();
                    this._idpAge = value;
                    this.SendPropertyChanged("IDPage");
                    this.OnIDPageChanged();
                }
            }
        }

        [Column(Storage = "_isActive", Name = "is_active", DbType = "INT", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                if ((_isActive != value))
                {
                    this.OnIsActiveChanging(value);
                    this.SendPropertyChanging();
                    this._isActive = value;
                    this.SendPropertyChanged("IsActive");
                    this.OnIsActiveChanged();
                }
            }
        }

        [Column(Storage = "_lastUpd", Name = "lastupd", DbType = "VARCHAR(20)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string LastUpd
        {
            get
            {
                return this._lastUpd;
            }
            set
            {
                if (((_lastUpd == value)
                            == false))
                {
                    this.OnLastUpdChanging(value);
                    this.SendPropertyChanging();
                    this._lastUpd = value;
                    this.SendPropertyChanged("LastUpd");
                    this.OnLastUpdChanged();
                }
            }
        }

        [Column(Storage = "_options", Name = "options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Options
        {
            get
            {
                return this._options;
            }
            set
            {
                if (((_options == value)
                            == false))
                {
                    this.OnOptionsChanging(value);
                    this.SendPropertyChanging();
                    this._options = value;
                    this.SendPropertyChanged("Options");
                    this.OnOptionsChanged();
                }
            }
        }

        [Column(Storage = "_scheduleInterval", Name = "schedule_interval", DbType = "INT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public System.Nullable<int> ScheduleInterval
        {
            get
            {
                return this._scheduleInterval;
            }
            set
            {
                if ((_scheduleInterval != value))
                {
                    this.OnScheduleIntervalChanging(value);
                    this.SendPropertyChanging();
                    this._scheduleInterval = value;
                    this.SendPropertyChanged("ScheduleInterval");
                    this.OnScheduleIntervalChanged();
                }
            }
        }

        [Column(Storage = "_scroll", Name = "scroll", DbType = "VARCHAR(15)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Scroll
        {
            get
            {
                return this._scroll;
            }
            set
            {
                if (((_scroll == value)
                            == false))
                {
                    this.OnScrollChanging(value);
                    this.SendPropertyChanging();
                    this._scroll = value;
                    this.SendPropertyChanged("Scroll");
                    this.OnScrollChanged();
                }
            }
        }

        [Column(Storage = "_title", Name = "title", DbType = "VARCHAR(255)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (((_title == value)
                            == false))
                {
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging();
                    this._title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }

        [Column(Storage = "_width", Name = "width", DbType = "INT", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if ((_width != value))
                {
                    this.OnWidthChanging(value);
                    this.SendPropertyChanging();
                    this._width = value;
                    this.SendPropertyChanged("Width");
                    this.OnWidthChanged();
                }
            }
        }

        [Column(Storage = "_x", Name = "x", DbType = "INT", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int X
        {
            get
            {
                return this._x;
            }
            set
            {
                if ((_x != value))
                {
                    this.OnXChanging(value);
                    this.SendPropertyChanging();
                    this._x = value;
                    this.SendPropertyChanged("X");
                    this.OnXChanged();
                }
            }
        }

        [Column(Storage = "_y", Name = "y", DbType = "INT", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int Y
        {
            get
            {
                return this._y;
            }
            set
            {
                if ((_y != value))
                {
                    this.OnYChanging(value);
                    this.SendPropertyChanging();
                    this._y = value;
                    this.SendPropertyChanged("Y");
                    this.OnYChanged();
                }
            }
        }

        #region Parents
        [Association(Storage = "_page", OtherKey = "ID", ThisKey = "IDPage", Name = "fk_frame_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Page Page
        {
            get
            {
                return this._page.Entity;
            }
            set
            {
                if (((this._page.Entity == value)
                            == false))
                {
                    if ((this._page.Entity != null))
                    {
                        Page previousPage = this._page.Entity;
                        this._page.Entity = null;
                        previousPage.Frame.Remove(this);
                    }
                    this._page.Entity = value;
                    if ((value != null))
                    {
                        value.Frame.Add(this);
                        _idpAge = value.ID;
                    }
                    else
                    {
                        _idpAge = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table(Name = "main.frontoffice")]
    public partial class FrontOffice : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private System.Nullable<int> _id;

        private string _lastUpd;

        private string _options;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(System.Nullable<int> value);

        partial void OnLastUpdChanged();

        partial void OnLastUpdChanging(string value);

        partial void OnOptionsChanged();

        partial void OnOptionsChanging(string value);
        #endregion


        public FrontOffice()
        {
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public System.Nullable<int> ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_lastUpd", Name = "lastupd", DbType = "VARCHAR(20)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string LastUpd
        {
            get
            {
                return this._lastUpd;
            }
            set
            {
                if (((_lastUpd == value)
                            == false))
                {
                    this.OnLastUpdChanging(value);
                    this.SendPropertyChanging();
                    this._lastUpd = value;
                    this.SendPropertyChanged("LastUpd");
                    this.OnLastUpdChanged();
                }
            }
        }

        [Column(Storage = "_options", Name = "options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Options
        {
            get
            {
                return this._options;
            }
            set
            {
                if (((_options == value)
                            == false))
                {
                    this.OnOptionsChanging(value);
                    this.SendPropertyChanging();
                    this._options = value;
                    this.SendPropertyChanged("Options");
                    this.OnOptionsChanged();
                }
            }
        }

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table(Name = "main.master_page")]
    public partial class MasterPage : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private string _description;

        private int _id;

        private string _options;

        private string _title;

        private EntitySet<Page> _page;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnDescriptionChanged();

        partial void OnDescriptionChanging(string value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnOptionsChanged();

        partial void OnOptionsChanging(string value);

        partial void OnTitleChanged();

        partial void OnTitleChanging(string value);
        #endregion


        public MasterPage()
        {
            _page = new EntitySet<Page>(new Action<Page>(this.Page_Attach), new Action<Page>(this.Page_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_description", Name = "description", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (((_description == value)
                            == false))
                {
                    this.OnDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._description = value;
                    this.SendPropertyChanged("Description");
                    this.OnDescriptionChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_options", Name = "options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Options
        {
            get
            {
                return this._options;
            }
            set
            {
                if (((_options == value)
                            == false))
                {
                    this.OnOptionsChanging(value);
                    this.SendPropertyChanging();
                    this._options = value;
                    this.SendPropertyChanged("Options");
                    this.OnOptionsChanged();
                }
            }
        }

        [Column(Storage = "_title", Name = "title", DbType = "VARCHAR(255)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (((_title == value)
                            == false))
                {
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging();
                    this._title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_page", OtherKey = "IDMasterPage", ThisKey = "ID", Name = "fk_page_0")]
        [DebuggerNonUserCode()]
        public EntitySet<Page> Page
        {
            get
            {
                return this._page;
            }
            set
            {
                this._page = value;
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void Page_Attach(Page entity)
        {
            this.SendPropertyChanging();
            entity.MasterPage = this;
        }

        private void Page_Detach(Page entity)
        {
            this.SendPropertyChanging();
            entity.MasterPage = null;
        }
        #endregion
    }

    [Table(Name = "main.options")]
    public partial class Options : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private string _lastUpd;

        private string _name;

        private string _objtYpe;

        private string _options1;

        private string _url;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnLastUpdChanged();

        partial void OnLastUpdChanging(string value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);

        partial void OnOBJTypeChanged();

        partial void OnOBJTypeChanging(string value);

        partial void OnOptions1Changed();

        partial void OnOptions1Changing(string value);

        partial void OnURLChanged();

        partial void OnURLChanging(string value);
        #endregion


        public Options()
        {
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_lastUpd", Name = "lastupd", DbType = "VARCHAR(20)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string LastUpd
        {
            get
            {
                return this._lastUpd;
            }
            set
            {
                if (((_lastUpd == value)
                            == false))
                {
                    this.OnLastUpdChanging(value);
                    this.SendPropertyChanging();
                    this._lastUpd = value;
                    this.SendPropertyChanged("LastUpd");
                    this.OnLastUpdChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "name", DbType = "NVARCHAR(255)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [Column(Storage = "_objtYpe", Name = "obj_type", DbType = "CHAR", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string OBJType
        {
            get
            {
                return this._objtYpe;
            }
            set
            {
                if (((_objtYpe == value)
                            == false))
                {
                    this.OnOBJTypeChanging(value);
                    this.SendPropertyChanging();
                    this._objtYpe = value;
                    this.SendPropertyChanged("OBJType");
                    this.OnOBJTypeChanged();
                }
            }
        }

        [Column(Storage = "_options1", Name = "options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Options1
        {
            get
            {
                return this._options1;
            }
            set
            {
                if (((_options1 == value)
                            == false))
                {
                    this.OnOptions1Changing(value);
                    this.SendPropertyChanging();
                    this._options1 = value;
                    this.SendPropertyChanged("Options1");
                    this.OnOptions1Changed();
                }
            }
        }

        [Column(Storage = "_url", Name = "url", DbType = "NVARCHAR(255)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string URL
        {
            get
            {
                return this._url;
            }
            set
            {
                if (((_url == value)
                            == false))
                {
                    this.OnURLChanging(value);
                    this.SendPropertyChanging();
                    this._url = value;
                    this.SendPropertyChanged("URL");
                    this.OnURLChanged();
                }
            }
        }

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table(Name = "main.page")]
    public partial class Page : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private int _idmAsterPage;

        private string _name;

        private string _options;

        private string _title;

        private string _xmlfOlderPath;

        private string _xmlurl;

        private EntitySet<Frame> _frame;

        private EntitySet<PageUsers> _pageUsers;

        private EntityRef<MasterPage> _masterPage = new EntityRef<MasterPage>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnIDMasterPageChanged();

        partial void OnIDMasterPageChanging(int value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);

        partial void OnOptionsChanged();

        partial void OnOptionsChanging(string value);

        partial void OnTitleChanged();

        partial void OnTitleChanging(string value);

        partial void OnXMLFolderPathChanged();

        partial void OnXMLFolderPathChanging(string value);

        partial void OnXMLURLChanged();

        partial void OnXMLURLChanging(string value);
        #endregion


        public Page()
        {
            _frame = new EntitySet<Frame>(new Action<Frame>(this.Frame_Attach), new Action<Frame>(this.Frame_Detach));
            _pageUsers = new EntitySet<PageUsers>(new Action<PageUsers>(this.PageUsers_Attach), new Action<PageUsers>(this.PageUsers_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_idmAsterPage", Name = "id_master_page", DbType = "INTEGER", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int IDMasterPage
        {
            get
            {
                return this._idmAsterPage;
            }
            set
            {
                if ((_idmAsterPage != value))
                {
                    this.OnIDMasterPageChanging(value);
                    this.SendPropertyChanging();
                    this._idmAsterPage = value;
                    this.SendPropertyChanged("IDMasterPage");
                    this.OnIDMasterPageChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "name", DbType = "VARCHAR(255)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [Column(Storage = "_options", Name = "options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Options
        {
            get
            {
                return this._options;
            }
            set
            {
                if (((_options == value)
                            == false))
                {
                    this.OnOptionsChanging(value);
                    this.SendPropertyChanging();
                    this._options = value;
                    this.SendPropertyChanged("Options");
                    this.OnOptionsChanged();
                }
            }
        }

        [Column(Storage = "_title", Name = "title", DbType = "VARCHAR(255)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (((_title == value)
                            == false))
                {
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging();
                    this._title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }

        [Column(Storage = "_xmlfOlderPath", Name = "xml_folder_path", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string XMLFolderPath
        {
            get
            {
                return this._xmlfOlderPath;
            }
            set
            {
                if (((_xmlfOlderPath == value)
                            == false))
                {
                    this.OnXMLFolderPathChanging(value);
                    this.SendPropertyChanging();
                    this._xmlfOlderPath = value;
                    this.SendPropertyChanged("XMLFolderPath");
                    this.OnXMLFolderPathChanged();
                }
            }
        }

        [Column(Storage = "_xmlurl", Name = "xml_url", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string XMLURL
        {
            get
            {
                return this._xmlurl;
            }
            set
            {
                if (((_xmlurl == value)
                            == false))
                {
                    this.OnXMLURLChanging(value);
                    this.SendPropertyChanging();
                    this._xmlurl = value;
                    this.SendPropertyChanged("XMLURL");
                    this.OnXMLURLChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_frame", OtherKey = "IDPage", ThisKey = "ID", Name = "fk_frame_0")]
        [DebuggerNonUserCode()]
        public EntitySet<Frame> Frame
        {
            get
            {
                return this._frame;
            }
            set
            {
                this._frame = value;
            }
        }

        [Association(Storage = "_pageUsers", OtherKey = "IDPage", ThisKey = "ID", Name = "fk_page_users_0")]
        [DebuggerNonUserCode()]
        public EntitySet<PageUsers> PageUsers
        {
            get
            {
                return this._pageUsers;
            }
            set
            {
                this._pageUsers = value;
            }
        }
        #endregion

        #region Parents
        [Association(Storage = "_masterPage", OtherKey = "ID", ThisKey = "IDMasterPage", Name = "fk_page_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public MasterPage MasterPage
        {
            get
            {
                return this._masterPage.Entity;
            }
            set
            {
                if (((this._masterPage.Entity == value)
                            == false))
                {
                    if ((this._masterPage.Entity != null))
                    {
                        MasterPage previousMasterPage = this._masterPage.Entity;
                        this._masterPage.Entity = null;
                        previousMasterPage.Page.Remove(this);
                    }
                    this._masterPage.Entity = value;
                    if ((value != null))
                    {
                        value.Page.Add(this);
                        _idmAsterPage = value.ID;
                    }
                    else
                    {
                        _idmAsterPage = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void Frame_Attach(Frame entity)
        {
            this.SendPropertyChanging();
            entity.Page = this;
        }

        private void Frame_Detach(Frame entity)
        {
            this.SendPropertyChanging();
            entity.Page = null;
        }

        private void PageUsers_Attach(PageUsers entity)
        {
            this.SendPropertyChanging();
            entity.Page = this;
        }

        private void PageUsers_Detach(PageUsers entity)
        {
            this.SendPropertyChanging();
            entity.Page = null;
        }
        #endregion
    }

    [Table(Name = "main.page_users")]
    public partial class PageUsers : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private int _idpAge;

        private int _iduSer;

        private string _lastUpd;

        private EntityRef<Page> _page = new EntityRef<Page>();

        private EntityRef<Users> _users = new EntityRef<Users>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnIDPageChanged();

        partial void OnIDPageChanging(int value);

        partial void OnIDUserChanged();

        partial void OnIDUserChanging(int value);

        partial void OnLastUpdChanged();

        partial void OnLastUpdChanging(string value);
        #endregion


        public PageUsers()
        {
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_idpAge", Name = "id_page", DbType = "INTEGER", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int IDPage
        {
            get
            {
                return this._idpAge;
            }
            set
            {
                if ((_idpAge != value))
                {
                    this.OnIDPageChanging(value);
                    this.SendPropertyChanging();
                    this._idpAge = value;
                    this.SendPropertyChanged("IDPage");
                    this.OnIDPageChanged();
                }
            }
        }

        [Column(Storage = "_iduSer", Name = "id_user", DbType = "INTEGER", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int IDUser
        {
            get
            {
                return this._iduSer;
            }
            set
            {
                if ((_iduSer != value))
                {
                    this.OnIDUserChanging(value);
                    this.SendPropertyChanging();
                    this._iduSer = value;
                    this.SendPropertyChanged("IDUser");
                    this.OnIDUserChanged();
                }
            }
        }

        [Column(Storage = "_lastUpd", Name = "lastupd", DbType = "VARCHAR(20)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string LastUpd
        {
            get
            {
                return this._lastUpd;
            }
            set
            {
                if (((_lastUpd == value)
                            == false))
                {
                    this.OnLastUpdChanging(value);
                    this.SendPropertyChanging();
                    this._lastUpd = value;
                    this.SendPropertyChanged("LastUpd");
                    this.OnLastUpdChanged();
                }
            }
        }

        #region Parents
        [Association(Storage = "_page", OtherKey = "ID", ThisKey = "IDPage", Name = "fk_page_users_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Page Page
        {
            get
            {
                return this._page.Entity;
            }
            set
            {
                if (((this._page.Entity == value)
                            == false))
                {
                    if ((this._page.Entity != null))
                    {
                        Page previousPage = this._page.Entity;
                        this._page.Entity = null;
                        previousPage.PageUsers.Remove(this);
                    }
                    this._page.Entity = value;
                    if ((value != null))
                    {
                        value.PageUsers.Add(this);
                        _idpAge = value.ID;
                    }
                    else
                    {
                        _idpAge = default(int);
                    }
                }
            }
        }

        [Association(Storage = "_users", OtherKey = "ID", ThisKey = "IDUser", Name = "fk_page_users_1", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Users Users
        {
            get
            {
                return this._users.Entity;
            }
            set
            {
                if (((this._users.Entity == value)
                            == false))
                {
                    if ((this._users.Entity != null))
                    {
                        Users previousUsers = this._users.Entity;
                        this._users.Entity = null;
                        previousUsers.PageUsers.Remove(this);
                    }
                    this._users.Entity = value;
                    if ((value != null))
                    {
                        value.PageUsers.Add(this);
                        _iduSer = value.ID;
                    }
                    else
                    {
                        _iduSer = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table(Name = "main.users")]
    public partial class Users : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private System.Nullable<int> _adMiN;

        private string _adMiNOptions;

        private string _description;

        private int _id;

        private string _lastUpd;

        private string _name;

        private string _pass;

        private string _userOptions;

        private EntitySet<PageUsers> _pageUsers;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnAdMInChanged();

        partial void OnAdMInChanging(System.Nullable<int> value);

        partial void OnAdMInOptionsChanged();

        partial void OnAdMInOptionsChanging(string value);

        partial void OnDescriptionChanged();

        partial void OnDescriptionChanging(string value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnLastUpdChanged();

        partial void OnLastUpdChanging(string value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);

        partial void OnPassChanged();

        partial void OnPassChanging(string value);

        partial void OnUserOptionsChanged();

        partial void OnUserOptionsChanging(string value);
        #endregion


        public Users()
        {
            _pageUsers = new EntitySet<PageUsers>(new Action<PageUsers>(this.PageUsers_Attach), new Action<PageUsers>(this.PageUsers_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_adMiN", Name = "admin", DbType = "INT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public System.Nullable<int> AdMIn
        {
            get
            {
                return this._adMiN;
            }
            set
            {
                if ((_adMiN != value))
                {
                    this.OnAdMInChanging(value);
                    this.SendPropertyChanging();
                    this._adMiN = value;
                    this.SendPropertyChanged("AdMIn");
                    this.OnAdMInChanged();
                }
            }
        }

        [Column(Storage = "_adMiNOptions", Name = "admin_options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string AdMInOptions
        {
            get
            {
                return this._adMiNOptions;
            }
            set
            {
                if (((_adMiNOptions == value)
                            == false))
                {
                    this.OnAdMInOptionsChanging(value);
                    this.SendPropertyChanging();
                    this._adMiNOptions = value;
                    this.SendPropertyChanged("AdMInOptions");
                    this.OnAdMInOptionsChanged();
                }
            }
        }

        [Column(Storage = "_description", Name = "description", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (((_description == value)
                            == false))
                {
                    this.OnDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._description = value;
                    this.SendPropertyChanged("Description");
                    this.OnDescriptionChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "id", DbType = "INTEGER", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_lastUpd", Name = "lastupd", DbType = "VARCHAR(20)", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string LastUpd
        {
            get
            {
                return this._lastUpd;
            }
            set
            {
                if (((_lastUpd == value)
                            == false))
                {
                    this.OnLastUpdChanging(value);
                    this.SendPropertyChanging();
                    this._lastUpd = value;
                    this.SendPropertyChanged("LastUpd");
                    this.OnLastUpdChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "name", DbType = "NVARCHAR(250)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [Column(Storage = "_pass", Name = "pass", DbType = "NVARCHAR(250)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Pass
        {
            get
            {
                return this._pass;
            }
            set
            {
                if (((_pass == value)
                            == false))
                {
                    this.OnPassChanging(value);
                    this.SendPropertyChanging();
                    this._pass = value;
                    this.SendPropertyChanged("Pass");
                    this.OnPassChanged();
                }
            }
        }

        [Column(Storage = "_userOptions", Name = "user_options", DbType = "TEXT", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string UserOptions
        {
            get
            {
                return this._userOptions;
            }
            set
            {
                if (((_userOptions == value)
                            == false))
                {
                    this.OnUserOptionsChanging(value);
                    this.SendPropertyChanging();
                    this._userOptions = value;
                    this.SendPropertyChanged("UserOptions");
                    this.OnUserOptionsChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_pageUsers", OtherKey = "IDUser", ThisKey = "ID", Name = "fk_page_users_1")]
        [DebuggerNonUserCode()]
        public EntitySet<PageUsers> PageUsers
        {
            get
            {
                return this._pageUsers;
            }
            set
            {
                this._pageUsers = value;
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void PageUsers_Attach(PageUsers entity)
        {
            this.SendPropertyChanging();
            entity.Users = this;
        }

        private void PageUsers_Detach(PageUsers entity)
        {
            this.SendPropertyChanging();
            entity.Users = null;
        }
        #endregion
    }
}
