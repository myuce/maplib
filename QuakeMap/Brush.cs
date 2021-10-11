using System.Collections.Generic;
using Console = System.Console;

namespace johndoe.QuakeMap
{
    /// <summary>
    /// Class for map brushes
    /// </summary>
    public class Brush
    {
        public List<BrushFace> faces;

        public Brush()
        {
            faces = new List<BrushFace>();
        }

        public static Vector3 GetPlaneIntersection(BrushFace face1, BrushFace face2, BrushFace face3)
        {
            Vector3 normal1 = face1.Normal().normalize();
            Vector3 normal2 = face2.Normal().normalize();
            Vector3 normal3 = face3.Normal().normalize();

            float det = normal1.dot(normal2.cross(normal3));

            if ((det <= 1e-5 && det >= -1e-5) || float.IsNaN(det))
                return null;
            else
            {
                return (
                    normal2.cross(normal3) * face1.Distance() +
                    normal3.cross(normal1) * face2.Distance() +
                    normal1.cross(normal2) * face3.Distance()
                ) / det;
            }
        }

        private bool PointIsLegal(Vector3 point)
        {
            foreach (BrushFace face in faces)
            {
                Vector3 facing = (point - face.Center()).normalize();
                if (facing.dot(face.Normal().normalize()) < -1e-5)
                    return false;
            }
            return true;
        }

        public void GetIntersectionPoints()
        {
            foreach (BrushFace face1 in faces.GetRange(0, faces.Count - 2))
            {
                foreach (BrushFace face2 in faces.GetRange(0, faces.Count - 1))
                {
                    foreach (BrushFace face3 in faces)
                    {
                        if (face1 == face2 || face1 == face3 || face2 == face3)
                            continue;

                        Vector3 intersectionPoint = GetPlaneIntersection(face1, face2, face3);

                        if (intersectionPoint is null || !PointIsLegal(intersectionPoint))
                            continue;

                        face1.AddPoint(intersectionPoint);
                        face2.AddPoint(intersectionPoint);
                        face3.AddPoint(intersectionPoint);

                    }
                }
            }

            foreach (BrushFace face in faces)
            {
                face.SortVertices();
                foreach (Vector3 point in face.points)
                    face.uvs.Add(face.GetUV(point));
            }
        }

        public override string ToString()
        {
            string res = "{\n";

            foreach (BrushFace face in faces)
                res += face + "\n";

            res += "}\n";
            return res;
        }
    }
}