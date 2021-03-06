﻿using System;

namespace EngineTests.TestModelWithInterfaces
{
    public class CreditDefaultSwap : ICreditDefaultSwap
    {
        public string InstrumentName { get; private set; }

        public DateTime? MaturityDate { get; set; }

        public string RefEntity { get; set; }

        public string Tenor { get; set; }

        public decimal Spread { get; set; }

        public decimal Nominal { get; set; }

        public string Currency { get; set; }

        public string Seniority { get; set; }

        public string Restructuring { get; set; }

        public string TransactionType { get; set; }
    }
}