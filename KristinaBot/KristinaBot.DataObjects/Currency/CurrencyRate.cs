

namespace KristinaBot.DataObjects.Currency
{
    public class CurrencyRate
    {
        public string CurrencyCodeL { get; set; } = string.Empty; 
        public int Units { get; set; }
        public double Amount { get; set; }
    }
}
