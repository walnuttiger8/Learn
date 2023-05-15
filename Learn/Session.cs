using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn
{
    public class Session
    {
        private static Session _instance;

        public bool IsAdmin = false;

        public Session()
        {
            
        }

        public static Session Get()
        {
            if (_instance == null)
            {
                _instance = new Session();
            }
            return _instance;
        }
    }
}
