namespace NexHUD.Apis.Spansh
{
    public class SpanshBodiesResult
    {
        public int count;
        public int from;
        public SpanshBody[] results;
        public string search_reference;
    }

    public class SpanshSystemsResult
    {
        public int count;
        public int from;
        public SpanshSystem[] results;
        public string search_reference;
    }
}
