using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public string Header {get;  set;}
} 


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
}


