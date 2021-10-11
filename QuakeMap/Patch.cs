using System.Collections.Generic;

namespace johndoe.QuakeMap
{
    /// <summary>
    /// Class for bezier patches
    /// </summary>
    public class Patch
    {
        public string texture { get; set; }
        public int rows { get; set; }
        public int cols { get; set; }
        public List<List<PatchVert>> verts;

        public Patch()
        {
            texture = "$default";
            rows = 0;
            cols = 0;
            verts = new List<List<PatchVert>>();
        }

        public override string ToString()
        {
            string res = "{\n";
            res += "patchDef2\n";
            res += "{\n";
            res += texture + "\n";
            res += $"( {rows} {cols} 0 0 0 )\n";
            res += "(\n";
            foreach (List<PatchVert> col in verts)
            {
                res += "(";

                foreach (PatchVert vert in col)
                    res += $" ( {vert.pos} {vert.uv} ) ";

                res += ")\n";
            }
            res += ")\n";
            res += "}\n}\n";

            return res;
        }
    }
}