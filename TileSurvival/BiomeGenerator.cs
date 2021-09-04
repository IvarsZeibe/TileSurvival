using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TileSurvival
{
    public enum HeatMap
    {
        Cold,
        Warm,
        Hot
    }
    public enum WetnessMap
    {
        Low,
        Medium,
        High
    }
    public enum HeightMap
    {
        Low,
        Medium,
        High
    }
    class BiomeGenerator
    {
        public static List<float> GradientForPolygon(List<Vector2> verticies)
        {
            var values = new List<float>();
            foreach(var vertex in verticies)
            {
                values.Add(ImprovedNoise.Noise2D(vertex / 64.6f, Game1.seed) + ImprovedNoise.Noise2D(vertex / 32.3f, Game1.seed) * 0.5f);
            }
            return values;
        }
        public static Biome GenerateBiome(Point coord)
        {
            HeatMap heat = Heat(coord.ToVector2());
            WetnessMap wetness = Wetness(coord.ToVector2());
            HeightMap height = Height(coord.ToVector2());

            if (height == HeightMap.High)
                return Biome.Mountains;
            switch (heat)
            {
                case HeatMap.Cold:
                    return Biome.Snow;
                case HeatMap.Warm:
                    switch(wetness)
                    {
                        case WetnessMap.Low:
                            return Biome.Plains;
                        case WetnessMap.Medium:
                            return Biome.Forest;
                        default:
                            break;
                    }
                    break;
                case HeatMap.Hot:
                    switch (wetness)
                    {
                        case WetnessMap.Low:
                            return Biome.Desert;
                        case WetnessMap.Medium:
                            return Biome.Plains;
                        case WetnessMap.High:
                            break;
                    }
                    break;
                default:
                    break;
            }
            if (height == HeightMap.Low)
                return Biome.Ocean;
            else
                return Biome.Forest;
            //if (heat < -0.3)
            //    return Biome.Mesa;
            //else if (heat < -0.1)
            //    return Biome.Desert;
            //else if (heat < 0.05)
            //    return Biome.Plains;
            //else
            //    return Biome.Forest;
        }
        static HeatMap Heat(Vector2 pos)
        {
            //Perlin.Reseed(Game1.seed);
            //float heat = Perlin.Perlin2D(pos / 32);
            float heat = ImprovedNoise.Noise2D(pos / 64, Game1.seed - 1);
            if (heat < -0.4)
                return HeatMap.Cold;
            else if (heat < 0.1f)
                return HeatMap.Warm;
            else
                return HeatMap.Hot;

        }
        static WetnessMap Wetness(Vector2 pos)
        {
            //Perlin.Reseed(Game1.seed + 1);
            //float wetness = Perlin.Perlin2D(pos / 32.0f);
            float wetness = ImprovedNoise.Noise2D(pos / 16, Game1.seed);
            if (wetness < -0.1)
                return WetnessMap.Low;
            else if (wetness < 0.1f)
                return WetnessMap.Medium;
            else
                return WetnessMap.High;
        }
        static HeightMap Height(Vector2 pos)
        {
            //Perlin.Reseed(Game1.seed + 2);
            //float height = Perlin.Perlin2D(pos / 128f);
            float height = ImprovedNoise.Noise2D(pos / 16, Game1.seed + 1);
            if (height < -0.1f)
                return HeightMap.Low;
            else if (height < 0.3f)
                return HeightMap.Medium;
            else
                return HeightMap.High;
        }

        ///Version 2
        //Biome GenerateBiome(Point coord)
        //{
        //    if (IsSeedTile(coord))
        //    {
        //        int hashCode = coord.GetHashCode() * coord.GetHashCode() * Game1.seed;
        //        //switch (Math.Abs(hashCode % (hashCode / 10)))
        //        //switch(new Random(hashCode * Game1.seed).Next(10))
        //        //int sum = 0;
        //        //while (hashCode != 0)
        //        //{
        //        //    sum += hashCode % 10;
        //        //    hashCode /= 10;
        //        //}
        //        //hashCode = sum * Game1.seed;
        //        switch (Math.Abs(hashCode % (hashCode / 10)))
        //        {
        //            case 0:
        //            case 1:
        //                return Biome.Desert;
        //            case 2:
        //            case 3:
        //                return Biome.Forest;
        //            case 4:
        //            case 5:
        //                return Biome.Ocean;
        //            case 6:
        //            case 7:
        //                return Biome.Mesa;
        //            default:
        //                return Biome.Plains;
        //        }

        //    }
        //    else
        //    {
        //        Point seedTile = SearchSeedTile(coord);
        //        return GenerateBiome(seedTile);
        //    }

        //}
        //Point SearchSeedTile(Point origin)
        //{
        //    int radius = 1;
        //    while (true)
        //    {
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(i, -radius) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(radius, i) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(-i, radius) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        for (int i = -radius; i < radius; i++)
        //        {
        //            Point seedTile = new Point(-radius, -i) + origin;
        //            if (IsSeedTile(seedTile))
        //                return seedTile;
        //        }
        //        radius++;
        //    }
        //}
        //bool IsSeedTile(Point coord)
        //{
        //    int hashCode = coord.GetHashCode() * Game1.seed;
        //    //int sum = 0;
        //    //while (hashCode != 0)
        //    //{
        //    //    sum += hashCode % 10;
        //    //    hashCode /= 10;
        //    //}
        //    //hashCode = sum * Game1.seed;
        //    return Math.Abs(new Random(hashCode * Game1.seed).Next(100)) < 2;
        //    //return Math.Abs(hashCode % (hashCode / 1000)) < 80;
        //}

        /// Version1
        //Biome GenerateBiome(Point coord, int recursion = 0)
        //{
        //    var color = CloneBiome(coord, recursion);
        //    if (color != null)
        //        return (Biome)color;

        //    int hashCode = coord.GetHashCode() * Game1.seed;
        //    int biome = Math.Abs(hashCode % (hashCode / 10));
        //    switch (biome)
        //    {
        //        case 0:
        //        case 1:
        //            return Biome.Desert;
        //        case 2:
        //        case 3:
        //            return Biome.Ocean;
        //        case 4:
        //        case 5:
        //            return Biome.Forest;
        //        case 6:
        //        case 7:
        //            return Biome.Mesa;
        //        default:
        //            return Biome.Plains;
        //    }
        //}
        //Biome? CloneBiome(Point coord, int recursion)
        //{
        //    if (recursion > 100)
        //        return null;
        //    int hashCode = coord.GetHashCode() * Game1.seed;
        //    int cloneFrom = Math.Abs((hashCode / 10) % (hashCode / 100));
        //    switch (cloneFrom)
        //    {
        //        case 0:
        //        case 1:
        //        case 2:
        //        case 3:
        //            return GenerateBiome(new Point(coord.X - 1, coord.Y), recursion);
        //        case 4:
        //        case 5:
        //        case 6:
        //        case 7:
        //            return GenerateBiome(new Point(coord.X, coord.Y - 1), recursion);
        //        default:
        //            return null;
        //    }
        //}
    }
}
