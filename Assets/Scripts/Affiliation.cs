
using System;

[Flags]
public enum Affiliation
{
    None = 0,
    
    Neutral = 1 << 0,
    Player  = 1 << 1,
    Demons  = 1 << 2,

    Any = int.MaxValue, 
}
