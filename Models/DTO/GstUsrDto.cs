using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public int NrSeededZoos { get; set; } = 0;
    public int NrUnseededZoos { get; set; } = 0;
}


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
}


