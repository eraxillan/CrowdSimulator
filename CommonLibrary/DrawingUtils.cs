using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaDC.Common.Drawing2D
{
    public class DrawingUtils
    {
        public static void DrawText( Graphics g, string text, PointF ptStart, RectangleF extent )
        {
            var gs = g.Save();
            // Inverse Y axis again - now it grow down;
            // if we don't do this, text will be drawn inverted
            g.ScaleTransform( 1.0f, -1.0f, MatrixOrder.Prepend );

            // Find the maximum appropriate text size to fix the extent
            float fontSize = 100.0f;
            Font fnt = null;
            RectangleF textRect;
            SizeF textSize;
            do
            {
                fnt = new Font( "Arial", fontSize / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );
                textSize = g.MeasureString( text, fnt );
                textRect = new RectangleF( new PointF( ptStart.X - textSize.Width / 2.0f, -ptStart.Y - textSize.Height / 2.0f ), textSize );

                var textRectInv = new RectangleF( textRect.X, -textRect.Y, textRect.Width, textRect.Height );
                if ( extent.Contains( textRectInv ) )
                    break;

                fontSize -= 1.0f;
                if ( fontSize <= 0 )
                {
                    fontSize = 1.0f;
                    break;
                }
            } while ( true );

            // Create a StringFormat object with the each line of text, and the block of text centered on the page
            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString( text, fnt, Brushes.Black, textRect, stringFormat );
            stringFormat.Dispose();

            g.Restore( gs );
        }
    }
}
