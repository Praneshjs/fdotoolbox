using System;
using System.Collections.Generic;
using System.Text;
using FdoToolbox.Core.Feature;
using OSGeo.FDO.Filter;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using Iesi.Collections.Generic;
using OSGeo.FDO.Schema;
using FdoToolbox.Core.Configuration;
using System.Xml.Serialization;
using System.IO;

namespace FdoToolbox.Core.ETL.Specialized
{
    /// <summary>
    /// Controls the <see cref="FdoJoin"/> operation
    /// </summary>
    public class FdoJoinOptions : IDisposable
    {
        private bool _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="FdoJoinOptions"/> class.
        /// </summary>
        /// <param name="owner">if set to <c>true</c> this will own the underlying connections and dispose of them when it itself is disposed</param>
        public FdoJoinOptions(bool owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FdoJoinOptions"/> class.
        /// </summary>
        public FdoJoinOptions()
            : this(false)
        {
        }

        private int _BatchSize;

        /// <summary>
        /// Gets or sets the batch size for batched inserts. Only applies if the
        /// target connection supports batch insertion.
        /// </summary>
        public int BatchSize
        {
            get { return _BatchSize; }
            set { _BatchSize = value; }
        }
	

        private FdoJoinType _JoinType;

        /// <summary>
        /// Gets or sets the type of join operation
        /// </summary>
        public FdoJoinType JoinType
        {
            get { return _JoinType; }
            set { _JoinType = value; }
        }

        private FdoSource _Left;

        /// <summary>
        /// Gets the left join source
        /// </summary>
        public FdoSource Left { get { return _Left; } }

        private FdoSource _Right;

        /// <summary>
        /// Gets the right join source
        /// </summary>
        public FdoSource Right { get { return _Right; } }

        private FdoSource _Target;

        /// <summary>
        /// Gets the join target
        /// </summary>
        public FdoSource Target { get { return _Target; } }

        private string _LeftPrefix;

        /// <summary>
        /// Gets or sets the left column prefix that is applied in the event of a name collision in the merged feature
        /// </summary>
        public string LeftPrefix
        {
            get { return _LeftPrefix; }
            set { _LeftPrefix = value; }
        }

        private string _RightPrefix;

        /// <summary>
        /// Gets or sets the right column prefix that is applied in the event of a name collision in the merged feature
        /// </summary>
        public string RightPrefix
        {
            get { return _RightPrefix; }
            set { _RightPrefix = value; }
        }

        private IList<string> _LeftProperties = new List<string>();

        /// <summary>
        /// Gets the property collection for the left side of the join
        /// </summary>
        public ICollection<string> LeftProperties
        {
            get { return _LeftProperties; }
        }

        private IList<string> _RightProperties = new List<string>();

        /// <summary>
        /// Gets the property collection for the right side of the join
        /// </summary>
        public ICollection<string> RightProperties
        {
            get { return _RightProperties; }
        }

        /// <summary>
        /// Adds a property to the left side of the join
        /// </summary>
        /// <param name="propertyName"></param>
        public void AddLeftProperty(string propertyName)
        {
            _LeftProperties.Add(propertyName);
        }

        /// <summary>
        /// Adds a property to the right side of the join
        /// </summary>
        /// <param name="propertyName"></param>
        public void AddRightProperty(string propertyName)
        {
            _RightProperties.Add(propertyName);
        }

        private NameValueCollection _joinPairs = new NameValueCollection();

        /// <summary>
        /// Gets the join pair collection
        /// </summary>
        public NameValueCollection JoinPairs
        {
            get { return _joinPairs; }
        }

        /// <summary>
        /// Sets the join pair collection
        /// </summary>
        /// <param name="pairs"></param>
        public void SetJoinPairs(NameValueCollection pairs)
        {
            _joinPairs.Add(pairs);
        }

        /// <summary>
        /// Sets the left source of the join
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="schemaName"></param>
        /// <param name="className"></param>
        public void SetLeft(FdoConnection conn, string schemaName, string className)
        {
            _Left = new FdoSource(conn, schemaName, className);
        }

        /// <summary>
        /// Sets the right source of the join
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="schemaName"></param>
        /// <param name="className"></param>
        public void SetRight(FdoConnection conn, string schemaName, string className)
        {
            _Right = new FdoSource(conn, schemaName, className);
        }

        /// <summary>
        /// Sets the target of the join
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="schemaName"></param>
        /// <param name="className"></param>
        public void SetTarget(FdoConnection conn, string schemaName, string className)
        {
            _Target = new FdoSource(conn, schemaName, className);
        }

        private SpatialOperations? _SpatialJoinPredicate;

        /// <summary>
        /// Gets or sets the spatial join predicate. This only applies if both sides of
        /// the join have geometric properties
        /// </summary>
        public SpatialOperations? SpatialJoinPredicate
        {
            get { return _SpatialJoinPredicate; }
            set { _SpatialJoinPredicate = value; }
        }
	
        /// <summary>
        /// Creates the query for the left side of the join
        /// </summary>
        /// <returns></returns>
        internal FeatureQueryOptions CreateLeftQuery()
        {
            FeatureQueryOptions qry = new FeatureQueryOptions(_Left.ClassName);
            return qry;
        }

        /// <summary>
        /// Creates the query for the right side of the join
        /// </summary>
        /// <returns></returns>
        internal FeatureQueryOptions CreateRightQuery()
        {
            FeatureQueryOptions qry = new FeatureQueryOptions(_Right.ClassName);
            return qry;
        }

        private string _GeometryProperty;

        /// <summary>
        /// Gets or sets the geometry property which will be the designated geometry
        /// property on the joined feature class. If a prefix has been specified, the
        /// geometry property must also be prefixed.
        /// </summary>
        public string GeometryProperty
        {
            get { return _GeometryProperty; }
            set { _GeometryProperty = value; }
        }

        private bool _ForceOneToOne;

        /// <summary>
        /// If true, will only merge the left side with the first matching result
        /// on the right side. Otherwise, all matching results on the right side
        /// are merged.
        /// </summary>
        public bool ForceOneToOne
        {
            get { return _ForceOneToOne; }
            set { _ForceOneToOne = value; }
        }

        /// <summary>
        /// Validates these options
        /// </summary>
        public void Validate()
        {
            if (_Left == null)
                throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_LEFT_UNDEFINED"));

            if (_Right == null)
                throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_RIGHT_UNDEFINED"));

            if (_Target == null)
                throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_TARGET_UNDEFINED"));

            if (string.IsNullOrEmpty(_Target.ClassName))
                throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_TARGET_CLASS_UNDEFINED"));

            if (this.JoinPairs.Count == 0 && !this.SpatialJoinPredicate.HasValue)
                throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_KEYS_UNDEFINED"));

            int count = this.LeftProperties.Count + this.RightProperties.Count;

            if (string.IsNullOrEmpty(_LeftPrefix) && string.IsNullOrEmpty(_RightPrefix))
            {
                ISet<string> set = new HashedSet<string>();
                set.AddAll(this.LeftProperties);
                set.AddAll(this.RightProperties);

                //If all properties are unique then the counts should be the same
                if (set.Count < count)
                    throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_PROPERTY_NAME_COLLISION"));
            }

            //Create target class. The schema must already exist, but the class must *not* already exist.
            using (FdoFeatureService service = this.Target.Connection.CreateFeatureService())
            {
                if (!service.SupportsCommand(OSGeo.FDO.Commands.CommandType.CommandType_ApplySchema))
                    throw new TaskValidationException(ResourceUtil.GetStringFormatted("ERR_UNSUPPORTED_CMD", OSGeo.FDO.Commands.CommandType.CommandType_ApplySchema));

                //Get target schema
                FeatureSchema schema = service.GetSchemaByName(this.Target.SchemaName);
                if (schema == null)
                    throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_SCHEMA_NOT_FOUND"));

                //Check target class does not exist
                int cidx = schema.Classes.IndexOf(this.Target.ClassName);
                if (cidx >= 0)
                    throw new TaskValidationException(ResourceUtil.GetString("ERR_JOIN_TARGET_EXISTS"));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_owner)
            {
                _Left.Connection.Dispose();
                _Right.Connection.Dispose();
                _Target.Connection.Dispose();
            }
        }
    }

