using GmmlPatcher;
using GmmlHooker;
using UndertaleModLib;
using UndertaleModLib.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using WysApi.Api;
using System.IO;
using UndertaleModLib.Decompiler;
using System.Reflection;
using TSIMPH;

namespace SnailBox
{


    public static class Extensions
    {
        public static List<string[]> GMLConvertAll(this List<IGMLSerializable> me)
        {
            List<string[]> data = new List<string[]>();
            foreach (IGMLSerializable item in me)
            {
                data.Add(item.SerializeToArray());
            }
            return data;
        }


        public static void ForAllPositions(this List<Tile> me, Action<int, int, Tile> tocall)
        {
            for (int i = 0; i < 32 * GameMakerMod.RoomSizeX; i++)
            {
                for (int j = 0; j < 18 * GameMakerMod.RoomSizeY; j++)
                {
                    Tile? the_tile = me.Find(l => l.X == i && l.Y == j);
                    tocall(i, j, the_tile);
                }

            }
        }
        

        public static bool AddTile(this List<Tile> me, Tile t, bool force)
        {
            Tile? the_tile = me.Find(l => l.X == t.X && l.Y == t.Y);
            if (the_tile == null)
            {
                me.Add(t);
                return true;
            }
            else if (force || the_tile.Type == TileType.Air)
            {
                me.Remove(the_tile);
                me.Add(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        //GMLConvertAll for lists of tiles
        public static List<string[]> GMLConvertAll(this List<Tile> me)
        {
            List<string[]> data = new List<string[]>();
            foreach (Tile item in me)
            {
                data.Add(item.SerializeToArray());
            }
            return data;
        }


    }


    public enum TileType : ushort
    {
        Air, //might go unused, idk
        Wall,
        WallB,
        PlayerSpawn,
        Spike
    }


    public interface IGMLSerializable
    {
        string[] SerializeToArray();
    }
    

    public class Tile : IGMLSerializable
    {
        public TileType Type = TileType.Wall;
        public int X = 0;
        public int Y = 0;

        public string[] SerializeToArray()
        {
            string[] dat = new string[3];
            dat[0] = Type.ToString();
            dat[1] = X.ToString();
            dat[2] = Y.ToString();
            return dat;
        }
        
        public Tile(int x, int y, TileType type)
        {
            X = x;
            Y = y;
            Type = type;
        }
    }
    
}
