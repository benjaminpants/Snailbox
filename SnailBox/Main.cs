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
using SimplexNoise;


namespace SnailBox
{
    public class GameMakerMod : IGameMakerMod
    {
        public const int RoomSizeX = 4;

        public const int RoomSizeY = 3;

        
        public static Dictionary<string, string> GMLkvp = new Dictionary<string, string>();

        [GmlInterop("generate_world_values")]
        public static string[][] GenerateWorld(ref Interop.CInstance self, ref Interop.CInstance other, double seed)
        {
            List<Tile> Tiles = new List<Tile>();
            Console.WriteLine(seed);
            Noise.Seed = (int)seed;
            Random rng = new Random((int)seed);
            float[,] n = Noise.Calc2D(32 * RoomSizeX, 18 * RoomSizeY, 0.1f);
            Tiles.ForAllPositions(delegate (int i, int j, Tile t)
            {
                Tiles.AddTile(new Tile(i, j, TileType.Air), true); //fill all tiles with air
            });
            Tiles.ForAllPositions(delegate (int i, int j, Tile t)
            {
                if (n[i, j] > 120f)
                {
                    Tiles.AddTile(new Tile(i, j, TileType.Wall), false);
                }
            });



            //decorations
            Noise.Seed = ((int)(seed * 2)) - 1;

            n = Noise.Calc2D(32 * RoomSizeX, 18 * RoomSizeY, 0.07f);

            Tiles.ForAllPositions(delegate (int i, int j, Tile t)
            {
                
                if (n[i, j] > 150f && t.Type == TileType.Wall)
                {
                    Tiles.AddTile(new Tile(i, j, TileType.WallB), true);
                }
            });


            //valid til
            List<Tile> ValidTiles = new List<Tile>();
            Tiles.ForAllPositions(delegate (int i, int j, Tile t)
            {
                if (t.Type == TileType.Air)
                {
                    ValidTiles.Add(t);
                }
            });

            Tile rngtile = ValidTiles[rng.Next(0, ValidTiles.Count - 1)];

            Tiles.AddTile(new Tile(rngtile.X, rngtile.Y, TileType.PlayerSpawn), false);



            return Tiles.GMLConvertAll().ToArray();
        }

        public void Load(int audioGroup, UndertaleData data, ModData currentmod)
        {
            if (audioGroup != 0) return;
            string gmlfolder = Path.Combine(currentmod.path, "GMLSource");

            GMLkvp.LoadGMLFolder(gmlfolder);

            data.Strings.First(x => x.Content == "SaavoGame23-2.sav").Content = "SnailBox_SaavoGame23-2.sav";

            data.Strings.First(x => x.Content == "SaveFileBackup-").Content = "SnailBox_SaveFileBackup-";

            data.Strings.First(x => x.Content == "SaveFileBackup_alt_-").Content = "SnailBox_SaveFileBackup_alt_-";




            UndertaleGameObject playerspawn_object = new UndertaleGameObject();

            playerspawn_object.Name = data.Strings.MakeString("obj_playerspawn");

            playerspawn_object.Sprite = data.Sprites.ByName("spr_player");

            playerspawn_object.Visible = false;

            data.GameObjects.Add(playerspawn_object);





            UndertaleRoom room = Conviences.CreateBlankLevelRoom("snailbox_main",data);

            room.AddObjectToLayer(data, "obj_post_processing_draw", "PostProcessing");

            room.AddObjectToLayer(data, "obj_music_helpy", "FadeOutIn");

            room.AddObjectToLayer(data, "obj_levelstyler", "PostProcessing");

            UndertaleRoom.GameObject pl = room.AddObjectToLayer(data, "obj_player", "Player");

            room.CreationCodeId = Conviences.CreateCode(data, "SnailboxMainLevelCreationCode", GMLkvp["GenerateWorld"]);

            room.Width *= RoomSizeX;

            room.Height *= RoomSizeY;

            data.Rooms.Add(room);

            Dictionary<string, string> AppendScripts = GMLKVP.DictionarizeGMLFolder(Path.Combine(gmlfolder, "AppendScripts"));

            foreach (KeyValuePair<string, string> kvp in AppendScripts)
            {
                UndertaleCode code = data.Code.First(c => c.Name.Content == kvp.Key);
                if (code != null)
                {
                    code.AppendGmlSafe(kvp.Value, data);
                }
            }

            data.CreateLegacyScript("collision_line_first",GMLkvp["collision_line_first"],7);

            data.Code.ByName("gml_Object_obj_persistent_Step_0").AppendGmlSafe(GMLkvp["LazyPersistantEdit"], data);

            data.Code.ByName("gml_Object_obj_player_Other_0").ReplaceGmlSafe(GMLkvp["DieInsteadOfWin"], data);

            data.GameObjects.ByName("obj_player").EventHandlerFor(EventType.Draw, EventSubtypeDraw.DrawGUI, data.Strings, data.Code, data.CodeLocals)
                .AppendGmlSafe(GMLkvp["DrawGUI"], data);

            data.GameObjects.ByName("obj_player").EventHandlerFor(EventType.Draw, EventSubtypeDraw.DrawEnd, data.Strings, data.Code, data.CodeLocals)
                .AppendGmlSafe(GMLkvp["DrawEnd"], data);            

        }

    }
}