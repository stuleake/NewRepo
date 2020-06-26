using System.Text;

namespace Api.Globals.Core.Helpers
{
    /// <summary>
    /// Class to do encryption
    /// </summary>
    public class CeaserCipher
    {
        private const int AlphabetCount = 26;
        private const int DigitCount = 10;

        private readonly int shiftKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="CeaserCipher"/> class
        /// </summary>
        /// <param name="shiftKey"> Salt being passed using dependency injection</param>
        public CeaserCipher(int shiftKey)
        {
            this.shiftKey = shiftKey;
        }

        /// <summary>
        /// Function to decrypt
        /// </summary>
        /// <param name="cipherText">The plaintext to be ciphered</param>
        /// <returns>A string representing the ciphered text</returns>
        public string Decrypt(string cipherText)
        {
            return Cipher(cipherText, false);
        }

        /// <summary>
        /// Function to encrypt
        /// </summary>
        /// <param name="plainText">The plaintext to be ciphered</param>
        /// <returns>A string representing the ciphered text</returns>
        public string Encrypt(string plainText)
        {
            return Cipher(plainText, true);
        }

        /// <summary>
        /// Function to encrypt email and datetime
        /// </summary>
        /// <param name="plainText">The plaintext to be ciphered</param>
        /// <returns>A string representing the ciphered text</returns>
        public string EncryptEmail(string plainText)
        {
            var cipher = Cipher(plainText, true);
            cipher = cipher.Replace("@", "1qtqrq12", System.StringComparison.InvariantCultureIgnoreCase);
            cipher = cipher.Replace(".", "450ptqrt908", System.StringComparison.InvariantCultureIgnoreCase);
            cipher = cipher.Replace(":", "096tqrtqb65", System.StringComparison.InvariantCultureIgnoreCase);
            cipher = cipher.Replace("-", "67rttqrttq7yt", System.StringComparison.InvariantCultureIgnoreCase);
            cipher = cipher.Replace(" ", "908ttqsp5", System.StringComparison.InvariantCultureIgnoreCase);
            return cipher;
        }

        /// <summary>
        /// Perform ceaser cipher
        /// </summary>
        /// <param name="text">the source text</param>
        /// <param name="isPlainText">Bool to indicate whter to encrypt/decrypt the specified text</param>
        /// <returns>string</returns>
        private string Cipher(string text, bool isPlainText)
        {
            // Retunr empty if null
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var strbldr = new StringBuilder();

            foreach (var chr in text)
            {
                // If letter shift (Base - 26)
                if (char.IsLetter(chr))
                {
                    strbldr.Append(Shifter(chr, (isPlainText ? 1 : -1) * shiftKey, char.IsUpper(chr) ? 'A' : 'a', AlphabetCount));
                }

                // Else if digit shift (Base - 10)
                else if (char.IsDigit(chr))
                {
                    strbldr.Append(Shifter(chr, (isPlainText ? 1 : -1) * shiftKey, '0', DigitCount));
                }

                // Else Shift nothing
                else
                {
                    strbldr.Append(chr);
                }
            }

            // Return the ciphened string
            return strbldr.ToString();
        }

        /// <summary>
        /// Shift the character
        /// </summary>
        /// <param name="chr">The source character</param>
        /// <param name="shift">The shift to be performed on the charater</param>
        /// <param name="lowerBound">The lower bound of for the base type</param>
        /// <param name="bound">The upper bound for the base type</param>
        /// <returns>char</returns>
        private static char Shifter(char chr, int shift, char lowerBound, int bound)
        {
            var tempShift = shift % bound;

            if (tempShift < 0)
            {
                tempShift = bound + tempShift;
            }

            var res = chr + tempShift;
            if (res < lowerBound)
            {
                res += bound;
            }
            else if (res > (lowerBound + bound - 1))
            {
                res -= bound;
            }

            return (char)res;
        }
    }
}