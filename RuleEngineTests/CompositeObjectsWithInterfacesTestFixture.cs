using BusinessRulesEngine.Interceptors;
using NUnit.Framework;
using RuleEngineTests.TestModelWithInterfaces;

namespace RuleEngineTests
{
    [TestFixture]
    public class CompositeObjectsWithInterfacesTestFixture
    {
        [Test]
        public void Counterparty_change_fills_product()
        {
            var trade = new CdsTrade
            {
                Product = new CreditDefaultSwap()
            };

            var p = new InterfaceWrapper<ICdsTrade>(trade, new CdsRules(trade)).Target;

            p.CdsProduct.RefEntity = "AXA";

            p.Counterparty = "CHASEOTC";

            Assert.AreEqual("ICEURO", trade.ClearingHouse);
            Assert.AreEqual("MMR", trade.CdsProduct.Restructuring);
            Assert.AreEqual("SNR", trade.CdsProduct.Seniority);
        }
    }
}