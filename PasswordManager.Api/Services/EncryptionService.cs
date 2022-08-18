namespace PasswordManager.Api.Services
{
    public class EncryptionService
    {
        private readonly Dictionary<string, byte[]> _userkeys;
       
        public EncryptionService()
        {
            _userkeys = new Dictionary<string, byte[]>();
        }


        public Dictionary<string, byte[]> GetUserKeys()
        {
            return _userkeys;
        }

        public byte[] GetUserKeysById(string userId)
        {
            return _userkeys.FirstOrDefault(o => o.Key.Equals(userId)).Value;
        }
    }
}
