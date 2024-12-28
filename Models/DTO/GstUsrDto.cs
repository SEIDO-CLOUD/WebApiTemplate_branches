using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public int NrSeededZoos { get; set; } = 0;
    public int NrUnseededZoos { get; set; } = 0;
}

public class GstUsrInfoZoosDto
{
    public string Country { get; set; } = null;
    public string City { get; set; } = null;
    public int NrZoos { get; set; } = 0;
}


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoZoosDto> Zoos { get; set; } = null;
}


