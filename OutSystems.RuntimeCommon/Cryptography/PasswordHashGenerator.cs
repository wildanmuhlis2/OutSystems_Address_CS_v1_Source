/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using OutSystems.RuntimeCommon.Cryptography.Implementations;
using OutSystems.RuntimeCommon.Cryptography.Interfaces;

namespace OutSystems.RuntimeCommon.Cryptography
{
    internal abstract class PasswordHashGenerator : IPasswordHashGenerator
    {
        public int Iterations { get; set; }
        public int Lenght { get; set; }

        protected PasswordHashGenerator()
        {
            Iterations = 943;
            Lenght = 512;
        }

        public abstract byte[] GetBytes(byte[] text, byte[] salt, int iterations);

        public byte[] GetBytes(byte[] text, byte[] salt)
        {
            return GetBytes(text, salt, Iterations);
        }
    }
}