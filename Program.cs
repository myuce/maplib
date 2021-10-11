using System;
using System.Collections.Generic;
using johndoe.QuakeMap;

class Program
{
    static void Main(string[] args)
    {
        List<Entity> ents = Entity.parseMap("maps/test_valve.map");
        Entity.ToObj(ents, "C:/Users/Mehmet/Desktop/maptest/test.obj");
    }
}
