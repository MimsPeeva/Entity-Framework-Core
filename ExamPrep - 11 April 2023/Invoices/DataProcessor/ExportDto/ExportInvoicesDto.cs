﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Invoice")]
    public class ExportInvoicesDto
    {
     [XmlElement(nameof(InvoiceNumber))]
     public int InvoiceNumber { get; set; } 
     [XmlElement(nameof(InvoiceAmount))]
     public decimal InvoiceAmount{ get; set; }
        [XmlElement(nameof(DueDate))]
     public string DueDate { get; set; } = null!;
        [XmlElement(nameof(Currency))]
     public string Currency { get; set; } = null!;
    }
}
