namespace CSharp.Contracts
{
    public class Order
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public Customer Customer { get; set; }
    }
}