namespace API_EF.Testing
{
    public class TestingRandom : IRandomTesting
    {



        public int value1 { get; set; }
        public int value2 { get; set; }

        public TestingRandom()
        {
            value1 = new Random().Next(0, int.MaxValue);
            value2 = new Random().Next(0, int.MaxValue);
        }

       
    }
}
