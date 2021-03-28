using Services.Exceptions.BadRequest;

namespace Services.Exceptions.NotFound
{
    class SettingNotFound : BaseNotFoundException
    {
        public SettingNotFound() : base()
        {
            CustomMessage = "Setting Not Found.";
        }
    }
}
