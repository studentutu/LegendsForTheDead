// using System.Collections;
// using System.Collections.Generic;
namespace Services.DependencyInjection
{
    public interface IDepBase
    {
        SettingsForType getMyTypeSettings();
        void setMyTypeSettings(SettingsForType mynewSetings);

        void RemoveFromLocalContextAndDestroy();
        void RemoveFromLocalContext();
        void AddToLocalContext();

        
    }
}
