namespace WindowsXP.filesystem
{
    public class FileSystem
    {
        // Cluster Config
        public int clusterSize;
        public int totalClusterCount;
        public int usableClusterCount;

        // Cluster State
        public int freeClusters;
        public int usedClusters;
        public int newfilePointer;

        // Space State
        public int unoccupiedSpace;
        public int usedSpace;

        public FileSystem(int clusterSize, int totalClusterCount, int usableClusterCount, int newfilePointer)
        {
            this.clusterSize = clusterSize;
            this.totalClusterCount = totalClusterCount;
            this.usableClusterCount = usableClusterCount;

            this.freeClusters = usableClusterCount;
            this.usedClusters = 0;
            this.newfilePointer = newfilePointer;

            this.unoccupiedSpace = freeClusters * 8;
            this.usedSpace = usedClusters * 8;
        }
    }
}