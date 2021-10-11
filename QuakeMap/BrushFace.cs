using System;
using System.Collections.Generic;

namespace johndoe.QuakeMap
{
    /// <summary>
    /// Class for brush faces
    /// </summary>

    public class BrushFace
    {
        public Vector3 p1, p2, p3;
        public string texture;
        public float xScale, yScale, rotation, xShift, yShift;
        public Vector3 uAxis, vAxis;
        public bool Valve;
        public List<Vector3> points;
        public List<Vector2> uvs;
        public Vector2 texSize = new Vector2(256f, 256f);

        public BrushFace(
            Vector3 p1, Vector3 p2, Vector3 p3,
            string texture,
            float xShift, float yShift, float rotation, float xScale, float yScale,
            Vector3 uAxis = null, Vector3 vAxis = null, bool Valve = false
        )
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;

            this.texture = texture;

            this.xScale = yScale;
            this.yScale = xScale;
            this.rotation = rotation;
            this.xShift = xShift;
            this.yShift = yShift;

            if (Valve)
            {
                this.uAxis = uAxis;
                this.vAxis = vAxis;
                this.Valve = true;
            }

            this.points = new List<Vector3>();
            this.uvs = new List<Vector2>();
        }

        /// <summary>
        /// Returns a dummy brush face
        /// </summary>
        /// <returns></returns>
        public static BrushFace empty()
        {
            return new BrushFace(
                new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f),
                "$default", 0f, 0f, 0f, 0f, 0f
            );
        }

        /// <summary>
        /// Parses brush face lines in map files
        /// </summary>
        /// <param name="faceString"></param>
        /// <returns></returns>
        public static BrushFace Parse(string faceString)
        {
            string[] tok = faceString.Trim().Replace("  ", " ").Split(" ");
            if (tok[16] == "[")
            {
                return new BrushFace(
                    Vector3.FromStr($"{tok[1]} {tok[2]} {tok[3]}"),
                    Vector3.FromStr($"{tok[6]} {tok[7]} {tok[8]}"),
                    Vector3.FromStr($"{tok[11]} {tok[12]} {tok[13]}"),
                    tok[15],
                    float.Parse(tok[20], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(tok[26], System.Globalization.CultureInfo.InvariantCulture),
                    0f, // rotation data is not needed in the Valve format
                    float.Parse(tok[29], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(tok[30], System.Globalization.CultureInfo.InvariantCulture),
                    Vector3.FromStr($"{tok[17]} {tok[18]} {tok[19]}"),
                    Vector3.FromStr($"{tok[23]} {tok[24]} {tok[25]}"),
                    true
                );
            }
            else
            {

                return new BrushFace(
                    Vector3.FromStr($"{tok[1]} {tok[2]} {tok[3]}"),
                    Vector3.FromStr($"{tok[6]} {tok[7]} {tok[8]}"),
                    Vector3.FromStr($"{tok[11]} {tok[12]} {tok[13]}"),
                    tok[15],
                    float.Parse(tok[16], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(tok[17], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(tok[18], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(tok[19], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(tok[20], System.Globalization.CultureInfo.InvariantCulture),
                    Valve: false
                );
            }
        }

        public Vector3 Normal()
        {
            Vector3 ab = p2 - p1;
            Vector3 ac = p3 - p1;
            return ab.cross(ac);
        }

        public Vector3 Center()
        {
            return (p1 + p2 + p3) / 3;
        }

        public float Distance()
        {
            Vector3 normal = Normal();
            return ((this.p1.x * normal.x) + (this.p1.y * normal.y) + (this.p1.z * normal.z)) / MathF.Sqrt(MathF.Pow(normal.x, 2f) + MathF.Pow(normal.y, 2f) + MathF.Pow(normal.z, 2f));
        }

        public Vector3 PointCenter()
        {
            Vector3 center = Vector3.Zero;
            foreach (Vector3 point in points)
                center = center + point;
            return center / points.Count;
        }

        public void AddPoint(Vector3 point)
        {
            foreach (Vector3 p in points)
            {
                if (point == p)
                    return;
            }
            points.Add(point);
        }

        public Vector2 GetUV(Vector3 point)
        {
            if (Valve)
            {
                return new Vector2(
                    point.dot(uAxis) / (texSize.x * xScale) + (xShift / texSize.x),
                    point.dot(vAxis) / (texSize.y * yScale) + (yShift / texSize.y)
                );
            }
            else
            {

                Vector2 uv = Vector2.Zero;
                Vector3 normal = Normal();
                float du = MathF.Abs(normal.dot(new Vector3(0f, 0f, 1f)));
                float dr = MathF.Abs(normal.dot(new Vector3(0f, 1f, 0f)));
                float df = MathF.Abs(normal.dot(new Vector3(1f, 0f, 0f)));

                if (du >= dr && du >= df)
                    uv = new Vector2(point.x, -point.y);
                else if (dr >= du && dr >= df)
                    uv = new Vector2(point.x, -point.z);
                else if (df >= du && df >= dr)
                    uv = new Vector2(point.y, -point.z);

                Vector2 rotated = Vector2.Zero;
                float angle = (MathF.PI / 180) * rotation;
                rotated.x = uv.x * MathF.Cos(angle) - uv.y * MathF.Sin(angle);
                rotated.y = uv.x * MathF.Sin(angle) + uv.y * MathF.Cos(angle);
                uv = rotated;

                uv.x /= texSize.x;
                uv.y /= texSize.y;

                uv.x /= xScale;
                uv.y /= yScale;

                uv.x += xShift / texSize.x;
                uv.y += yShift / texSize.y;

                return uv;
            }
        }

        public void SortVertices()
        {
            Vector3 center = Center();
            Vector3 normal = Normal().normalize();

            for (int i = 0; i < points.Count; i++)
            {
                for (int k = 0; k < points.Count - 1; k++)
                {
                    if (i == k)
                        continue;

                    Vector3 ca = center - points[i];
                    Vector3 cb = center - points[k];
                    Vector3 caXcb = ca.cross(cb);

                    if (normal.dot(caXcb) <= 0.0001)
                    {
                        Vector3 temp = points[i];
                        points[i] = points[k];
                        points[k] = temp;
                    }
                }
            }
        }

        public override string ToString()
        {
            if (Valve)
            {
                return String.Format(
                    "( {0} ) ( {1} ) ( {2} ) {3} [ {4} {5} ] [ {6} {7} ] 0 {8} {9}",
                    p1, p2, p3, texture,
                    uAxis, xShift,
                    vAxis, yShift,
                    xScale, yScale
                );
            }
            else
            {
                return String.Format(
                    "( {0} ) ( {1} ) ( {2} ) {3} {4} {5} {6} {7} {8}",
                    p1, p2, p3, texture,
                    xScale,
                    yScale,
                    rotation,
                    xShift,
                    yShift
                );
            }
        }
    }

}