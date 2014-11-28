﻿// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 
namespace GeometryTypes
{
    using System.Xml.Serialization;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Building", Namespace = "", IsNullable = false)]
    public partial class TBuilding : TGeometryItem
    {
        private TFloor[] floorListField;

        private TStairway[] stairwayListField;

        private TBox roofField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Floor", IsNullable = false)]
        public TFloor[] FloorList
        {
            get
            {
                return this.floorListField;
            }
            set
            {
                this.floorListField = value;
                this.RaisePropertyChanged("FloorList");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Stairway", IsNullable = false)]
        public TStairway[] StairwayList
        {
            get
            {
                return this.stairwayListField;
            }
            set
            {
                this.stairwayListField = value;
                this.RaisePropertyChanged("StairwayList");
            }
        }

        /// <remarks/>
        public TBox Roof
        {
            get
            {
                return this.roofField;
            }
            set
            {
                this.roofField = value;
                this.RaisePropertyChanged("Roof");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return false; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TFloor : TGeometryItem
    {
        private int numberField;

        private TRoom[] roomListField;

        private TAperture[] apertureListField;

        /// <remarks/>
        public int Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Room", IsNullable = false)]
        public TRoom[] RoomList
        {
            get
            {
                return this.roomListField;
            }
            set
            {
                this.roomListField = value;
                this.RaisePropertyChanged("RoomList");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Aperture", IsNullable = false)]
        public TAperture[] ApertureList
        {
            get
            {
                return this.apertureListField;
            }
            set
            {
                this.apertureListField = value;
                this.RaisePropertyChanged("ApertureList");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return false; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TRoom : TGeometryItem
    {
        private TBox[] geometryField;

        private float fireLoadField;

        private float massField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("GeometryItem", IsNullable = false)]
        public TBox[] Geometry
        {
            get
            {
                return this.geometryField;
            }
            set
            {
                this.geometryField = value;
                this.RaisePropertyChanged("Geometry");
            }
        }

        /// <remarks/>
        public float FireLoad
        {
            get
            {
                return this.fireLoadField;
            }
            set
            {
                this.fireLoadField = value;
                this.RaisePropertyChanged("FireLoad");
            }
        }

        /// <remarks/>
        public float Mass
        {
            get
            {
                return this.massField;
            }
            set
            {
                this.massField = value;
                this.RaisePropertyChanged("Mass");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return false; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TBox : TGeometryItem
    {
        private float x1Field;

        private float y1Field;

        private float z1Field;

        private float x2Field;

        private float y2Field;

        private float z2Field;

        /// <remarks/>
        public float X1
        {
            get
            {
                return this.x1Field;
            }
            set
            {
                this.x1Field = value;
                this.RaisePropertyChanged("X1");
            }
        }

        /// <remarks/>
        public float Y1
        {
            get
            {
                return this.y1Field;
            }
            set
            {
                this.y1Field = value;
                this.RaisePropertyChanged("Y1");
            }
        }

        /// <remarks/>
        public float Z1
        {
            get
            {
                return this.z1Field;
            }
            set
            {
                this.z1Field = value;
                this.RaisePropertyChanged("Z1");
            }
        }

        /// <remarks/>
        public float X2
        {
            get
            {
                return this.x2Field;
            }
            set
            {
                this.x2Field = value;
                this.RaisePropertyChanged("X2");
            }
        }

        /// <remarks/>
        public float Y2
        {
            get
            {
                return this.y2Field;
            }
            set
            {
                this.y2Field = value;
                this.RaisePropertyChanged("Y2");
            }
        }

        /// <remarks/>
        public float Z2
        {
            get
            {
                return this.z2Field;
            }
            set
            {
                this.z2Field = value;
                this.RaisePropertyChanged("Z2");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return true; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TAperture ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TStairway ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TFlight ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TPlatform ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TBox ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TRoom ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TFloor ) )]
    [System.Xml.Serialization.XmlIncludeAttribute( typeof( TBuilding ) )]
    [System.CodeDom.Compiler.GeneratedCodeAttribute( "xsd", "4.0.30319.33440" )]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute( "code" )]
    public abstract partial class TGeometryItem : object, System.ComponentModel.INotifyPropertyChanged
    {
        private string classNameField;
        private int idField;
        private int typeField;
        private string nameField;
        private bool nameFieldSpecified;

        /// <remarks/>
        public string ClassName
        {
            get
            {
                return this.classNameField;
            }
            set
            {
                this.classNameField = value;
                this.RaisePropertyChanged( "ClassName" );
            }
        }

        /// <remarks/>
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged( "Id" );
            }
        }

        /// <remarks/>
        public int Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged( "Type" );
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged( "Name" );
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NameSpecified
        {
            get
            {
                return this.nameFieldSpecified;
            }
            set
            {
                this.nameFieldSpecified = value;
                this.RaisePropertyChanged( "NameSpecified" );
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public abstract bool CanBeApertureTarget
        {
            get;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged( string propertyName )
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ( ( propertyChanged != null ) )
            {
                propertyChanged( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ArrayOfStairwayGeometry : object, System.ComponentModel.INotifyPropertyChanged
    {

        private TGeometryItem[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Box", typeof(TBox))]
        [System.Xml.Serialization.XmlElementAttribute("Flight", typeof(TFlight))]
        [System.Xml.Serialization.XmlElementAttribute("Platform", typeof(TPlatform))]
        public TGeometryItem[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TFlight : TGeometryItem
    {
        private float x1Field;

        private float y1Field;

        private float z1Field;

        private float x2Field;

        private float y2Field;

        private float z2Field;

        private float x3Field;

        private float y3Field;

        private float z3Field;

        private float lengthField;

        private float heightField;

        private float widthField;

        private float angleField;

        /// <remarks/>
        public float X1
        {
            get
            {
                return this.x1Field;
            }
            set
            {
                this.x1Field = value;
                this.RaisePropertyChanged("X1");
            }
        }

        /// <remarks/>
        public float Y1
        {
            get
            {
                return this.y1Field;
            }
            set
            {
                this.y1Field = value;
                this.RaisePropertyChanged("Y1");
            }
        }

        /// <remarks/>
        public float Z1
        {
            get
            {
                return this.z1Field;
            }
            set
            {
                this.z1Field = value;
                this.RaisePropertyChanged("Z1");
            }
        }

        /// <remarks/>
        public float X2
        {
            get
            {
                return this.x2Field;
            }
            set
            {
                this.x2Field = value;
                this.RaisePropertyChanged("X2");
            }
        }

        /// <remarks/>
        public float Y2
        {
            get
            {
                return this.y2Field;
            }
            set
            {
                this.y2Field = value;
                this.RaisePropertyChanged("Y2");
            }
        }

        /// <remarks/>
        public float Z2
        {
            get
            {
                return this.z2Field;
            }
            set
            {
                this.z2Field = value;
                this.RaisePropertyChanged("Z2");
            }
        }

        /// <remarks/>
        public float X3
        {
            get
            {
                return this.x3Field;
            }
            set
            {
                this.x3Field = value;
                this.RaisePropertyChanged("X3");
            }
        }

        /// <remarks/>
        public float Y3
        {
            get
            {
                return this.y3Field;
            }
            set
            {
                this.y3Field = value;
                this.RaisePropertyChanged("Y3");
            }
        }

        /// <remarks/>
        public float Z3
        {
            get
            {
                return this.z3Field;
            }
            set
            {
                this.z3Field = value;
                this.RaisePropertyChanged("Z3");
            }
        }

        /// <remarks/>
        public float Length
        {
            get
            {
                return this.lengthField;
            }
            set
            {
                this.lengthField = value;
                this.RaisePropertyChanged("Length");
            }
        }

        /// <remarks/>
        public float Height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
                this.RaisePropertyChanged("Height");
            }
        }

        /// <remarks/>
        public float Width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
                this.RaisePropertyChanged("Width");
            }
        }

        /// <remarks/>
        public float Angle
        {
            get
            {
                return this.angleField;
            }
            set
            {
                this.angleField = value;
                this.RaisePropertyChanged("Angle");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return false; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TPlatform : TGeometryItem
    {
        private float x1Field;
        private float y1Field;
        private float z1Field;
        private float x2Field;
        private float y2Field;
        private float z2Field;

        /// <remarks/>
        public float X1
        {
            get
            {
                return this.x1Field;
            }
            set
            {
                this.x1Field = value;
                this.RaisePropertyChanged("X1");
            }
        }

        /// <remarks/>
        public float Y1
        {
            get
            {
                return this.y1Field;
            }
            set
            {
                this.y1Field = value;
                this.RaisePropertyChanged("Y1");
            }
        }

        /// <remarks/>
        public float Z1
        {
            get
            {
                return this.z1Field;
            }
            set
            {
                this.z1Field = value;
                this.RaisePropertyChanged("Z1");
            }
        }

        /// <remarks/>
        public float X2
        {
            get
            {
                return this.x2Field;
            }
            set
            {
                this.x2Field = value;
                this.RaisePropertyChanged("X2");
            }
        }

        /// <remarks/>
        public float Y2
        {
            get
            {
                return this.y2Field;
            }
            set
            {
                this.y2Field = value;
                this.RaisePropertyChanged("Y2");
            }
        }

        /// <remarks/>
        public float Z2
        {
            get
            {
                return this.z2Field;
            }
            set
            {
                this.z2Field = value;
                this.RaisePropertyChanged("Z2");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return true; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TStairway : TGeometryItem
    {
        private ArrayOfStairwayGeometry geometryField;

        /// <remarks/>
        public ArrayOfStairwayGeometry Geometry
        {
            get
            {
                return this.geometryField;
            }
            set
            {
                this.geometryField = value;
                this.RaisePropertyChanged("Geometry");
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return false; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TAperture : TGeometryItem
    {
        private float x1Field;

        private bool x1FieldSpecified;

        private float y1Field;

        private bool y1FieldSpecified;

        private float z1Field;

        private bool z1FieldSpecified;

        private float x2Field;

        private bool x2FieldSpecified;

        private float y2Field;

        private bool y2FieldSpecified;

        private float z2Field;

        private bool z2FieldSpecified;

        private int boxId1Field;

        private bool boxId1FieldSpecified;

        private int boxId2Field;

        private bool boxId2FieldSpecified;

        private int lockField;

        private bool lockFieldSpecified;

        private int closerField;

        private bool closerFieldSpecified;

        private int antiFireField;

        private bool antiFireFieldSpecified;

        private float angleField;

        private bool angleFieldSpecified;

        /// <remarks/>
        public float X1
        {
            get
            {
                return this.x1Field;
            }
            set
            {
                this.x1Field = value;
                this.RaisePropertyChanged("X1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool X1Specified
        {
            get
            {
                return this.x1FieldSpecified;
            }
            set
            {
                this.x1FieldSpecified = value;
                this.RaisePropertyChanged("X1Specified");
            }
        }

        /// <remarks/>
        public float Y1
        {
            get
            {
                return this.y1Field;
            }
            set
            {
                this.y1Field = value;
                this.RaisePropertyChanged("Y1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Y1Specified
        {
            get
            {
                return this.y1FieldSpecified;
            }
            set
            {
                this.y1FieldSpecified = value;
                this.RaisePropertyChanged("Y1Specified");
            }
        }

        /// <remarks/>
        public float Z1
        {
            get
            {
                return this.z1Field;
            }
            set
            {
                this.z1Field = value;
                this.RaisePropertyChanged("Z1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Z1Specified
        {
            get
            {
                return this.z1FieldSpecified;
            }
            set
            {
                this.z1FieldSpecified = value;
                this.RaisePropertyChanged("Z1Specified");
            }
        }

        /// <remarks/>
        public float X2
        {
            get
            {
                return this.x2Field;
            }
            set
            {
                this.x2Field = value;
                this.RaisePropertyChanged("X2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool X2Specified
        {
            get
            {
                return this.x2FieldSpecified;
            }
            set
            {
                this.x2FieldSpecified = value;
                this.RaisePropertyChanged("X2Specified");
            }
        }

        /// <remarks/>
        public float Y2
        {
            get
            {
                return this.y2Field;
            }
            set
            {
                this.y2Field = value;
                this.RaisePropertyChanged("Y2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Y2Specified
        {
            get
            {
                return this.y2FieldSpecified;
            }
            set
            {
                this.y2FieldSpecified = value;
                this.RaisePropertyChanged("Y2Specified");
            }
        }

        /// <remarks/>
        public float Z2
        {
            get
            {
                return this.z2Field;
            }
            set
            {
                this.z2Field = value;
                this.RaisePropertyChanged("Z2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Z2Specified
        {
            get
            {
                return this.z2FieldSpecified;
            }
            set
            {
                this.z2FieldSpecified = value;
                this.RaisePropertyChanged("Z2Specified");
            }
        }

        /// <remarks/>
        public int BoxId1
        {
            get
            {
                return this.boxId1Field;
            }
            set
            {
                this.boxId1Field = value;
                this.RaisePropertyChanged("BoxId1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BoxId1Specified
        {
            get
            {
                return this.boxId1FieldSpecified;
            }
            set
            {
                this.boxId1FieldSpecified = value;
                this.RaisePropertyChanged("BoxId1Specified");
            }
        }

        /// <remarks/>
        public int BoxId2
        {
            get
            {
                return this.boxId2Field;
            }
            set
            {
                this.boxId2Field = value;
                this.RaisePropertyChanged("BoxId2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BoxId2Specified
        {
            get
            {
                return this.boxId2FieldSpecified;
            }
            set
            {
                this.boxId2FieldSpecified = value;
                this.RaisePropertyChanged("BoxId2Specified");
            }
        }

        /// <remarks/>
        public int Lock
        {
            get
            {
                return this.lockField;
            }
            set
            {
                this.lockField = value;
                this.RaisePropertyChanged( "Lock" );
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockSpecified
        {
            get
            {
                return this.lockFieldSpecified;
            }
            set
            {
                this.lockFieldSpecified = value;
                this.RaisePropertyChanged( "LockSpecified" );
            }
        }

        /// <remarks/>
        public int Closer
        {
            get
            {
                return this.closerField;
            }
            set
            {
                this.closerField = value;
                this.RaisePropertyChanged( "Closer" );
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CloserSpecified
        {
            get
            {
                return this.closerFieldSpecified;
            }
            set
            {
                this.closerFieldSpecified = value;
                this.RaisePropertyChanged( "CloserSpecified" );
            }
        }

        /// <remarks/>
        public int AntiFire
        {
            get
            {
                return this.antiFireField;
            }
            set
            {
                this.antiFireField = value;
                this.RaisePropertyChanged( "AntiFire" );
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AntiFireSpecified
        {
            get
            {
                return this.antiFireFieldSpecified;
            }
            set
            {
                this.antiFireFieldSpecified = value;
                this.RaisePropertyChanged( "AntiFireSpecified" );
            }
        }

        /// <remarks/>
        public float Angle
        {
            get
            {
                return this.angleField;
            }
            set
            {
                this.angleField = value;
                this.RaisePropertyChanged( "Angle" );
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AngleSpecified
        {
            get
            {
                return this.angleFieldSpecified;
            }
            set
            {
                this.angleFieldSpecified = value;
                this.RaisePropertyChanged( "AngleSpecified" );
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public override bool CanBeApertureTarget
        {
            get { return false; }
        }
    }
}
