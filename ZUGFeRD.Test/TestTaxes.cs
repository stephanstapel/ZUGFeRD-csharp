using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace s2industries.ZUGFeRD.Test;

[TestClass]
public class TestTaxes
{
    [TestMethod]
    [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
    [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
    public void SavingThenReadingAppliedTradeTaxesShouldWork(ZUGFeRDVersion version, Profile profile)
    {
        InvoiceDescriptor expected = InvoiceDescriptor.CreateInvoice("123", new DateTime(2024, 12, 5), CurrencyCodes.EUR);
        var lineItem = expected.AddTradeLineItem(name: "Something",
            grossUnitPrice: 9.9m,
            netUnitPrice: 9.9m,
            billedQuantity: 20m,
            taxType: TaxTypes.VAT,
            categoryCode: TaxCategoryCodes.S,
            taxPercent: 19m
            );
        lineItem.LineTotalAmount = 198m; // 20 * 9.9
        expected.AddApplicableTradeTax(
            basisAmount: lineItem.LineTotalAmount!.Value,
            percent: 19m,
            taxAmount: 29.82m, // 19% of 198
            typeCode: TaxTypes.VAT,
            categoryCode: TaxCategoryCodes.S,
            allowanceChargeBasisAmount: -5m
            );
        expected.LineTotalAmount = 198m;
        expected.TaxBasisAmount = 198m;
        expected.TaxTotalAmount = 29.82m;
        expected.GrandTotalAmount = 198m + 29.82m;
        expected.DuePayableAmount = expected.GrandTotalAmount;

        using MemoryStream ms = new();
        expected.Save(ms, version, profile);
        ms.Seek(0, SeekOrigin.Begin);

        InvoiceDescriptor actual = InvoiceDescriptor.Load(ms);

        Assert.AreEqual(expected.Taxes.Count, actual.Taxes.Count);
        Assert.AreEqual(1, actual.Taxes.Count);
        Tax actualTax = actual.Taxes[0];
        Assert.AreEqual(198m, actualTax.BasisAmount);
        Assert.AreEqual(19m, actualTax.Percent);
        Assert.AreEqual(29.82m, actualTax.TaxAmount);
        Assert.AreEqual(TaxTypes.VAT, actualTax.TypeCode);
        Assert.AreEqual(TaxCategoryCodes.S, actualTax.CategoryCode);
        Assert.AreEqual(-5m, actualTax.AllowanceChargeBasisAmount);
    }
}
