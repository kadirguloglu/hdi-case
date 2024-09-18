public static class CronExpressions
{
    #region days
    //at 1:00 a.m.
    public static string EveryDay => "0 1 * * *";
    //at 1:00 a.m.
    public static string Every2Days => "0 1 */2 * *";
    public static string Every3Days => "0 1 */3 * *";
    public static string Every4Days => "0 1 */4 * *";
    public static string Every5Days => "0 1 */5 * *";
    public static string Every8Days => "0 1 */8 * *";
    public static string Every10Days => "0 1 */10 * *";
    #endregion

    #region hours
    public static string EveryHours => "0 * * * *";
    public static string Every2Hours => "0 */2 * * *";
    public static string Every3Hours => "0 */3 * * *";
    public static string Every4Hours => "0 */4 * * *";
    public static string Every5Hours => "0 */5 * * *";
    public static string Every8Hours => "0 */8 * * *";
    public static string Every10Hours => "0 */10 * * *";
    public static string Every12Hours => "0 */12 * * *";
    #endregion

    #region minutes
    public static string EveryMinute => "* * * * *";
    public static string Every5Minute => "*/5 * * * *";
    public static string Every8Minute => "*/8 * * * *";
    public static string Every10Minute => "*/10 * * * *";
    public static string Every15Minute => "*/15 * * * *";
    public static string Every20Minute => "*/20 * * * *";
    public static string Every30Minute => "0/30 * * * *";
    #endregion
}