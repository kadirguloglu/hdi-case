using System.Runtime.Serialization;
using System.Text.Json.Serialization;


[DataContract]
public class Result<T, K, L> : Result<T, K>
{
    public Result()
    {

    }
    public Result(bool isSuccessfull, string message) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
    }
    public Result(bool isSuccessfull, T? data, K? data2, L? data3) : base(isSuccessfull, data, data2)
    {
        IsSuccessfull = isSuccessfull;
        Data = data;
        IsAccessible = isSuccessfull && data != null;
        Data2 = data2;
        IsAccessible2 = isSuccessfull && data2 != null;
        Data3 = data3;
        IsAccessible3 = isSuccessfull && data3 != null;
    }

    [DataMember(Order = 8)]
    public L? Data3 { get; }

    [DataMember(Order = 9)]
    public Boolean IsAccessible3 { get; }
}

[DataContract]
public class Result<T, K> : Result<T>
{
    public Result()
    {

    }
    public Result(bool isSuccessfull, string message) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
    }
    public Result(bool isSuccessfull, string message, T? data) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
        Data = data;
        IsAccessible = isSuccessfull && data != null;
    }
    public Result(bool isSuccessfull, string message, K? data2) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
        Data2 = data2;
        IsAccessible2 = isSuccessfull && data2 != null;
    }
    public Result(bool isSuccessfull, string message, T? data, K? data2) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
        Data = data;
        IsAccessible = isSuccessfull && data != null;
        Data2 = data2;
        IsAccessible2 = isSuccessfull && data2 != null;
    }
    public Result(bool isSuccessfull, T? data, K? data2) : base(isSuccessfull, data)
    {
        IsSuccessfull = isSuccessfull;
        Data = data;
        IsAccessible = isSuccessfull && data != null;
        Data2 = data2;
        IsAccessible2 = isSuccessfull && data2 != null;
    }

    [DataMember(Order = 6)]
    public K? Data2 { get; protected set; }

    [DataMember(Order = 7)]
    public bool IsAccessible2 { get; protected set; }
}

[DataContract]
// [KnownType(typeof(Result<,>))]
public class Result<T> : Result
{
    public Result()
    {

    }
    public Result(bool isSuccessfull) : base(isSuccessfull)
    {
        IsSuccessfull = isSuccessfull;
    }
    public Result(bool isSuccessfull, T? data) : base(isSuccessfull)
    {
        IsSuccessfull = isSuccessfull;
        Data = data;
        IsAccessible = isSuccessfull && data != null;
    }

    [JsonConstructor]
    public Result(bool isSuccessfull, T? data, string message) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Data = data;
        IsAccessible = isSuccessfull && data != null;
        Message = message;
    }

    public Result(bool isSuccessfull, string? message) : base(isSuccessfull, message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
        if (typeof(T) == typeof(string) && message != null)
        {
            Data = (T)(object)message;
        }
    }

    [DataMember(Order = 4)]
    public T? Data { get; protected set; }
    [DataMember(Order = 5)]
    public Boolean IsAccessible { get; protected set; }
}

[DataContract]
// [KnownType(typeof(Result<>))]
public class Result
{
    public Result()
    {

    }
    public Result(bool isSuccessfull)
    {
        IsSuccessfull = isSuccessfull;
    }

    [JsonConstructor]
    public Result(bool isSuccessfull, string? message)
    {
        IsSuccessfull = isSuccessfull;
        Message = message;
    }

    [DataMember(Order = 1)]
    public Boolean IsSuccessfull { get; protected set; }
    [DataMember(Order = 2)]
    public string? Message { get; protected set; }
}