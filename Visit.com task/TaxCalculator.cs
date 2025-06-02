internal partial class Program
{
    /// <summary>
    /// Implements a tax calculator for our client.
    /// The calculator has a set of standard tax rates that are hard-coded in the class.
    /// It also allows our client to remotely set new, custom tax rates.
    /// Finally, it allows the fetching of tax rate information for a specific commodity and point in time.
    /// TODO: We know there are a few bugs in the code below, since the calculations look messed up every now and then.
    ///       There are also a number of things that have to be implemented.
    /// </summary>
    public class TaxCalculator : ITaxCalculator
    {
        /// <summary>
        /// Get the standard tax rate for a specific commodity.
        /// </summary>
        public double GetStandardTaxRate(Commodity commodity)
        {
            if (commodity == Commodity.Default)
                return 0.25;
            if (commodity == Commodity.Alcohol)
                return 0.25;
            if (commodity == Commodity.Food)
                return 0.12;
            if (commodity == Commodity.FoodServices)
                return 0.12;
            if (commodity == Commodity.Literature)
                return 0.6;
            if (commodity == Commodity.Transport)
                return 0.6;
            if (commodity == Commodity.CulturalServices)
                return 0.6;

            return 0.25;
        }

        /// <summary>
        /// This method allows the client to remotely set new custom tax rates.
        /// When they do, we save the commodity/rate information as well as the UTC timestamp of when it was done.
        /// NOTE: Each instance of this object supports a different set of custom rates, since we run one thread per customer.
        /// </summary>
        public void SetCustomTaxRate(Commodity commodity, double rate)
        {
            //TODO: support saving multiple custom rates for different combinations of Commodity/DateTime
            //TODO: make sure we never save duplicates, in case of e.g. clock resets, DST etc - overwrite old values if this happens

            DateTime now = DateTime.UtcNow;

            if (!_customRates.ContainsKey(commodity))
                _customRates[commodity] = new List<Tuple<DateTime, double>>();

            var existing = _customRates[commodity].OrderByDescending(t => t.Item1).FirstOrDefault(t => Math.Abs((t.Item1 - now).TotalSeconds) < 60); ;

            if (existing != null)
                _customRates[commodity].Remove(existing);

            _customRates[commodity].Add(Tuple.Create(now, rate));
        }


        private Dictionary<Commodity, List<Tuple<DateTime, double>>> _customRates
        = new Dictionary<Commodity, List<Tuple<DateTime, double>>>
        {
            {
                Commodity.Food, new List<Tuple<DateTime, double>>
                {
                    Tuple.Create(new DateTime(2024, 01, 01), 0.12),
                    Tuple.Create(new DateTime(2025, 01, 01), 0.14),
                    Tuple.Create(DateTime.UtcNow, 0.0)
                }
            },
            {
                Commodity.Alcohol, new List<Tuple<DateTime, double>>
                {
                    Tuple.Create(new DateTime(2023, 06, 01), 0.25),
                    Tuple.Create(new DateTime(2025, 05, 01), 0.28)
                }
            },
            {
                Commodity.Transport, new List<Tuple<DateTime, double>>
                {
                    Tuple.Create(new DateTime(2022, 12, 01), 0.06)
                }
            }
        };



        /// <summary>
        /// Gets the tax rate that is active for a specific point in time (in UTC).
        /// A custom tax rate is seen as the currently active rate for a period from its starting timestamp until a new custom rate is set.
        /// If there is no custom tax rate for the specified date, use the standard tax rate.
        /// </summary>
        public double GetTaxRateForDateTime(Commodity commodity, DateTime date)
        {

            if (_customRates[commodity].OrderByDescending(t => t.Item1).FirstOrDefault(x => x.Item1 <= date) != null)
                return _customRates[commodity].OrderByDescending(t => t.Item1).FirstOrDefault(x => x.Item1 <= date).Item2;

            return GetStandardTaxRate(commodity);
        }

        /// <summary>
        /// Gets the tax rate that is active for the current point in time.
        /// A custom tax rate is seen as the currently active rate for a period from its starting timestamp until a new custom rate is set.
        /// If there is no custom tax currently active, use the standard tax rate.
        /// </summary>
        public double GetCurrentTaxRate(Commodity commodity)
        {
            return GetTaxRateForDateTime(commodity, DateTime.UtcNow);
        }

    }
}


