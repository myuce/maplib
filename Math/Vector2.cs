using System;

public class Vector2
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public Vector2(float _x, float _y)
    {
        x = _x;
        y = _y;
    }

    public static Vector2 Zero = new Vector2(0f, 0f);

    public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    public static Vector2 operator +(Vector2 lhs, float rhs)
    {
        return new Vector2(lhs.x + rhs, lhs.y + rhs);
    }

    public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    public static Vector2 operator -(Vector2 lhs, float rhs)
    {
        return new Vector2(lhs.x - rhs, lhs.y - rhs);
    }

    public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    public static Vector2 operator *(Vector2 lhs, float rhs)
    {
        return new Vector2(lhs.x * rhs, lhs.y * rhs);
    }

    public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    public static Vector2 operator /(Vector2 lhs, float rhs)
    {
        return new Vector2(lhs.x / rhs, lhs.y / rhs);
    }

    public static bool operator ==(Vector2 lhs, Vector2 rhs)
    {
        return (lhs - rhs).len() <= 1e-5;
    }

    public static bool operator !=(Vector2 lhs, Vector2 rhs)
    {
        return !((lhs - rhs).len() <= 1e-5);
    }

    public override bool Equals(object o)
    {
        Console.WriteLine(o);
        return false;
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public float dot(Vector2 rhs)
    {
        return (x * rhs.x) + (y * rhs.y) + (z * rhs.z);
    }

    public float sqrLen()
    {
        return this.dot(this);
    }

    public float len()
    {
        return MathF.Sqrt(this.sqrLen());
    }

    public Vector2 normalize()
    {
        return this / this.len();
    }

    public Vector2 cross(Vector2 rhs)
    {
        return new Vector2(
            this.y * rhs.z - this.z * rhs.y,
            this.z * rhs.x - this.x * rhs.z
        );
    }

    public float distance(Vector2 rhs)
    {
        return MathF.Sqrt(MathF.Pow(this.x - rhs.x, 2) + MathF.Pow(this.y - rhs.y, 2));
    }

    public Vector2 lerp(Vector2 rhs, float alpha)
    {
        return new Vector2(
            this.x + ((rhs.x - this.x) * alpha),
            this.y + ((rhs.y - this.y) * alpha)
        );
    }

    public Vector2 round(int digits = 3)
    {
        return new Vector2(MathF.Round(x, digits), MathF.Round(y, digits));
    }

    public static Vector2 FromStr(string vec)
    {
        string[] tok = vec.Split(" ");
        return new Vector2(
            float.Parse(tok[0], System.Globalization.CultureInfo.InvariantCulture),
            float.Parse(tok[1], System.Globalization.CultureInfo.InvariantCulture)
        );
    }

    public override string ToString()
    {
        return String.Format(
            "{0} {1}",
            x.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture),
            y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)
        );
    }
}
