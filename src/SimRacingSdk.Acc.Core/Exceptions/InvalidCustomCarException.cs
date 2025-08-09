namespace SimRacingSdk.Acc.Core.Exceptions;

public class InvalidCustomCarException : Exception
{
    public InvalidCustomCarException()
        : base("Unexpected error processing custom car JSON file") { }

    public InvalidCustomCarException(string filePath, Exception innerException)
        : base($"Unexpected error processing custom car file: {filePath}.", innerException) { }
}