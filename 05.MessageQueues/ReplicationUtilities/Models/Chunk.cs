namespace ReplicationUtilities.Models
{
    public class Chunk
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public bool IsLastChunk { get; set; }
        public int Index { get; set; }
    }
}