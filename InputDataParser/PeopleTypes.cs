﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18449
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 
namespace PeopleTypes
{
    using System.Xml.Serialization;
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Building", Namespace = "", IsNullable = false)]
    public partial class TBuilding
    {

        private string classNameField;

        private int idField;

        private int typeField;

        private string nameField;

        private TFloor[] floorListField;

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Floor")]
        public TFloor[] FloorList
        {
            get
            {
                return this.floorListField;
            }
            set
            {
                this.floorListField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TFloor
    {

        private string classNameField;

        private int idField;

        private int typeField;

        private string nameField;

        private int numberField;

        private TMan[] peopleField;

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
            }
        }

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Man")]
        public TMan[] People
        {
            get
            {
                return this.peopleField;
            }
            set
            {
                this.peopleField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TMan
    {

        private string classNameField;

        private int idField;

        private int typeField;

        private float pxField;

        private float pyField;

        private float pzField;

        private int colorField;

        private int mobilityField;

        private int ageField;

        private float sizeField;

        private int sexField;

        private int emoStateField;

        private int educLevelField;

        private int roleField;

        private int startRoomField;

        private int startTimeField;

        private int isControlField;

        private int exitIdField;

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
            }
        }

        /// <remarks/>
        public float px
        {
            get
            {
                return this.pxField;
            }
            set
            {
                this.pxField = value;
            }
        }

        /// <remarks/>
        public float py
        {
            get
            {
                return this.pyField;
            }
            set
            {
                this.pyField = value;
            }
        }

        /// <remarks/>
        public float pz
        {
            get
            {
                return this.pzField;
            }
            set
            {
                this.pzField = value;
            }
        }

        /// <remarks/>
        public int Color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        public int Mobility
        {
            get
            {
                return this.mobilityField;
            }
            set
            {
                this.mobilityField = value;
            }
        }

        /// <remarks/>
        public int Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }

        /// <remarks/>
        public float Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        public int Sex
        {
            get
            {
                return this.sexField;
            }
            set
            {
                this.sexField = value;
            }
        }

        /// <remarks/>
        public int EmoState
        {
            get
            {
                return this.emoStateField;
            }
            set
            {
                this.emoStateField = value;
            }
        }

        /// <remarks/>
        public int EducLevel
        {
            get
            {
                return this.educLevelField;
            }
            set
            {
                this.educLevelField = value;
            }
        }

        /// <remarks/>
        public int Role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        /// <remarks/>
        public int StartRoom
        {
            get
            {
                return this.startRoomField;
            }
            set
            {
                this.startRoomField = value;
            }
        }

        /// <remarks/>
        public int StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public int IsControl
        {
            get
            {
                return this.isControlField;
            }
            set
            {
                this.isControlField = value;
            }
        }

        /// <remarks/>
        public int ExitId
        {
            get
            {
                return this.exitIdField;
            }
            set
            {
                this.exitIdField = value;
            }
        }
    }
}
