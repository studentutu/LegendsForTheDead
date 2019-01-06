

namespace Services.DependencyInjection
{
    public interface IDependencyContext
    {
        void RegisterContext(IDependencyContext itemToAdd);
        void RemoveContext(IDependencyContext itemToAdd);

        DependencyContextTypes GetMyContextType();


        void AddToGivenContext( BaseMonoObject baseMonoObject , SettingsForType settings);
        BaseMonoObject GetFromLocalContext ( SettingsForType settings);
        void RemoveFromContext(SettingsForType settings);
    }
}
