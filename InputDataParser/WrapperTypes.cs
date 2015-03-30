/* WrapperTypes.cs - Private wrapper definitions for XSD-generated data types;
 * they are needed to increase abstraction level, i.e. have ability to regenerate XSD code
 * 
 * Copyright (C) 2014 Alexander Kamyshnikov
 *
 * This file is part of CrowdSimulator.
 *
 * CrowdSimulator is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * CrowdSimulator is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using SigmaDC.Interfaces;
using SigmaDC.Common.MathEx;
using SigmaDC.Common.Drawing2D;

namespace SigmaDC.Types
{
    public class HumanWrapper : IBaseObject, IVisualisable
    {
        PeopleTypes.TMan m_human;
        float m_diameter;

        public HumanWrapper( PeopleTypes.TMan human )
        {
            m_human = human;
            m_diameter = 2 * ( float )Math.Sqrt( m_human.Size / ( float )Math.PI );
        }

        public int Id
        {
            get { return m_human.Id; }
        }

        // NOTE: "Name" property is absent yet on XML TMan type
        public string Name
        {
            get { return string.Empty; }
        }

        public int Type
        {
            get { return m_human.Type; }
        }

        public Point3F Center
        {
            get { return new Point3F( m_human.px, m_human.py, m_human.pz ); }
        }

        public float Diameter
        {
            get { return m_diameter; }
        }

        public int ExitId
        {
            get { return m_human.ExitId; }
        }

        public int MobilityGroup
        {
            get { return m_human.Mobility; }
        }

        public int AgeGroup
        {
            get { return m_human.Age; }
        }

        public int EmotionState
        {
            get { return m_human.EmoState; }
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            switch ( Type )
            {
                // TODO: check the difference with TMan type 1 and type 2
                case 0:
                case 1:
                {
                    using ( var brownBrush = new SolidBrush( Color.Red ) )
                    {
                        // S = Pi*(R^2), R > 0 ==> R=sqrt(S/Pi)
                        var radius = (float)Math.Sqrt( m_human.Size / Math.PI );
                        var extent = new RectangleF(
                            new PointF( m_human.px - radius, m_human.py - radius ),
                            new SizeF( 2 * radius, 2 * radius ) );
                        g.FillEllipse( brownBrush, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                default: throw new NotImplementedException();
            }
        }
    }

    // --------------------------------------------------------------------------------------------
    [System.ComponentModel.DefaultProperty( "Id" )]
    public abstract class CashableExtentOwner : IExtentOwner
    {
        private RectangleF m_extent;

        protected abstract RectangleF GetExtents();

        [System.ComponentModel.Category( "Placement" ),
        System.ComponentModel.Description( "The 2D extent of the geometry object" )]
        public RectangleF Extents
        {
            get
            {
                if ( !m_extent.IsEmpty )
                    return m_extent;

                m_extent = GetExtents();
                return m_extent;
            }
        }
    }

    [System.ComponentModel.DefaultProperty( "Id" )]
    public abstract class CashableCuboid : CashableExtentOwner, ICuboid
    {
        private Point3F m_nearLeft;
        private Point3F m_farRight;

        [System.ComponentModel.Category( "Placement" )]
        public Point3F NearLeft
        {
            get { return m_nearLeft; }
            set { m_nearLeft = value; }
        }

        [System.ComponentModel.Category( "Placement" )]
        public Point3F FarRight
        {
            get { return m_farRight; }
            set { m_farRight = value; }
        }
    }

    [System.ComponentModel.DefaultProperty( "Id" )]
    public abstract class GeometryItemWrapper : CashableCuboid, IBaseObject
    {
        protected GeometryTypes.TGeometryItem m_geometryItem = null;

        protected GeometryItemWrapper( GeometryTypes.TGeometryItem geometryItem )
        {
            m_geometryItem = geometryItem;
        }

        public int Id
        {
            get { return m_geometryItem.Id; }
        }

        public string Name
        {
            get { return m_geometryItem.Name; }
        }

        public int Type
        {
            get { return m_geometryItem.Type; }
        }

        public bool CanBeApertureTarget
        {
            get { return m_geometryItem.CanBeApertureTarget; }
        }
    }

    //---------------------------------------------------------------------------------------------

    public class ApertureWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TAperture m_aperture = null;

        public ApertureWrapper( GeometryTypes.TAperture aperture )
            : base( aperture )
        {
            m_aperture = aperture;

            NearLeft = new Point3F( aperture.X1, aperture.Y1, aperture.Z1 );
            FarRight = new Point3F( aperture.X2, aperture.Y2, aperture.Z2 );
        }

        public int BoxId1
        {
            get { return m_aperture.BoxId1; }
        }

        public int BoxId2
        {
            get { return m_aperture.BoxId2; }
        }

        public void MergeProperties( ApertureTypes.TAperture aperture )
        {
            if ( aperture.x1Specified ) m_aperture.X1 = aperture.x1;
            if ( aperture.y1Specified ) m_aperture.Y1 = aperture.y1;
            if ( aperture.z1Specified ) m_aperture.Z1 = aperture.z1;

            if ( aperture.x2Specified ) m_aperture.X2 = aperture.x2;
            if ( aperture.y2Specified ) m_aperture.Y2 = aperture.y2;
            if ( aperture.z2Specified ) m_aperture.Z2 = aperture.z2;

            if ( aperture.BoxId1Specified ) m_aperture.BoxId1 = aperture.BoxId1;
            if ( aperture.BoxId2Specified ) m_aperture.BoxId2 = aperture.BoxId2;

            if ( aperture.LockSpecified ) m_aperture.Lock = aperture.Lock;
            if ( aperture.CloserSpecified ) m_aperture.Closer = aperture.Closer;
            if ( aperture.AntiFireSpecified ) m_aperture.AntiFire = aperture.AntiFire;
            if ( aperture.AngleSpecified ) m_aperture.Angle = aperture.Angle;
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            var extent = Extents;

            // NOTE: aperture have only one valid size: width or height; another is null
            if ( !MathUtils.NearlyZero( extent.Width ) && !MathUtils.NearlyZero( extent.Height ) )
                throw new InvalidOperationException( "Aperture is a flat structure and can have only width or height in one time" );

            switch ( m_aperture.Type )
            {
                case 0: // Inner door
                {
                    using ( var grayPen = new Pen( Color.Gray, 3.0f / g.DpiX ) )
                    {
                        grayPen.StartCap = LineCap.SquareAnchor;
                        grayPen.EndCap = LineCap.SquareAnchor;
                        g.DrawLine( grayPen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }
                    break;
                }
                case 1: // Window
                {
                    using ( var aquaPen = new Pen( Color.Aqua, 1.0f / g.DpiX ) )
                    {
                        g.DrawLine( aquaPen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }
                    break;
                }
                case 2: // Fake ("clue")
                {
                    using ( var pinkPen = new Pen( Color.Pink, 1.0f / g.DpiX ) )
                    {
                        g.DrawLine( pinkPen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }

                    // TODO: check the clue aperture data correctness in the KinderGarten file
                   /* if ( m_aperture.BoxId1 == m_aperture.BoxId2 )
                    {
                        using ( var whitePen = new Pen( Color.Lime, 1.0f / g.DpiX ) )
                        {
                            g.DrawLine( whitePen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                        }
                    }
                    else
                    {
                        using ( var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX ) )
                        {
                            g.DrawLine( brownPen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                        }
                    }*/
                    break;
                }
                case 3: // Exit from building door
                {
                    using ( var pen = new Pen( Color.Black, 3.0f / g.DpiX ) )
                    {
                        pen.StartCap = LineCap.SquareAnchor;
                        pen.EndCap = LineCap.SquareAnchor;
                        g.DrawLine( pen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TBox type" );
            }
        }

        protected override RectangleF GetExtents()
        {
            float x1 = m_aperture.X1;
            float y1 = m_aperture.Y1;
            float x2 = m_aperture.X2;
            float y2 = m_aperture.Y2;

            float w = x2 - x1;
            float h = y2 - y1;
            if ( w < 0 ) w *= -1.0f;
            if ( h < 0 ) h *= -1.0f;

            return new RectangleF( x1, y1, w, h );
        }
    }

    public class FurnitureWrapper : CashableCuboid, IBaseObject, IVisualisable
    {
        FurnitureTypes.TFurnitureItem m_furniture = null;

        public FurnitureWrapper( FurnitureTypes.TFurnitureItem furniture )
        {
            m_furniture = furniture;

            NearLeft = new Point3F( m_furniture.X, m_furniture.Y, m_furniture.Z );
            FarRight = new Point3F( m_furniture.X + m_furniture.Width,
                m_furniture.Y + m_furniture.Length,
                m_furniture.Z + m_furniture.Height );
        }

        [System.ComponentModel.Category( "Base properties" )]
        public int Id
        {
            get { return m_furniture.Id; }
        }

        [System.ComponentModel.Category( "Base properties" )]
        public string Name
        {
            get { return m_furniture.Name; }
        }

        [System.ComponentModel.Category( "Base properties" )]
        public int Type
        {
            get { return m_furniture.Type; }
        }

        [System.ComponentModel.Category( "Furniture" )]
        public float Angle
        {
            get { return m_furniture.Angle; }
        }

        [System.ComponentModel.Category( "Furniture" )]
        public int SeatNumber
        {
            get { return m_furniture.Set; }
        }

        protected override RectangleF GetExtents()
        {
            float x1 = m_furniture.X;
            float y1 = m_furniture.Y;
            float x2 = m_furniture.X + m_furniture.Width;
            float y2 = m_furniture.Y + m_furniture.Length;

            float w = x2 - x1;
            float h = y2 - y1;

            if ( w <= 0 || h <= 0 ) System.Diagnostics.Debugger.Break();
            if ( w < 0 || MathUtils.NearlyZero( w ) || h < 0 || MathUtils.NearlyZero( h ) )
                throw new InvalidOperationException( "Furniture could not have null or negative extent width or height" );

            return new RectangleF( x1, y1, w, h );
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            var extent = Extents;

            using (var blackPen = new Pen( Color.Black, 1.0f / g.DpiX ))
            {
            switch ( m_furniture.Type )
            {
                case 1: // Bed
                {
                    using ( var pinkBrush = new HatchBrush( HatchStyle.Divot, Color.Brown, Color.White ) )
                    {
                        g.FillRectangle( pinkBrush, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                case 2: // Table
                {
                    using ( var blackBrush = new HatchBrush( HatchStyle.ZigZag, Color.Brown, Color.White ) )
                    {
                        g.FillRectangle( blackBrush, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                case 3: // Case
                {
                    using ( var greenBrush = new HatchBrush( HatchStyle.Weave, Color.Brown, Color.White ) )
                    {
                        g.FillRectangle( greenBrush, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                case 5: // Geometry stuff
                {
                    using ( var brownBrush = new SolidBrush( Color.Brown ) )
                    {
                        g.FillRectangle( brownBrush, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TFurniture type" );
            }
            g.DrawRectangle( blackPen, extent.X, extent.Y, extent.Width, extent.Height );
            }
        }
    }

    public class FlightWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TFlight m_flight = null;

        public FlightWrapper( GeometryTypes.TFlight flight )
            : base( flight )
        {
            m_flight = flight;

            NearLeft = new Point3F( flight.X1, flight.Y1, flight.Z1 );
            FarRight = new Point3F( flight.X3, flight.Y3, flight.Z3 );
            if ( !MathUtils.NearlyEqual( flight.Z1, flight.Z2 ) ) throw new InvalidOperationException( "Z1 != Z2" );
        }

        [System.ComponentModel.Category( "Flight" )]
        public float Angle
        {
            get { return m_flight.Angle; }
        }

        [System.ComponentModel.Category( "Flight" )]
        public Point3F NearRight
        {
            get { return new Point3F( m_flight.X2, m_flight.Y2, m_flight.Z2 ); }
        }

        protected override RectangleF GetExtents()
        {
            float x1 = Math.Min( m_flight.X1, m_flight.X3 );
            float y1 = Math.Min( m_flight.Y1, m_flight.Y3 );
            float x2 = x1 + m_flight.Width;
            float y2 = y1 + m_flight.Length;

            float w = x2 - x1;
            float h = y2 - y1;

            if ( w <= 0 || h <= 0 ) System.Diagnostics.Debugger.Break();
            if ( w < 0 || MathUtils.NearlyZero( w ) || h < 0 || MathUtils.NearlyZero( h ) )
                throw new InvalidOperationException( "Flight could not have null or negative extent width or height" );

            return new RectangleF( x1, y1, w, h );
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            var extent = Extents;

            using (var blackPen = new Pen( Color.Black, 1.0f / g.DpiX ))
            {
            switch ( m_flight.Type )
            {
                case 0:
                {
                    using ( var pinkBrush = new HatchBrush( HatchStyle.LargeGrid, Color.Brown, Color.White ) )
                    {
                        g.FillRectangle( pinkBrush, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TFlight type" );
            }
            g.DrawRectangle( blackPen, extent.X, extent.Y, extent.Width, extent.Height );
            }
        }
    }

    public class PlatformWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TPlatform m_platform = null;

        public PlatformWrapper( GeometryTypes.TPlatform platform )
            : base( platform )
        {
            m_platform = platform;

            NearLeft = new Point3F( platform.X1, platform.Y1, platform.Z1 );
            FarRight = new Point3F( platform.X2, platform.Y2, platform.Z2 );
        }

        protected override RectangleF GetExtents()
        {
            float x1 = m_platform.X1;
            float y1 = m_platform.Y1;
            float x2 = m_platform.X2;
            float y2 = m_platform.Y2;

            float w = x2 - x1;
            float h = y2 - y1;

            if ( w <= 0 || h <= 0 ) System.Diagnostics.Debugger.Break();
            if ( w < 0 || MathUtils.NearlyZero( w ) || h < 0 || MathUtils.NearlyZero( h ) )
                throw new InvalidOperationException( "Platform could not have null or negative extent width or height" );

            return new RectangleF( x1, y1, w, h );
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            switch ( m_platform.Type )
            {
                case 0:
                {
                    // Draw box rectangle
                    var extent = Extents;
                    using ( var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX ) )
                    {
                        g.DrawRectangle( brownPen, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TPlatform type" );
            }
        }
    }

    public class StairwayWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TStairway m_stairway = null;
        List<GeometryItemWrapper> m_items = null;

        public StairwayWrapper( GeometryTypes.TStairway stairway )
            : base( stairway )
        {
            m_stairway = stairway;

            m_items = new List<GeometryItemWrapper>( stairway.Geometry.Items.Count() );
            for ( int i = 0; i < stairway.Geometry.Items.Count(); ++i )
            {
                // FIXME: ugly non-OOP code :/
                if ( stairway.Geometry.Items[ i ] is GeometryTypes.TBox )
                {
                    GeometryTypes.TBox b = stairway.Geometry.Items[ i ] as GeometryTypes.TBox;
                    m_items.Add( new BoxWrapper( b ) );
                }
                else if ( stairway.Geometry.Items[ i ] is GeometryTypes.TAperture )
                {
                    GeometryTypes.TAperture a = stairway.Geometry.Items[ i ] as GeometryTypes.TAperture;
                    m_items.Add( new ApertureWrapper( a ) );
                }
                else if ( stairway.Geometry.Items[ i ] is GeometryTypes.TFlight )
                {
                    GeometryTypes.TFlight f = stairway.Geometry.Items[ i ] as GeometryTypes.TFlight;
                    m_items.Add( new FlightWrapper( f ) );
                }
                else if ( stairway.Geometry.Items[ i ] is GeometryTypes.TPlatform )
                {
                    GeometryTypes.TPlatform p = stairway.Geometry.Items[ i ] as GeometryTypes.TPlatform;
                    m_items.Add( new PlatformWrapper( p ) );
                }
                else throw new InvalidOperationException( "Unknown TStairway geometry item type" );
            }
        }

        public StairwayWrapper( StairwayWrapper stairway, int start, int count )
            : base( stairway.m_stairway )
        {
            if ( start < 0 || start >= stairway.Geometry.Count ) throw new ArgumentException();
            if ( count < 1 || count > stairway.Geometry.Count ) throw new ArgumentException();

            m_stairway = stairway.m_stairway;
            m_items = stairway.m_items.GetRange( start, count );
        }

        public List<GeometryItemWrapper> Geometry
        {
            get { return m_items; }
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            switch ( m_stairway.Type )
            {
                case 1: // 0 - Default
                {
                    using ( var blackPen = new Pen( Color.Black, 1.0f / g.DpiX ) )
                    {
                        using ( var grayBrush = new HatchBrush( HatchStyle.Horizontal, Color.LightGray, Color.White ) )
                        {
                            g.FillRectangle( grayBrush, Extents.X, Extents.Y, Extents.Width, Extents.Height );
                            g.DrawRectangle( blackPen, Extents.X, Extents.Y, Extents.Width, Extents.Height );
                        }
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TStairway type" );
            }
        }

        protected override RectangleF GetExtents()
        {
            var extent = new RectangleF();
            if ( m_items.Count >= 1 )
            {
                extent = m_items[ 0 ].Extents;
            }

            for ( int i = 1; i < m_items.Count; ++i )
            {
                extent = RectangleF.Union( extent, m_items[ i ].Extents );
            }
            return extent;
        }
    }

    public class BoxWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TBox m_box = null;

        public BoxWrapper( GeometryTypes.TBox box )
            : base( box )
        {
            m_box = box;

            NearLeft = new Point3F( m_box.X1, m_box.Y1, m_box.Z1 );
            FarRight = new Point3F( m_box.X2, m_box.Y2, m_box.Z2 );
        }

        protected override RectangleF GetExtents()
        {
            float x1 = m_box.X1;
            float y1 = m_box.Y1;
            float x2 = m_box.X2;
            float y2 = m_box.Y2;

            float w = x2 - x1;
            float h = y2 - y1;
            if ( w < 0 || MathUtils.NearlyZero( w ) || h < 0 || MathUtils.NearlyZero( h ) )
                throw new InvalidOperationException( "Furniture could not have null or negative extent width or height" );

            return new RectangleF( x1, y1, w, h );
        }

        public float Z1
        {
            get { return m_box.Z1; }
        }

        public float Z2
        {
            get { return m_box.Z2; }
        }

        public bool LimePen
        {
            get; set;
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            switch ( m_box.Type )
            {
                case 0:
                case 1: // FIXME: What means this type?
                {
                    // Draw box rectangle
                    var extent = Extents;
                    using ( var brownPen = new Pen( LimePen ? Color.Lime : Color.Brown, 1.0f / g.DpiX ) )
                    {
                        g.DrawRectangle( brownPen, extent.X, extent.Y, extent.Width, extent.Height );
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TBox type" );
            }
        }
    }

    public class RoomWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TRoom m_room = null;
        List<BoxWrapper> m_boxes = null;

        public RoomWrapper( GeometryTypes.TRoom room )
            : base( room )
        {
            m_room = room;

            m_boxes = new List<BoxWrapper>( room.Geometry.Count() );
            for ( int i = 0; i < room.Geometry.Count(); ++i )
            {
                m_boxes.Add( new BoxWrapper( room.Geometry[ i ] ) );
            }
        }

        public List<BoxWrapper> Boxes
        {
            get { return m_boxes; }
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            switch ( m_room.Type )
            {
                case 0: // Room itself
                {
                    foreach ( var box in m_boxes ) box.Draw( g );
                    break;
                }
                case 1: // Corridor
                {
                    foreach ( var box in m_boxes )
                    {
                        box.LimePen = true;
                        box.Draw( g );
                    }

                    using ( var grayBrush = new HatchBrush( HatchStyle.DottedGrid, Color.LightGray, Color.White ) )
                    {
                        g.FillRectangle( grayBrush,Extents.X, Extents.Y, Extents.Width, Extents.Height );
                    }
                    break;
                }
                default: throw new InvalidOperationException( "Invalid TRoom type" );
            }
        }

        protected override RectangleF GetExtents()
        {
            RectangleF extent = new RectangleF();
            if ( m_boxes.Count >= 1 )
            {
                extent = m_boxes[ 0 ].Extents;
            }

            for ( int i = 1; i < m_boxes.Count; ++i )
            {
                extent = RectangleF.Union( extent, m_boxes[ i ].Extents );
            }

            return extent;
        }
    }

    public class FloorWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TFloor m_floor = null;
        List<RoomWrapper> m_rooms = null;
        List<ApertureWrapper> m_apertures = null;
        List<FurnitureWrapper> m_furniture = null;
        List<StairwayWrapper> m_stairways = null;
        List<HumanWrapper> m_people = null;
        bool m_drawFurniture = true;
        bool m_drawPeople = true;

        public FloorWrapper( GeometryTypes.TFloor floor )
            : base( floor )
        {
            m_floor = floor;

            m_rooms = new List<RoomWrapper>( floor.RoomList.Count() );
            for ( int i = 0; i < floor.RoomList.Count(); ++i )
                m_rooms.Add( new RoomWrapper( floor.RoomList[ i ] ) );

            m_apertures = new List<ApertureWrapper>( floor.ApertureList.Count() );
            for ( int i = 0; i < floor.ApertureList.Count(); ++i )
                m_apertures.Add( new ApertureWrapper( floor.ApertureList[ i ] ) );

            m_stairways = new List<StairwayWrapper>();
        }

        public void LoadFurniture( FurnitureTypes.TFloor furnFloor )
        {
            m_furniture = new List<FurnitureWrapper>( furnFloor.Furniture.Count() );
            for ( int i = 0; i < furnFloor.Furniture.Count(); ++i )
            {
                m_furniture.Add( new FurnitureWrapper( furnFloor.Furniture[ i ] ) );
            }
        }

        public void LoadPeople( PeopleTypes.TFloor peopleFloor )
        {
            m_people = new List<HumanWrapper>( peopleFloor.People.Count() );
            foreach ( var human in peopleFloor.People ) m_people.Add( new HumanWrapper( human ) );
        }

        public void AddStairway( StairwayWrapper sw )
        {
            m_stairways.Add( sw );
        }

        public int Number
        {
            get { return m_floor.Number; }
        }

        public IEnumerable<BoxWrapper> Geometry
        {
            get
            {
                List<BoxWrapper> boxes = new List<BoxWrapper>();
                foreach( var r in m_rooms )
                {
                    foreach( var b in r.Boxes )
                    {
                        boxes.Add( b );
                    }
                }
                return boxes;
            }
        }

        public IEnumerable<ApertureWrapper> Apertures
        {
            get { return m_apertures; }
        }

        public IEnumerable<StairwayWrapper> Stairways
        {
            get { return m_stairways; }
            set { m_stairways = value.ToList(); }
        }

        public IEnumerable<FurnitureWrapper> Furniture
        {
            get { return m_furniture; }
        }

        public IEnumerable<HumanWrapper> People
        {
            get { return m_people; }
        }

        public ApertureWrapper FindAperture( int id )
        {
            return Apertures.Where( x => x.Id == id ).FirstOrDefault();
        }

        public Dictionary<RectangleF, BoxWrapper> GetBoxMap()
        {
            var boxes = new Dictionary<RectangleF, BoxWrapper>();
            foreach ( var room in m_rooms )
            {
                foreach ( var box in room.Boxes )
                {
                    boxes.Add( box.Extents, box );
                }
            }
            return boxes;
        }

        public IEnumerable<ApertureWrapper> Doors
        {
            get
            {
                return m_apertures.Where( x => x.Type == 0 );
            }
        }

        public IEnumerable<ApertureWrapper> Windows
        {
            get
            {
                return m_apertures.Where( x => x.Type == 1 );
            }
        }

        public IEnumerable<ApertureWrapper> FakeApertures
        {
            get
            {
                return m_apertures.Where( x => x.Type == 2 );
            }
        }

        public IEnumerable<ApertureWrapper> Exits
        {
            get
            {
                return m_apertures.Where( x => x.Type == 3 );
            }
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            if ( options.ContainsKey( "drawFurniture" ) ) m_drawFurniture = ( bool )options[ "drawFurniture" ];
            if ( options.ContainsKey( "drawPeople" ) ) m_drawPeople = ( bool )options[ "drawPeople" ];
        }

        public void Draw( Graphics g )
        {
            switch ( m_floor.Type )
            {
                case 0:
                {
                    foreach ( var room in m_rooms ) room.Draw( g );
                    foreach ( var aperture in m_apertures ) aperture.Draw( g );
                    foreach ( var stairway in m_stairways ) stairway.Draw( g );
                    if ( m_drawFurniture )
                    {
                        foreach ( var furnitureItem in m_furniture ) furnitureItem.Draw( g );
                    }
                    if ( m_drawPeople )
                    {
                        foreach ( var human in m_people ) human.Draw( g );
                    }

                    // Draw box subscriptions
                    foreach ( var box in Geometry )
                    {
                        // Draw box properties as text over it
                        var extent = box.Extents;
                        var extentCenter = new PointF( ( extent.Left + extent.Right ) / 2, ( extent.Bottom + extent.Top ) / 2 );
                        DrawingUtils.DrawText( g, "ID: " + box.Id + "\n" + "Height: " + ( box.Z2 - box.Z1 ), extentCenter, extent );
                    }

                    break;
                }
                default: throw new InvalidOperationException( "Invalid TFloor type" );
            }
        }

        protected override RectangleF GetExtents()
        {
            RectangleF extent = new RectangleF();
            if ( m_rooms.Count >= 1 )
            {
                extent = m_rooms[ 0 ].Extents;
            }

            for ( int i = 1; i < m_rooms.Count; ++i )
            {
                extent = RectangleF.Union( extent, m_rooms[ i ].Extents );
            }

            return extent;
        }
    }

    public class BuildingWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TBuilding m_building = null;
        int m_floorNumber = -1;
        List<FloorWrapper> m_floors = null;
        List<StairwayWrapper> m_stairways = null;

        static readonly string GEOMETRY_FILE_NAME = @"geometry.xml";
        static readonly string APERTURES_FILE_NAME = @"apertures.xml";
        static readonly string FURNITURE_FILE_NAME = @"furniture.xml";
        static readonly string PEOPLE_FILE_NAME = @"people.xml";

        #region Floor sorter class (by numbers of floor)
        // FIXME: inherit all floors from one class to remove switch(type) ugly construction
        class FloorComparer : IComparer
        {
            public int Compare( object x, object y )
            {
                if ( x == null || y == null )
                    return 1;
                if ( x.GetType() != y.GetType() )
                    throw new ArgumentException( "Floor types are different" );

                if ( x is GeometryTypes.TFloor )
                {
                    var xx = x as GeometryTypes.TFloor;
                    var yy = y as GeometryTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a Geometry TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else if ( x is ApertureTypes.TFloor )
                {
                    var xx = x as ApertureTypes.TFloor;
                    var yy = y as ApertureTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a Apertures TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else if ( x is FurnitureTypes.TFloor )
                {
                    var xx = x as FurnitureTypes.TFloor;
                    var yy = y as FurnitureTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a Furniture TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else if ( x is PeopleTypes.TFloor )
                {
                    var xx = x as PeopleTypes.TFloor;
                    var yy = y as PeopleTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a People TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                
                throw new InvalidOperationException( "Invalid Floor type" );
            }
        }

        void SortFloors( Array arr )
        {
            Array.Sort( arr, new FloorComparer() );
        }
        #endregion

        public BuildingWrapper( string dataDir )
            : base( null )
        {
            if ( !Directory.Exists( dataDir ) ) throw new DirectoryNotFoundException( "Building data directory was not found" );

            var geometryFileNameAbs = dataDir + Path.DirectorySeparatorChar + GEOMETRY_FILE_NAME;
            var aperturesFileNameAbs = dataDir + Path.DirectorySeparatorChar + APERTURES_FILE_NAME;
            var furnitureFileNameAbs = dataDir + Path.DirectorySeparatorChar + FURNITURE_FILE_NAME;
            var peopleFileNameAbs = dataDir + Path.DirectorySeparatorChar + PEOPLE_FILE_NAME;

            var parser = new InputDataParser.Parser();
            LoadGeometry( parser, geometryFileNameAbs );
            LoadApertures( parser, aperturesFileNameAbs );
            LoadFurniture( parser, furnitureFileNameAbs );
            LoadPeople( parser, peopleFileNameAbs );
        }

        public bool LoadGeometry( InputDataParser.Parser parser, string fileName )
        {
            m_building = parser.LoadGeometryXMLRoot( fileName );
            if ( m_building.FloorList.Count() == 0 )
            {
                throw new InvalidOperationException( "Building has no floors" );
            }

            SortFloors( m_building.FloorList );

            // Load floors
            m_floorNumber = m_building.FloorList.Count() > 0 ? m_building.FloorList[ 0 ].Number : -1;
            m_floors = new List<FloorWrapper>( FloorCount );
            for ( int i = 0; i < FloorCount; ++i )
            {
                m_floors.Add( new FloorWrapper( m_building.FloorList[ i ] ) );
            }

            // Load stairways
            // Assign each stairway to floors: one half to i-th floor and another to the i+1-th
            // Detection method: each floor must have apertures opening/closing to the some stairway items,
            // currently boxes and platforms
            m_stairways = new List<StairwayWrapper>( FloorCount );
            for ( int i = 0; i < m_building.StairwayList.Count(); ++i )
            {
                m_stairways.Add( new StairwayWrapper( m_building.StairwayList[ i ] ) );

                if ( m_stairways[ i ].Geometry.Count % 2 != 0 ) throw new NotImplementedException( "Stairway with odd number of items" );
                int halfOfItemCount = m_stairways[ i ].Geometry.Count / 2;

                foreach ( var gi in m_stairways[ i ].Geometry )
                {
                    if ( !gi.CanBeApertureTarget ) continue;

                    // FIXME: ensure that one stairway has added only one time to each floor; i.e. no duplicates are allowed
                    foreach ( var floor in Floors )
                    {
                        foreach ( var aper in floor.Apertures )
                        {
                            if ( ( aper.BoxId1 == gi.Id ) && ( aper.BoxId2 == -1 ) )
                            {
                                var sw = new StairwayWrapper( m_stairways[ i ], 0, halfOfItemCount );
                                floor.AddStairway( sw );
                            }

                            if ( aper.BoxId2 == gi.Id )
                            {
                                var sw = new StairwayWrapper( m_stairways[ i ], halfOfItemCount, halfOfItemCount );
                                floor.AddStairway( sw );
                            }
                        }
                    }
                }
            }
            m_stairways = null;

            m_geometryItem = m_building;
            return true;
        }

        public bool LoadApertures( InputDataParser.Parser parser, string fileName )
        {
            var building = parser.LoadAperturesXMLRoot( fileName );
            if ( building.FloorList.Count() == 0 )
            {
                throw new InvalidOperationException( "Building has no floors" );
            }
            if ( building.FloorList.Count() != m_floors.Count ) throw new InvalidOperationException( "Invalid aperture file" );

            SortFloors( building.FloorList );

            foreach ( var floor in building.FloorList )
            {
                FloorWrapper fw = GetSpecifiedFloor( floor.Number );
                for ( int i = 0; i < floor.ApertureList.Count(); ++i )
                {
                    // Search for aperture with the same ID in the geometry file
                    var aw = fw.FindAperture( floor.ApertureList[ i ].Id );
                    if ( aw != null )
                        aw.MergeProperties( floor.ApertureList[ i ] );
                }
            }

            return true;
        }

        public bool LoadFurniture( InputDataParser.Parser parser, string fileName )
        {
            var building = parser.LoadFurnitureXMLRoot( fileName );
            if ( building.FloorList.Count() == 0 )
            {
                throw new InvalidOperationException( "Building has no floors" );
            }
            if ( building.FloorList.Count() != m_floors.Count ) throw new InvalidOperationException( "Invalid furniture file" );

            SortFloors( building.FloorList );

            foreach ( var floor in building.FloorList )
            {
                FloorWrapper fw = GetSpecifiedFloor( floor.Number );
                fw.LoadFurniture( floor );
            }
            return true;
        }

        public bool LoadPeople( InputDataParser.Parser parser, string fileName )
        {
            var building = parser.LoadPeopleXMLRoot( fileName );
            // TODO: try to move such error checks to the aspect
            if ( building.FloorList.Count() == 0 )
            {
                throw new InvalidOperationException( "Building has no floors" );
            }
            if ( building.FloorList.Count() != m_floors.Count ) throw new InvalidOperationException( "Invalid people file" );

            SortFloors( building.FloorList );

            foreach ( var floor in building.FloorList )
            {
                FloorWrapper fw = GetSpecifiedFloor( floor.Number );
                fw.LoadPeople( floor );
            }
            return true;
        }

        public string GetGeometryFileName()
        {
            return GEOMETRY_FILE_NAME;
        }

        public string GetAperturesFileName()
        {
            return APERTURES_FILE_NAME;
        }

        public string GetFurnitureFileName()
        {
            return FURNITURE_FILE_NAME;
        }

        public string GetPeopleFileName()
        {
            return PEOPLE_FILE_NAME;
        }

        public List<FloorWrapper> Floors
        {
            get { return m_floors; }
        }

        public int CurrentFloorNumber
        {
            get { return m_floorNumber; }
            set
            {
                if ( value >= FirstFloorNumber && value <= FloorCount )
                    m_floorNumber = value;
                else
                    throw new ArgumentException( "Floor number must be in range " + FirstFloorNumber + " - " + FloorCount );
            }
        }

        public int FirstFloorNumber
        {
            get
            {
                return m_building.FloorList[ 0 ].Number;
            }
        }

        public int FloorCount
        {
            get
            {
                // FIXME: think about negative floor numbers
                return m_building.FloorList.Count();
            }
        }

        FloorWrapper GetSpecifiedFloor( int number )
        {
            foreach ( var floor in m_floors )
            {
                if ( floor.Number == number )
                    return floor;
            }

            return null;
        }

        public FloorWrapper CurrentFloor
        {
            get { return GetSpecifiedFloor( m_floorNumber ); }
        }

        public float MinHumanDiameter
        {
            get
            {
                float minHumanDiameter = float.PositiveInfinity;
                foreach ( var human in CurrentFloor.People )
                {
                    if ( human.Diameter < minHumanDiameter )
                        minHumanDiameter = human.Diameter;
                }
                return minHumanDiameter;
            }
        }

        public void SetDrawOptions( Dictionary<string, object> options )
        {
            CurrentFloor.SetDrawOptions( options );
        }

        public void Draw( Graphics g )
        {
            CurrentFloor.Draw( g );
        }

        protected override RectangleF GetExtents()
        {
            return CurrentFloor.Extents;
        }
    }
}
