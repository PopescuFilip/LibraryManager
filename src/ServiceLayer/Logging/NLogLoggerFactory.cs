using NLog;

namespace ServiceLayer.Logging;

public interface INLogLoggerFactory
{
    ILogger GetLogger<T>();
}

public class NLogLoggerFactory : INLogLoggerFactory
{
    public ILogger GetLogger<T>() => LogManager.GetLogger(typeof(T).ToString());
}