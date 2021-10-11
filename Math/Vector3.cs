using System;
public class Vector3
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public static Vector3 Zero = new Vector3(0f, 0f, 0f);
    public static Vector3 Up = new Vector3(0f, 0f, 1f);
    public static Vector3 Right = new Vector3(0f, 1f, 0f);
    public static Vector3 Forward = new Vector3(1f, 0f, 0f);

    public Vector3(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    }

    public static Vector3 operator +(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
    }

    public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }

    public static Vector3 operator -(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
    }

    public static Vector3 operator *(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
    }

    public static Vector3 operator *(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
    }

    public static Vector3 operator /(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
    }

    public static Vector3 operator /(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
    }

    public static bool operator ==(Vector3 lhs, Vector3 rhs)
    {
        return (lhs - rhs).len() <= 1e-5;
    }

    public static bool operator !=(Vector3 lhs, Vector3 rhs)
    {
        return (lhs - rhs).len() >= 1e-5;
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

    public float dot(Vector3 rhs)
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

    public Vector3 normalize()
    {
        return this / this.len();
    }

    public Vector3 cross(Vector3 rhs)
    {
        return new Vector3(
            this.y * rhs.z - this.z * rhs.y,
            this.z * rhs.x - this.x * rhs.z,
            this.x * rhs.y - this.y * rhs.x
        );
    }

    public float distance(Vector3 rhs)
    {
        return MathF.Sqrt(MathF.Pow(this.x - rhs.x, 2) + MathF.Pow(this.y - rhs.y, 2) + MathF.Pow(this.z - rhs.z, 2));
    }

    public Vector3 lerp(Vector3 rhs, float alpha)
    {
        return new Vector3(
            this.x + ((rhs.x - this.x) * alpha),
            this.y + ((rhs.y - this.y) * alpha),
            this.z + ((rhs.z - this.z) * alpha)
        );
    }

    public Vector3 round(int digits = 3)
    {
        return new Vector3(MathF.Round(x, digits), MathF.Round(y, digits), MathF.Round(z, digits));
    }

    public static Vector3 FromStr(string vec)
    {
        string[] tok = vec.Split(" ");
        return new Vector3(
            float.Parse(tok[0], System.Globalization.CultureInfo.InvariantCulture),
            float.Parse(tok[1], System.Globalization.CultureInfo.InvariantCulture),
            float.Parse(tok[2], System.Globalization.CultureInfo.InvariantCulture)
        );
    }

    public override string ToString()
    {
        return String.Format(
            "{0} {1} {2}",
            x.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture),
            y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture),
            z.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)
        );
    }
}