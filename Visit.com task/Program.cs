internal partial class Program
{
    private static void Main(string[] args)
    {

        TaxCalculator taxCalculator = new TaxCalculator();
        //GetStandardTaxRate
        Console.WriteLine($"GetStandardTaxRate for {Commodity.Default} => " + taxCalculator.GetStandardTaxRate(Commodity.Default));

        //SetCustomTaxRate
        taxCalculator.SetCustomTaxRate(Commodity.CulturalServices, 0.15);
        //taxCalculator.SetCustomTaxRate(Commodity.Food, 0.15);
        //taxCalculator.SetCustomTaxRate(Commodity.Literature, 0.12);
        //taxCalculator.SetCustomTaxRate(Commodity.CulturalServices, 0.12);

        //GetTaxRateForDateTime
        //Console.WriteLine();
        //Console.WriteLine($"GetTaxRateForDateTime for {Commodity.CulturalServices} => " + taxCalculator.GetTaxRateForDateTime(Commodity.Alcohol, new DateTime(2023, 7, 29, 18, 00, 00)));
        //Console.WriteLine($"GetTaxRateForDateTime for {Commodity.CulturalServices} => " + taxCalculator.GetTaxRateForDateTime(Commodity.CulturalServices, new DateTime(2023, 7, 29, 18, 00, 00))); 
        //Console.WriteLine($"GetTaxRateForDateTime for {Commodity.CulturalServices} => " + taxCalculator.GetTaxRateForDateTime(Commodity.CulturalServices, DateTime.UtcNow));

        //GetCurrentTaxRate
        //Console.WriteLine();
        //Console.WriteLine($"CulturalServices for {Commodity.CulturalServices} => " + taxCalculator.GetCurrentTaxRate(Commodity.Food));
        //Console.WriteLine($"CulturalServices for {Commodity.CulturalServices} => " + taxCalculator.GetCurrentTaxRate(Commodity.CulturalServices));

    }
}


