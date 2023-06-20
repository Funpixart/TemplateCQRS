namespace TemplateCQRS.Domain.Common;

public static class TimeUtilities
{
    public static List<string> DayHours { get; set; } = new()
        {
            "12:00 AM",
            "01:00 AM",
            "02:00 AM",
            "03:00 AM",
            "04:00 AM",
            "05:00 AM",
            "06:00 AM",
            "07:00 AM",
            "08:00 AM",
            "09:00 AM",
            "10:00 AM",
            "11:00 AM"
        };

    public static List<string> LateHours { get; set; } = new()
        {
            "12:00 PM",
            "01:00 PM",
            "02:00 PM",
            "03:00 PM",
            "04:00 PM",
            "05:00 PM",
            "06:00 PM",
            "07:00 PM",
            "08:00 PM",
            "09:00 PM",
            "10:00 PM",
            "11:00 PM"
        };

    public static List<string> LaborTime { get; set; } = new()
        {
            "Weekdays",
            "Weekend",
            "1 Day at week",
            "2 Days at week",
            "3 Days at week",
            "4 Days at week",
            "Everyday"
        };

    public static List<string> HorarioLaboral { get; set; } = new()
        {
            "Lunes a Viernes",
            "Sabados y Domingo",
            "Todos los días",
            "1 Día a la semana",
            "2 Días a la semana",
            "3 Días a la semana",
            "4 Días a la semana"
        };
}