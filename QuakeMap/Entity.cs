using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using johndoe.Map2Obj;

namespace johndoe.QuakeMap
{
    /// <summary>
    /// Class for map entities
    /// </summary>
    public class Entity
    {
        public List<Brush> brushes = new List<Brush>();
        public List<Patch> patches = new List<Patch>();
        public Dictionary<string, string> keyValues = new Dictionary<string, string>();

        public override string ToString()
        {
            string res = "{\n";
            foreach (KeyValuePair<string, string> kvp in keyValues)
                res += $"\"{kvp.Key}\" \"{kvp.Value}\"\n";

            foreach (Brush brush in brushes)
                res += brush;

            foreach (Patch patch in patches)
                res += patch;

            res += "}\n";
            return res;
        }

        /// <summary>
        /// Converts a list of entities to a map string
        /// </summary>
        /// <param name="ents"></param>
        /// <returns></returns>
        public static string Stringify(List<Entity> ents)
        {
            string res = "";

            foreach (Entity ent in ents)
                res += ent;

            return res;
        }

        private static string FixTexture(string texture)
        {
            if (texture.StartsWith("common/"))
            {
                string res = texture.Split("/")[0];
                if (res.Contains("caulk") || res.Contains("nodraw"))
                    return "caulk";
                else if (res.Contains("clip"))
                    return "clip";
                else if (res == "hint" || res == "skip")
                    return res;
                else if (res.Contains("portal"))
                    return "portal_nodraw";
                else if (res.Contains("terrain"))
                    return "case";
                else
                    return "clip";
            }
            else
                return "case";
        }

        public static void ToObj(List<Entity> ents, string path)
        {
            ObjModel obj = new ObjModel();
            for (int e = 0; e < ents.Count; e++)
            {
                Entity ent = ents[e];
                for (int b = 0; b < ent.brushes.Count; b++)
                {
                    Brush brush = ent.brushes[b];
                    brush.GetIntersectionPoints();
                    for (int f = 0; f < brush.faces.Count; f++)
                    {
                        BrushFace face = brush.faces[f];
                        ObjGroup group = new ObjGroup();
                        group.name = ent.keyValues["classname"] + e + "_brush" + b + "_" + f;
                        group.material = face.texture;
                        Vector3 normal = face.Normal().normalize();
                        int n = obj.AddNormal(normal); // all vertices will share the same normal
                        Polygon poly = new Polygon();
                        for (int i = 0; i < face.points.Count; i++)
                        {
                            int v = obj.AddVec(face.points[i]);
                            int uv = obj.AddUV(face.uvs[i]);
                            poly.points.Add(new Point(v, uv, n));
                        }
                        group.polygons.Add(poly);
                        obj.groups.Add(group);
                    }
                }
            }
            obj.ToFile(path);
        }

        /// <summary>
        /// Parses key-value pairs in map files.
        /// Splits the strings by quotes.
        /// Should probably be optimized for escaped quotes.
        /// </summary>
        /// <param name="KVString"></param>
        public void parseKVP(string KVString)
        {
            string[] tok = KVString.Split('"');
            this.keyValues.Add(tok[1], tok[3]);
        }

        /// <summary>
        /// Parses map files.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static List<Entity> parseMap(string map)
        {

            string file = File.ReadAllText(map);
            string[] lines = file.Split("\n");

            List<Entity> res = new List<Entity>();

            string current = "none";

            Entity tempEntity = new Entity();
            Brush tempBrush = new Brush();
            Patch tempPatch = new Patch();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                string firstChar = StringInfo.GetNextTextElement(line, 0);

                if (line.StartsWith("//"))
                    continue;
                else if (firstChar == "{")
                {
                    if (current == "none")
                        current = "entity";
                    else if (current == "entity")
                    {
                        if (lines[i + 1].Trim().StartsWith("patchDef2"))
                        {
                            tempPatch.texture = lines[i + 3].Trim();
                            string[] size = lines[i + 4].Trim().Split(" ");
                            tempPatch.rows = int.Parse(size[1]);
                            tempPatch.cols = int.Parse(size[2]);
                            for (int r = 0; r < tempPatch.rows; r++)
                            {
                                tempPatch.verts.Add(PatchVert.parse(lines[i + r + 6].Trim()));
                            }
                            i += 8 + tempPatch.rows;
                            tempEntity.patches.Add(tempPatch);
                            tempPatch = new Patch();
                            continue;
                        }
                        else
                            current = "brush";
                    }
                    else
                    {
                        Console.WriteLine("Unexpected { on line " + (i + 1) + ". Current: " + current);
                        return null;
                    }
                }
                else if (firstChar == "}")
                {
                    if (current == "brush")
                    {
                        tempEntity.brushes.Add(tempBrush);
                        tempBrush = new Brush();
                        current = "entity";
                    }
                    else if (current == "entity")
                    {
                        res.Add(tempEntity);
                        tempEntity = new Entity();
                        current = "none";
                    }
                    else
                    {
                        Console.WriteLine("Unexpected } on line " + (i + 1) + ". Current: " + current);
                        return null;
                    }
                }
                else if (firstChar == "\"")
                {
                    if (current == "entity")
                    {
                        tempEntity.parseKVP(line);
                    }
                    else
                    {
                        Console.WriteLine("Unexpected \" on line " + (i + 1) + ". Current: " + current);
                        return null;
                    }
                }
                else if (firstChar == "(")
                {
                    if (current == "brush")
                    {
                        tempBrush.faces.Add(BrushFace.Parse(line));
                        continue;
                    }
                    else
                        Console.WriteLine($"Unexpected ( on line {i + 1} + Current: {current}");
                }
            }
            return res;
        }
    }
}
