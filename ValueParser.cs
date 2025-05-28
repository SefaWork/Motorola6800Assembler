using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public class ValueParser {

        public static int ParseValue(string valueString, Dictionary<string, int>? labels) {
            bool result;
            int value;
            if (valueString.StartsWith("$")) {
                valueString = valueString.Remove(0, 1);
                // Interpret value as a hexidecimal value.
                result = int.TryParse(valueString, System.Globalization.NumberStyles.HexNumber, null, out value);
            } else {
                // Interpret value as a decimal value.
                result = int.TryParse(valueString, System.Globalization.NumberStyles.Integer, null, out value);
            }

            if(!result) {
                if(labels != null) {
                    if(!labels.TryGetValue(valueString, out value)) {
                        throw new Exception($"Failed to parse value from sequence \"{valueString}\". Label not found.");
                    }
                } else {
                    throw new Exception($"Failed to parse value from sequence \"{valueString}\"");
                }
            }

            return value;
        }

    }
}
