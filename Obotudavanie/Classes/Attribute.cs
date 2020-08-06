namespace Obotudavanie.Classes
{
    public class Attribute<T> : IAttribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string EdIzm { get; set; }
        public T Value { get; set; }
        public T DefValue { get; set; }
        

    }
}
