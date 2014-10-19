
public class Point3F
{
    float m_X = float.NaN;
    float m_Y = float.NaN;
    float m_Z = float.NaN;

    public Point3F( float X, float Y, float Z = 0 )
    {
        m_X = X;
        m_Y = Y;
        m_Z = Z;
    }

    public bool IsNull
    {
        get { return ( float.IsNaN( m_X ) || float.IsNaN( m_Y ) || float.IsNaN( m_Z ) ); }
    }

    public float X
    {
        get { return m_X; }
    }

    public float Y
    {
        get { return m_Y; }
    }

    public float Z
    {
        get { return m_Z; }
    }

    public override string ToString()
    {
        if ( IsNull )
            return "<Invalid>";

        string pntString = "{ ";
        pntString += X.ToString( "F3" );
        pntString += "; ";
        pntString += Y.ToString( "F3" );
        pntString += "; ";
        pntString += Z.ToString( "F3" );
        pntString += " }";
        return pntString;
    }
}