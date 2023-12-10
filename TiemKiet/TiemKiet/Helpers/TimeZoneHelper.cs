namespace TiemKiet.Helpers
{
    public class TimeZoneHelper
    {
        private readonly TimeZoneInfo _vnTimeZone;

        public TimeZoneHelper()
        {
            string vnTimeZoneId = "SE Asia Standard Time";

            _vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneId);

            if (_vnTimeZone == null)
            {
                throw new TimeZoneNotFoundException($"Time zone with ID '{vnTimeZoneId}' not found.");
            }
        }

        public TimeZoneHelper(TimeZoneInfo vnTimeZone)
        {
            _vnTimeZone = vnTimeZone ?? throw new ArgumentNullException(nameof(vnTimeZone));
        }

        public string ConvertUtcToVnTimeString(DateTime utcTime)
        {
            DateTime vnTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _vnTimeZone);

            return vnTime.ToString("HH:mm dd/MM/yyyy");
        }
    }
}
