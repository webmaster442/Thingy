using System;

namespace Thingy.Db
{
    internal class DataBaseInitializer
    {
        internal static void Init(IDataBase dataBase)
        {
            dataBase.MediaLibary.AddRadioStations(initialData.RadioStations.Collection);
        }
    }
}