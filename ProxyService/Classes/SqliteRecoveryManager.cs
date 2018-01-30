using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProxyService.Classes;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    public class SqliteRecoveryManager : IRecoveryManager
    {
        private TaskItemContext _taskContext;
        private delegate bool SelectionHandler(string token, bool value);
        private event SelectionHandler StructureCreatedHandler;

        public SqliteRecoveryManager(TaskItemContext context)
        {
            _taskContext = context;
        }

        public void Recover(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Operates setting recovery points
        /// </summary>
        /// <param name="selector">Determines last successful major process</param>
        /// <param name="token">Request identificator</param>
        /// <param name="value">Flag value</param>
        /// <returns>true if success, otherwise false</returns>
        
        public bool SetProperty(PropertySelector selector, string token, bool value)
        {
            switch (selector)
            {
                case PropertySelector.FolderStructureCreated:
                    StructureCreatedHandler += (tkn, val) =>
                    {
                        try
                        {
                            var task = _taskContext.Tasks.Where(x => x.Token.Equals(tkn)).FirstOrDefault();
                            if (task == null)
                                return false;
                            task.FolderStructureCreated = value;
                            int i = _taskContext.SaveChanges();
                            return i == 1;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    };
                    return StructureCreatedHandler(token, value);
                default:
                    return false;
            }
        }
    }
}
