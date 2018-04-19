using System;

public class RandomHelper
{

    public static int Range(float x, float y, int key, int range)
    {
        return Range((int)x, (int)y, key, range);
    }
    public static int Range(int x,int y,int key,int range)
    {
        uint hash = (uint)key;
        hash ^= (uint)x;
        hash *= 0x51d7348d;
        hash ^= 0x85dbdda2;
        hash = (hash << 16) ^ (hash >> 16);
        hash *= 0x7588f287;
        hash ^= (uint)y;
        hash *= 0x487a5559;
        hash ^= 0x64887219;
        hash = (hash << 16) ^ (hash >> 16);
        hash *= 0x63288691;
        return (int)(hash % range);
    }
}

