using SagErpBlazor.Data;

namespace SagErpBlazor.Services
{
    public class ActiveUserSingleton
    {
        private static GLUser _activeUser;
        private static readonly object _lock = new object();

        public static GLUser Instance
        {
            get
            {
                if (_activeUser == null)
                {
                    lock (_lock)
                    {
                        if (_activeUser == null)
                        {
                            // Fetch and initialize the _activeUser here
                        }
                    }
                }
                return _activeUser;
            }
        }
        public static void Initialize(GLUser user)
        {
            _activeUser = user;
        }
        public static void Logout()
        {
            _activeUser = null;
        }
    }

}
