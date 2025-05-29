using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public class ValueParser {
        public static CodeDomProvider CODEDOMPROVIDER = CodeDomProvider.CreateProvider("C#");

        public static void ParseValue(string valueString, LineProcess process) {
            bool result;
            int value;

            char firstChar = valueString[0];

            if (firstChar == '$') {
                valueString = valueString.Remove(0, 1);
                // Interpret value as a hexidecimal value.
                result = int.TryParse(valueString, System.Globalization.NumberStyles.HexNumber, null, out value);
            } else if (firstChar == '%') {
                valueString = valueString.Remove(0, 1);
                // Interpret value as a binary value.
                result = int.TryParse(valueString, System.Globalization.NumberStyles.BinaryNumber, null, out value);
            } else {
                // Interpret value as a decimal value.
                result = int.TryParse(valueString, System.Globalization.NumberStyles.Integer, null, out value);
            }

            if (!result) {
                if (CODEDOMPROVIDER.IsValidIdentifier(valueString)) {
                    // Could be a label.
                    process.variable = valueString;
                } else {
                    // Does not follow label naming conventions, throw an exception.
                    throw new Exception($"Invalid value: {valueString}");
                }
            } else {
                process.value = value;
            }
        }
    }
}
