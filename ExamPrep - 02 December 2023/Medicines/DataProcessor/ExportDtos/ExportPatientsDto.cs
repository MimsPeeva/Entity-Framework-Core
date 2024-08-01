using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientsDto
    {
        [XmlAttribute("Gender")]
        public string Genger { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; }
        [XmlArray("Medicines")]
        public ExportMedicinesDto[] Medicines { get; set; }


    }
}
