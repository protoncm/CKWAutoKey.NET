using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinAutoKeyweight.NET
{
    public class DatabaseServices
    {
        private readonly CKWDatabaseEntities Entities;
        public static DatabaseServices _instance;
        public DatabaseServices()
        {
            Entities = new CKWDatabaseEntities();
        }

        public static DatabaseServices Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DatabaseServices();
                } 
                return _instance;
            }
        }

        public Table LoadConfiguration()
        {
            var dataset = Entities.Tables;
            if(dataset != null && dataset.Count() > 0)
            {
                return dataset.FirstOrDefault();
            }
            return null;
        }
        public bool SaveConfiguration(string assignedKey, string assignedActiveWindow, bool isSnapping)
        {
            try
            {
                var config = LoadConfiguration();
                if(config != null)
                {
                    config.AssignedActiveWindow = assignedActiveWindow;
                    config.AssignedKey = assignedKey;
                    config.IsSnapping = isSnapping;
                    Entities.Tables.Attach(config);
                    Entities.Entry(config).State = System.Data.Entity.EntityState.Modified;
                    Entities.SaveChanges();
                    return true;
                }
                else
                {
                    Table newConfig = new Table();
                    newConfig.Id = 1;
                    newConfig.AssignedActiveWindow = assignedActiveWindow;
                    newConfig.AssignedKey = assignedKey;
                    newConfig.IsSnapping = isSnapping;
                    Entities.Tables.Add(newConfig);
                    Entities.SaveChanges();
                    return true;
                }
                
            }
            catch (InvalidOperationException invalidOperationException) { throw invalidOperationException; }
            catch (NotSupportedException notsupportedException) { throw notsupportedException; }
        }
    }
}
