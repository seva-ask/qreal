﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("QRealModel", "FK_GraphicParents", "GraphicInstances", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(QReal.Web.Database.GraphicInstance), "GraphicInstances1", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(QReal.Web.Database.GraphicInstance), true)]
[assembly: EdmRelationshipAttribute("QRealModel", "FK_GraphicToLogical", "LogicalInstances", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(QReal.Web.Database.LogicalInstance), "GraphicInstances", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(QReal.Web.Database.GraphicInstance), true)]
[assembly: EdmRelationshipAttribute("QRealModel", "FK_PropertiesToLogicalInstances", "LogicalInstances", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(QReal.Web.Database.LogicalInstance), "InstanceProperties", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(QReal.Web.Database.InstanceProperty), true)]

#endregion

namespace QReal.Web.Database
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class QRealEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new QRealEntities object using the connection string found in the 'QRealEntities' section of the application configuration file.
        /// </summary>
        public QRealEntities() : base("name=QRealEntities", "QRealEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new QRealEntities object.
        /// </summary>
        public QRealEntities(string connectionString) : base(connectionString, "QRealEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new QRealEntities object.
        /// </summary>
        public QRealEntities(EntityConnection connection) : base(connection, "QRealEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<GraphicInstance> GraphicInstances
        {
            get
            {
                if ((_GraphicInstances == null))
                {
                    _GraphicInstances = base.CreateObjectSet<GraphicInstance>("GraphicInstances");
                }
                return _GraphicInstances;
            }
        }
        private ObjectSet<GraphicInstance> _GraphicInstances;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<InstanceProperty> InstanceProperties
        {
            get
            {
                if ((_InstanceProperties == null))
                {
                    _InstanceProperties = base.CreateObjectSet<InstanceProperty>("InstanceProperties");
                }
                return _InstanceProperties;
            }
        }
        private ObjectSet<InstanceProperty> _InstanceProperties;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<LogicalInstance> LogicalInstances
        {
            get
            {
                if ((_LogicalInstances == null))
                {
                    _LogicalInstances = base.CreateObjectSet<LogicalInstance>("LogicalInstances");
                }
                return _LogicalInstances;
            }
        }
        private ObjectSet<LogicalInstance> _LogicalInstances;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the GraphicInstances EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToGraphicInstances(GraphicInstance graphicInstance)
        {
            base.AddObject("GraphicInstances", graphicInstance);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the InstanceProperties EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToInstanceProperties(InstanceProperty instanceProperty)
        {
            base.AddObject("InstanceProperties", instanceProperty);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the LogicalInstances EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToLogicalInstances(LogicalInstance logicalInstance)
        {
            base.AddObject("LogicalInstances", logicalInstance);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="QRealModel", Name="GraphicInstance")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class GraphicInstance : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new GraphicInstance object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="logicalId">Initial value of the LogicalId property.</param>
        /// <param name="x">Initial value of the X property.</param>
        /// <param name="y">Initial value of the Y property.</param>
        /// <param name="width">Initial value of the Width property.</param>
        /// <param name="height">Initial value of the Height property.</param>
        public static GraphicInstance CreateGraphicInstance(global::System.Int32 id, global::System.Int32 logicalId, global::System.Double x, global::System.Double y, global::System.Double width, global::System.Double height)
        {
            GraphicInstance graphicInstance = new GraphicInstance();
            graphicInstance.Id = id;
            graphicInstance.LogicalId = logicalId;
            graphicInstance.X = x;
            graphicInstance.Y = y;
            graphicInstance.Width = width;
            graphicInstance.Height = height;
            return graphicInstance;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 LogicalId
        {
            get
            {
                return _LogicalId;
            }
            set
            {
                OnLogicalIdChanging(value);
                ReportPropertyChanging("LogicalId");
                _LogicalId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("LogicalId");
                OnLogicalIdChanged();
            }
        }
        private global::System.Int32 _LogicalId;
        partial void OnLogicalIdChanging(global::System.Int32 value);
        partial void OnLogicalIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> ParentId
        {
            get
            {
                return _ParentId;
            }
            set
            {
                OnParentIdChanging(value);
                ReportPropertyChanging("ParentId");
                _ParentId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("ParentId");
                OnParentIdChanged();
            }
        }
        private Nullable<global::System.Int32> _ParentId;
        partial void OnParentIdChanging(Nullable<global::System.Int32> value);
        partial void OnParentIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Double X
        {
            get
            {
                return _X;
            }
            set
            {
                OnXChanging(value);
                ReportPropertyChanging("X");
                _X = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("X");
                OnXChanged();
            }
        }
        private global::System.Double _X;
        partial void OnXChanging(global::System.Double value);
        partial void OnXChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                OnYChanging(value);
                ReportPropertyChanging("Y");
                _Y = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Y");
                OnYChanged();
            }
        }
        private global::System.Double _Y;
        partial void OnYChanging(global::System.Double value);
        partial void OnYChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                OnWidthChanging(value);
                ReportPropertyChanging("Width");
                _Width = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Width");
                OnWidthChanged();
            }
        }
        private global::System.Double _Width;
        partial void OnWidthChanging(global::System.Double value);
        partial void OnWidthChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                OnHeightChanging(value);
                ReportPropertyChanging("Height");
                _Height = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Height");
                OnHeightChanged();
            }
        }
        private global::System.Double _Height;
        partial void OnHeightChanging(global::System.Double value);
        partial void OnHeightChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("QRealModel", "FK_GraphicParents", "GraphicInstances1")]
        public EntityCollection<GraphicInstance> Children
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<GraphicInstance>("QRealModel.FK_GraphicParents", "GraphicInstances1");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<GraphicInstance>("QRealModel.FK_GraphicParents", "GraphicInstances1", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("QRealModel", "FK_GraphicParents", "GraphicInstances")]
        public GraphicInstance Parent
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<GraphicInstance>("QRealModel.FK_GraphicParents", "GraphicInstances").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<GraphicInstance>("QRealModel.FK_GraphicParents", "GraphicInstances").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<GraphicInstance> ParentReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<GraphicInstance>("QRealModel.FK_GraphicParents", "GraphicInstances");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<GraphicInstance>("QRealModel.FK_GraphicParents", "GraphicInstances", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("QRealModel", "FK_GraphicToLogical", "LogicalInstances")]
        public LogicalInstance LogicalInstance
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LogicalInstance>("QRealModel.FK_GraphicToLogical", "LogicalInstances").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LogicalInstance>("QRealModel.FK_GraphicToLogical", "LogicalInstances").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<LogicalInstance> LogicalInstanceReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LogicalInstance>("QRealModel.FK_GraphicToLogical", "LogicalInstances");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<LogicalInstance>("QRealModel.FK_GraphicToLogical", "LogicalInstances", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="QRealModel", Name="InstanceProperty")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class InstanceProperty : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new InstanceProperty object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="instanceId">Initial value of the InstanceId property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        public static InstanceProperty CreateInstanceProperty(global::System.Int32 id, global::System.Int32 instanceId, global::System.String name)
        {
            InstanceProperty instanceProperty = new InstanceProperty();
            instanceProperty.Id = id;
            instanceProperty.InstanceId = instanceId;
            instanceProperty.Name = name;
            return instanceProperty;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 InstanceId
        {
            get
            {
                return _InstanceId;
            }
            set
            {
                OnInstanceIdChanging(value);
                ReportPropertyChanging("InstanceId");
                _InstanceId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("InstanceId");
                OnInstanceIdChanged();
            }
        }
        private global::System.Int32 _InstanceId;
        partial void OnInstanceIdChanging(global::System.Int32 value);
        partial void OnInstanceIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Value
        {
            get
            {
                return _Value;
            }
            set
            {
                OnValueChanging(value);
                ReportPropertyChanging("Value");
                _Value = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Value");
                OnValueChanged();
            }
        }
        private global::System.String _Value;
        partial void OnValueChanging(global::System.String value);
        partial void OnValueChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("QRealModel", "FK_PropertiesToLogicalInstances", "LogicalInstances")]
        public LogicalInstance LogicalInstance
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LogicalInstance>("QRealModel.FK_PropertiesToLogicalInstances", "LogicalInstances").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LogicalInstance>("QRealModel.FK_PropertiesToLogicalInstances", "LogicalInstances").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<LogicalInstance> LogicalInstanceReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LogicalInstance>("QRealModel.FK_PropertiesToLogicalInstances", "LogicalInstances");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<LogicalInstance>("QRealModel.FK_PropertiesToLogicalInstances", "LogicalInstances", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="QRealModel", Name="LogicalInstance")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class LogicalInstance : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new LogicalInstance object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="type">Initial value of the Type property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        public static LogicalInstance CreateLogicalInstance(global::System.Int32 id, global::System.String type, global::System.String name)
        {
            LogicalInstance logicalInstance = new LogicalInstance();
            logicalInstance.Id = id;
            logicalInstance.Type = type;
            logicalInstance.Name = name;
            return logicalInstance;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Type
        {
            get
            {
                return _Type;
            }
            set
            {
                OnTypeChanging(value);
                ReportPropertyChanging("Type");
                _Type = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Type");
                OnTypeChanged();
            }
        }
        private global::System.String _Type;
        partial void OnTypeChanging(global::System.String value);
        partial void OnTypeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("QRealModel", "FK_GraphicToLogical", "GraphicInstances")]
        public EntityCollection<GraphicInstance> GraphicInstances
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<GraphicInstance>("QRealModel.FK_GraphicToLogical", "GraphicInstances");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<GraphicInstance>("QRealModel.FK_GraphicToLogical", "GraphicInstances", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("QRealModel", "FK_PropertiesToLogicalInstances", "InstanceProperties")]
        public EntityCollection<InstanceProperty> InstanceProperties
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<InstanceProperty>("QRealModel.FK_PropertiesToLogicalInstances", "InstanceProperties");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<InstanceProperty>("QRealModel.FK_PropertiesToLogicalInstances", "InstanceProperties", value);
                }
            }
        }

        #endregion
    }

    #endregion
    
}