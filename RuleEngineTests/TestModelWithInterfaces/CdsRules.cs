﻿using System;
using RulesEngine.RulesEngine;

namespace RuleEngineTests.TestModelWithInterfaces
{
    public class CdsRules : MappingRules<ICdsTrade>
    {
        public CdsRules()
        {
            Set(t => t.CounterpartyRole)
                .With(t => t.Sales != null ? "Client" : "Dealer")
                .OnChanged(t => t.Sales)
                .EndRule();

            Set(t => t.ClearingHouse)
                .With(t => GetDefaultClearingHouse(t.Counterparty, t.CdsProduct.RefEntity))
                .OnChanged(t => t.CdsProduct.RefEntity)
                .Or(t => t.Counterparty)
                .EndRule();

            Set(t => t.SalesCredit)
                .With(t => Calculator(t.CdsProduct.Spread, t.CdsProduct.Nominal))
                .OnChanged(t => t.CdsProduct.Spread)
                .Or(t => t.CdsProduct.RefEntity)
                .EndRule();

            Set(t => t.CdsProduct.TransactionType)
                .With(t => GetTransactionType(t.CdsProduct.RefEntity))
                .OnChanged(t => t.CdsProduct.RefEntity)
                .EndRule();

            Set(t => t.CdsProduct.Currency)
                .With(t => GetDefaultCurrency(t.CdsProduct.TransactionType))
                .OnChanged(t => t.CdsProduct.TransactionType)
                .EndRule();

            Set(t => t.CdsProduct.Restructuring)
                .With(t => GetDefaultRestructuring(t.CdsProduct.TransactionType))
                .OnChanged(t => t.CdsProduct.TransactionType)
                .EndRule();

            Set(t => t.CdsProduct.Seniority)
                .With(t => GetDefaultSeniority(t.CdsProduct.TransactionType))
                .OnChanged(t => t.CdsProduct.TransactionType)
                .EndRule();
        }

        private string GetTransactionType(string refEntity)
        {
            if (refEntity == "AXA")
            {
                return "Standard European Corporate";
            }

            return null;
        }

        private string GetDefaultSeniority(string transactionType)
        {
            if (transactionType == "Standard European Corporate")
            {
                return "SNR";
            }

            return null;
        }

        private string GetDefaultRestructuring(string transactionType)
        {
            if (transactionType == "Standard European Corporate")
            {
                return "MMR";
            }

            return null;
        }

        private string GetDefaultCurrency(string transactionType)
        {
            if (transactionType == "Standard European Corporate")
            {
                return "EUR";
            }

            return null;
        }

        private static decimal Calculator(decimal spread, decimal nominal)
        {
            return 4;
        }

        private static string GetDefaultClearingHouse(string counterpary, string refEntity)
        {
            if (refEntity == "AXA" && counterpary == "CHASEOTC")
            {
                return "ICEURO";
            }

            if (refEntity == "RENAULT" && counterpary == "CHASEOTC")
            {
                return "ICETRUST";
            }

            return null;
        }

        #region Overrides of MappingRules<ITrade>

        protected override void Trace(Rule<ICdsTrade> triggeredRule, string triggerProperty, ICdsTrade instance)
        {
            Console.WriteLine(triggeredRule);
        }

        #endregion
    }
}