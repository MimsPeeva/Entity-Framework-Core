﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachesDto
    {
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }
        [XmlElement("CoachName")] 
        public string CoachName { get; set; }
        [XmlArray("Footballers")]
        public ExportFootballersDto[] Footballers { get; set; }
    }
}
