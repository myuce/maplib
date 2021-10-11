using System;
using System.IO;
using System.Collections.Generic;

namespace johndoe.Map2Obj
{

    public class ObjModel
    {
        public List<Vector3> verts = new List<Vector3>();
        private Dictionary<string, int> vert_hash = new Dictionary<string, int>();
        public List<Vector3> normals = new List<Vector3>();
        private Dictionary<string, int> normal_hash = new Dictionary<string, int>();
        public List<Vector2> uvs = new List<Vector2>();
        private Dictionary<string, int> uv_hash = new Dictionary<string, int>();
        public List<ObjGroup> groups = new List<ObjGroup>();

        public int AddVec(Vector3 v)
        {

            string key = v.round(3).ToString();
            if (!vert_hash.ContainsKey(key))
            {
                verts.Add(v);
                int idx = verts.Count - 1;
                vert_hash[key] = idx;
                return idx;
            }
            else
            {
                return vert_hash[key];
            }
        }

        public int AddNormal(Vector3 n)
        {

            string key = n.round(3).ToString();
            if (!normal_hash.ContainsKey(key))
            {
                normals.Add(n);
                int idx = normals.Count - 1;
                normal_hash[key] = idx;
                return idx;
            }
            else
            {
                return normal_hash[key];
            }
        }

        public int AddUV(Vector2 t)
        {

            string key = t.round(3).ToString();
            if (!uv_hash.ContainsKey(key))
            {
                uvs.Add(t);
                int idx = uvs.Count - 1;
                uv_hash[key] = idx;
                return idx;
            }
            else
            {
                return uv_hash[key];
            }
        }

        public void ToFile(string path)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                file.WriteLine("# exported by johndoe's map converter");
                file.WriteLine("mtllib test.mtl");

                foreach (Vector3 vert in verts)
                    file.WriteLine($"v {vert}");
                foreach (Vector3 normal in normals)
                    file.WriteLine($"vn {normal}");
                foreach (Vector2 uv in uvs)
                    file.WriteLine($"vt {uv}");
                foreach (ObjGroup group in groups)
                    file.WriteLine(group);
            }
        }
    }

    public class ObjGroup
    {
        public string name;
        public string material;
        public List<Polygon> polygons = new List<Polygon>();

        public override string ToString()
        {
            string res = $"g {name}\n";
            res += $"usemtl {material}\n";

            foreach (Polygon poly in polygons)
                res += poly;

            return res;
        }
    }

    public class Point
    {
        public int vertex, uv, normal;

        public Point(int _vertex, int _uv, int _normal)
        {
            vertex = _vertex;
            uv = _uv;
            normal = _normal;
        }

        public override string ToString()
        {
            return $"{vertex + 1}/{uv + 1}/{normal + 1}";
        }
    }

    public class Polygon
    {
        public List<Point> points = new List<Point>();

        public override string ToString()
        {
            return "f " + string.Join(" ", points) + "\n";
        }
    }
}