    /// <summary>
    /// Defines a FDO source
    /// </summary>
    public class FdoSource
    {
        private FdoConnection _Connection;

        /// <summary>
        /// The connection for this source
        /// </summary>
        public FdoConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }

        private string _SchemaName;

        /// <summary>
        /// The schema name
        /// </summary>
        public string SchemaName            
        {
            get { return _SchemaName; }
            set { _SchemaName = value; }
        }

        private string _ClassName;

        /// <summary>
        /// The class name
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="schema"></param>
        /// <param name="className"></param>
        public FdoSource(FdoConnection conn, string schema, string className)
        {
            _Connection = conn;
            _SchemaName = schema;
            _ClassName = className;
        }
    }

    /// <summary>
    /// Defines the possible join types
    /// </summary>
    public enum FdoJoinType
    {
        /// <summary>
        /// Inner join, only matching objects from both sides are merged
        /// </summary>
        Inner,
        /// <summary>
        /// Left join, left side objects are merged with right side objects regardless of whether the right side object exists or not
        /// </summary>
        Left,
        /// <summary>
        /// Right join, right side objects are merged with left side objects regardless of whether the left side object exists or not
        /// </summary>
        Right,
        /// <summary>
        /// Full join, both sides of the join are merged regardless of whether either side exists or not
        /// </summary>
        Full,
    }
}