using System;
using System.Collections.Generic;

namespace johndoe.QuakeMap
{
    /// <summary>
    /// Class for bezier patch vertices. Each patch is an n-dimensional array.
    /// </summary>
    public class PatchVert
    {
        public Vector3 pos { get; set; }
        public Vector2 uv { get; set; }

        public PatchVert(Vector3 _pos, Vector2 _uv)
        {
            pos = _pos;
            uv = _uv;
        }

        /// <summary>
        /// Parses patch vertex lines in map files
        /// </summary>
        /// <param name="patchString"></param>
        /// <returns></returns>
        public static List<PatchVert> parse(string patchString)
        {
            List<PatchVert> res = new List<PatchVert>();
            string[] tok = patchString.Split(" ");

            if ((tok.Length - 2) % 7 != 0)
            {
                Console.WriteLine("Parse error. Invalid patch");
                return null;
            }

            for (int i = 2; i < tok.Length - 1; i += 7)
            {
                res.Add(new PatchVert(
                    new Vector3(
                        float.Parse(tok[i], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(tok[i + 1], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(tok[i + 2], System.Globalization.CultureInfo.InvariantCulture)
                    ),
                    new Vector2(
                        float.Parse(tok[i + 3], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(tok[i + 4], System.Globalization.CultureInfo.InvariantCulture)
                    )
                ));
            }

            return res;
        }

        public override string ToString()
        {
            return $"( {pos} {uv} )";
        }
    }
}