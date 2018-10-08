using System;

namespace MassTransit.Messages
{
    public class TestBaseMessage
    {
        public string Name { get; set; }

        public DateTime Time { get; set; }

        public string Message { get; set; }
    }

    public class TestCustomMessage
    {
        public string Name { get; set; }

        public DateTime Time { get; set; }

        public int Age { get; set; }

        public string Message { get; set; }
    }
}
