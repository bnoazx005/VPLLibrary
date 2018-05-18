namespace VPLLibrary.Interfaces
{
    /// <summary>
    /// interface IInputStream
    /// 
    /// The interface represents a stream on input data that
    /// interpreter can use to interact with external systems
    /// </summary>

    public interface IInputStream
    {
        /// <summary>
        /// The method returns an input parameter, which is placed
        /// at position of specified index
        /// </summary>
        /// <param name="index">An input index, should be positive</param>
        /// <returns>An input data (an integer array)</returns>

        int[] Read(int index);
    }
}
