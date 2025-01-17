﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Invoices.Data.Models;
namespace Invoices.DataProcessor.ExportDto
{
    [XmlType(nameof(Client))]
    public class ExportClientDto
    {
        [XmlElement(nameof(ClientName))] 
        public string ClientName { get; set; } = null!;

        [XmlElement(nameof(VatNumber))] 
        public string VatNumber { get; set; } = null!;

        [XmlArray(nameof(Invoices))] 
        public ExportInvoicesDto[] Invoices { get; set; } = null!;
        [XmlAttribute(nameof(InvoicesCount))]
        public int InvoicesCount { get; set; }
    }
}
