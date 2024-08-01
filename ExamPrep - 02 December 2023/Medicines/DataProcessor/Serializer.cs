using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ExportDtos;
using Medicines.Utilities;
using Newtonsoft.Json;

namespace Medicines.DataProcessor
{
    using Medicines.Data;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime givenDate;

            if (!DateTime.TryParse(date, out givenDate))
            {
                throw new ArgumentException("Invalid date format!");
            }

            var patients = context.Patients
                .Where(p => p.PatientsMedicines
                    .Any(pm => pm.Medicine.ProductionDate >= givenDate))
                .Select(p => new ExportPatientsDto()
                {
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Genger = p.Gender.ToString().ToLower(),
                    Medicines = p.PatientsMedicines
                        .Where(pm => pm.Medicine.ProductionDate >= givenDate)
                        .Select(pm => pm.Medicine)
                        .OrderByDescending(pm => pm.ExpiryDate)
                        .ThenBy(pm => pm.Price)
                        .Select(m => new ExportMedicinesDto()
                        {
                            Name = m.Name,
                            Price = m.Price.ToString("f2"),
                            Category = m.Category.ToString().ToLower(),
                            Producer = m.Producer,
                            BestBefore = m.ExpiryDate.ToString("yyyy-MM-dd")
                        }).ToArray()
                })
                .OrderByDescending(p => p.Medicines.Length)
                .ThenBy(p => p.Name)
                .ToArray();

            XmlHelper helper = new XmlHelper();
            return helper.Serialize(patients, "Patients");

        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                .Where(m => m.Category == (Category)medicineCategory
                            && m.Pharmacy.IsNonStop)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = m.Price.ToString("F2"),
                    Pharmacy = new
                    {
                        m.Pharmacy.Name,
                        m.Pharmacy.PhoneNumber
                    }
                }).ToArray();
            return JsonConvert.SerializeObject(medicines, Formatting.Indented);
        }
    }
}
