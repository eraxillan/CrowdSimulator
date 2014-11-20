﻿/* WrapperTypes.cs - Private wrapper definitions for XSD-generated data types;
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
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace SigmaDC.Types
{
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
    public abstract class CashableCuboid : CashableExtentOwner
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
    }

    //---------------------------------------------------------------------------------------------

    class ApertureWrapper : GeometryItemWrapper, IVisualisable
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

        public void Draw( Graphics g )
        {
            // FIXME: aperture have only one valid size: width or height; another is null
            var extent = Extents;
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
                default:
                    System.Diagnostics.Debug.Assert( false, "Invalid TBox type" );
                    break;
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

    class FurnitureWrapper : CashableCuboid, IBaseObject, IVisualisable
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
            System.Diagnostics.Debug.Assert( w > 0, "Furniture extent have negative width" );
            System.Diagnostics.Debug.Assert( h > 0, "Furniture extent have negative height" );

            return new RectangleF( x1, y1, w, h );
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
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TFurniture type" );
                    break;
                }
            }
            g.DrawRectangle( blackPen, extent.X, extent.Y, extent.Width, extent.Height );
            }
        }
    }

    class FlightWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TFlight m_flight = null;

        public FlightWrapper( GeometryTypes.TFlight flight )
            : base( flight )
        {
            m_flight = flight;

            NearLeft = new Point3F( flight.X1, flight.Y1, flight.Z1 );
            FarRight = new Point3F( flight.X3, flight.Y3, flight.Z3 );
            System.Diagnostics.Debug.Assert( Math.Abs( flight.Z1 - flight.Z2 ) <= 0.001f );
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
            System.Diagnostics.Debug.Assert( w > 0, "Flight extent have negative width" );
            System.Diagnostics.Debug.Assert( h > 0, "Flight extent have negative height" );

            return new RectangleF( x1, y1, w, h );
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
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TFlight type" );
                    break;
                }
            }
            g.DrawRectangle( blackPen, extent.X, extent.Y, extent.Width, extent.Height );
            }
        }
    }

    class PlatformWrapper : GeometryItemWrapper, IVisualisable
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
            System.Diagnostics.Debug.Assert( w > 0, "Platform extent have negative width" );
            System.Diagnostics.Debug.Assert( h > 0, "Platform extent have negative height" );

            return new RectangleF( x1, y1, w, h );
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
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TPlatform type" );
                    break;
                }
            }
        }
    }

    class StairwayWrapper : GeometryItemWrapper, IVisualisable
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
                else System.Diagnostics.Debug.Assert( false, "Unknown TStairway geometry item type" );
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
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TStairway type" );
                    break;
                }
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

    class BoxWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TBox m_box = null;
        Font m_textFont = null;
        RectangleF m_textRect;

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

            if ( w <= 0 || h <= 0 ) System.Diagnostics.Debugger.Break();
            System.Diagnostics.Debug.Assert( w > 0, "Box extent have negative width" );
            System.Diagnostics.Debug.Assert( h > 0, "Box extent have negative height" );

            return new RectangleF( x1, y1, w, h );
        }

        private bool m_pink = false;
        public bool PinkPen
        {
            get { return m_pink; }
            set { m_pink = value; }
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
                    using ( var brownPen = new Pen( PinkPen ? Color.Pink : Color.Brown, 1.0f / g.DpiX ) )
                    {
                        g.DrawRectangle( brownPen, extent.X, extent.Y, extent.Width, extent.Height );
                    }

                    // Draw box properties as text on it
                    var extentCenter = new PointF( ( extent.Left + extent.Right ) / 2, ( extent.Bottom + extent.Top ) / 2 );
                    DrawText( g, "ID: " + m_box.Id + "\n" + "Height: " + ( m_box.Z2 - m_box.Z1 ), extentCenter, extent );
                    break;
                }
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TBox type" );
                    break;
                }
            }
        }

        private void DrawText( Graphics g, string text, PointF ptStart, RectangleF extent )
        {
            var gs = g.Save();
            // Inverse Y axis again - now it grow down;
            // if we don't do this, text will be drawn inverted
            g.ScaleTransform( 1.0f, -1.0f, MatrixOrder.Prepend );

            if ( m_textFont == null )
            {
                // Find the maximum appropriate text size to fix the extent
                float fontSize = 100.0f;
                Font fnt;
                SizeF textSize;
                do
                {
                    fnt = new Font( "Arial", fontSize / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );
                    textSize = g.MeasureString( text, fnt );
                    m_textRect = new RectangleF( new PointF( ptStart.X - textSize.Width / 2.0f, -ptStart.Y - textSize.Height / 2.0f ), textSize );

                    var textRectInv = new RectangleF( m_textRect.X, -m_textRect.Y, m_textRect.Width, m_textRect.Height );
                    if ( extent.Contains( textRectInv ) )
                        break;

                    fontSize -= 1.0f;
                    if ( fontSize <= 0 )
                    {
                        fontSize = 1.0f;
                        break;
                    }
                } while ( true );

                m_textFont = fnt;
            }

            // Create a StringFormat object with the each line of text, and the block of text centered on the page
            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString( text, m_textFont, Brushes.Black, m_textRect, stringFormat );
            stringFormat.Dispose();

            g.Restore( gs );
        }
    }

    class RoomWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TRoom m_room = null;
        BoxWrapper[] m_boxes = null;

        public RoomWrapper( GeometryTypes.TRoom room )
            : base( room )
        {
            m_room = room;

            m_boxes = new BoxWrapper[ room.Geometry.Count() ];
            for ( int i = 0; i < room.Geometry.Count(); ++i )
            {
                m_boxes[ i ] = new BoxWrapper( room.Geometry[ i ] );
            }
        }

        public BoxWrapper[] Boxes
        {
            get { return m_boxes; }
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
                        box.PinkPen = true;
                        box.Draw( g );
                    }

                    using ( var grayBrush = new HatchBrush( HatchStyle.DottedGrid, Color.LightGray, Color.White ) )
                    {
                        g.FillRectangle( grayBrush,Extents.X, Extents.Y, Extents.Width, Extents.Height );
                    }
                    break;
                }
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TRoom type" );
                    break;
                }
            }
        }

        protected override RectangleF GetExtents()
        {
            RectangleF extent = new RectangleF();
            if ( m_boxes.Count() >= 1 )
            {
                extent = m_boxes[ 0 ].Extents;
            }

            for ( int i = 1; i < m_boxes.Count(); ++i )
            {
                extent = RectangleF.Union( extent, m_boxes[ i ].Extents );
            }

            return extent;
        }
    }

    class FloorWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TFloor m_floor = null;
        RoomWrapper[] m_rooms = null;
        ApertureWrapper[] m_apertures = null;
        FurnitureWrapper[] m_furniture = null;
        List<StairwayWrapper> m_stairways = null;

        // FIXME: implement
        //PeopleWrapper[] m_people = null;

        public FloorWrapper( GeometryTypes.TFloor floor )
            : base( floor )
        {
            m_floor = floor;

            m_rooms = new RoomWrapper[ floor.RoomList.Count() ];
            for ( int i = 0; i < floor.RoomList.Count(); ++i )
            {
                m_rooms[ i ] = new RoomWrapper( floor.RoomList[ i ] );
            }

            m_apertures = new ApertureWrapper[ floor.ApertureList.Count() ];
            for ( int i = 0; i < floor.ApertureList.Count(); ++i )
            {
                m_apertures[ i ] = new ApertureWrapper( floor.ApertureList[ i ] );
            }

            m_stairways = new List<StairwayWrapper>();
        }

        public void LoadFurniture( FurnitureTypes.TFloor furnFloor )
        {
            m_furniture = new FurnitureWrapper[ furnFloor.Furniture.Count() ];
            for ( int i = 0; i < furnFloor.Furniture.Count(); ++i )
            {
                m_furniture[ i ] = new FurnitureWrapper( furnFloor.Furniture[ i ] );
            }
        }

        public int Number
        {
            get { return m_floor.Number; }
        }

        public ApertureWrapper[] Apertures
        {
            get { return m_apertures; }
        }

        public List<StairwayWrapper> Stairways
        {
            get { return m_stairways; }
            set { m_stairways = value; }
        }

        public FurnitureWrapper[] Furniture
        {
            get { return m_furniture; }
        }

        public ApertureWrapper FindAperture( int id )
        {
            foreach ( var aperture in Apertures )
            {
                if ( aperture.Id == id )
                    return aperture;
            }

            return null;
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

        public void Draw( Graphics g )
        {
            switch ( m_floor.Type )
            {
                case 0:
                {
                    // FIXME: draw other stuff
                    foreach ( var furnitureItem in m_furniture ) furnitureItem.Draw( g );
                    foreach ( var room in m_rooms ) room.Draw( g );
                    foreach ( var aperture in m_apertures ) aperture.Draw( g );
                    foreach ( var stairway in m_stairways ) stairway.Draw( g );
                    //foreach ( var human in m_people ) human.Draw( g );
                    break;
                }
                default:
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid TFloor type" );
                    break;
                }
            }
        }

        protected override RectangleF GetExtents()
        {
            RectangleF extent = new RectangleF();
            if ( m_rooms.Count() >= 1 )
            {
                extent = m_rooms[ 0 ].Extents;
            }

            for ( int i = 1; i < m_rooms.Count(); ++i )
            {
                extent = RectangleF.Union( extent, m_rooms[ i ].Extents );
            }

            return extent;
        }
    }

    class BuildingWrapper : GeometryItemWrapper, IVisualisable
    {
        GeometryTypes.TBuilding m_building = null;
        int m_floorNumber = -1;
        FloorWrapper[] m_floors = null;
        StairwayWrapper[] m_stairways = null;

        static readonly string GEOMETRY_FILE_NAME = @"geometry.xml";
        static readonly string APERTURES_FILE_NAME = @"apertures.xml";
        static readonly string FURNITURE_FILE_NAME = @"furniture.xml";
        static readonly string PEOPLE_FILE_NAME = @"people.xml";

        // TODO: implement last error logic
        string m_lastError;

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
                else
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid Floor type" );
                }

                return 1;
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
            System.Diagnostics.Debug.Assert( Directory.Exists( dataDir ) );

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
                m_lastError = "Building has no floors";
                return false;
            }

            SortFloors( m_building.FloorList );

            // Load floors
            m_floorNumber = m_building.FloorList.Count() > 0 ? m_building.FloorList[ 0 ].Number : -1;
            m_floors = new FloorWrapper[ FloorCount ];
            for ( int i = 0; i < m_building.FloorList.Count(); ++i )
            {
                m_floors[ i ] = new FloorWrapper( m_building.FloorList[ i ] );
            }

            // Load stairways
            m_stairways = new StairwayWrapper[ FloorCount ];
            for ( int i = 0; i < m_building.StairwayList.Count(); ++i )
            {
                m_stairways[ i ] = new StairwayWrapper( m_building.StairwayList[ i ] );

                if ( m_stairways[ i ].Geometry.Count % 2 != 0 ) throw new NotImplementedException( "Stairway with odd number of items" );
                int halfOfItemCount = m_stairways[ i ].Geometry.Count / 2;

                foreach ( var gi in m_stairways[ i ].Geometry )
                {
                    // FIXME: replace this ugly OOP-forbidden construction to the virtual function/property
                    if ( !( gi is BoxWrapper ) && !( gi is PlatformWrapper ) ) continue;

                    // FIXME: ensure that one stairway has added only one time to each floor; i.e. no duplicates are allowed
                    foreach ( var floor in Floors )
                    {
                        foreach ( var aper in floor.Apertures )
                        {
                            if ( ( aper.BoxId1 == gi.Id ) && ( aper.BoxId2 == -1 ) )
                            {
                                var sw = new StairwayWrapper( m_stairways[ i ], 0, halfOfItemCount );
                                floor.Stairways.Add( sw );
                            }

                            if ( aper.BoxId2 == gi.Id )
                            {
                                var sw = new StairwayWrapper( m_stairways[ i ], halfOfItemCount, halfOfItemCount );
                                floor.Stairways.Add( sw );
                            }
                        }
                    }
                }
            }

            m_geometryItem = m_building;
            return true;
        }

        public bool LoadApertures( InputDataParser.Parser parser, string fileName )
        {
            var building = parser.LoadAperturesXMLRoot( fileName );
            if ( building.FloorList.Count() == 0 )
            {
                m_lastError = "Building has no floors";
                return false;
            }
            System.Diagnostics.Debug.Assert( building.FloorList.Count() == m_floors.Count() );

            SortFloors( building.FloorList );

            foreach ( var floor in building.FloorList )
            {
                FloorWrapper fw = GetSpecifiedFloor( floor.Number );
                System.Diagnostics.Debug.Assert( fw.Number == floor.Number );

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
                m_lastError = "Building has no floors";
                return false;
            }
            System.Diagnostics.Debug.Assert( building.FloorList.Count() == m_floors.Count() );

            SortFloors( building.FloorList );

            foreach ( var floor in building.FloorList )
            {
                FloorWrapper fw = GetSpecifiedFloor( floor.Number );
                System.Diagnostics.Debug.Assert( fw.Number == floor.Number );

                fw.LoadFurniture( floor );
            }
            return true;
        }

        public bool LoadPeople( InputDataParser.Parser parser, string fileName )
        {
//            throw new NotImplementedException();
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

        public FloorWrapper[] Floors
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
