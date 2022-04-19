﻿namespace Quiz_Generator
{
    public interface IEncryption
    {
        string EncryptString(string key, string plainText);

        string DecryptString(string key, string cipherText);
    }
}
