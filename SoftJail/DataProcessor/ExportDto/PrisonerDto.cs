﻿using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftJail.DataProcessor.ExportDto
{
    public class PrisonerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CellNumber { get; set; }

        public List<OfficerDto> Officers { get; set; }

        public decimal TotalOfficerSalary { get { return Officers.Sum(o => o.Salary); } }
    }
}
