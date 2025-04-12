#region GNU Lesser General Public License
/*
This file is part of DotFuzzy.

DotFuzzy is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

DotFuzzy is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with DotFuzzy.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DotFuzzy
{
    /// <summary>
    /// Represents a collection of rules.
    /// </summary>
    public class LinguisticVariableCollection : IEnumerable<LinguisticVariable>
    {
        private List<LinguisticVariable> variables = new List<LinguisticVariable>();

        public void Add(LinguisticVariable variable)
        {
            variables.Add(variable);
        }

        public IEnumerator<LinguisticVariable> GetEnumerator()
        {
            return variables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public LinguisticVariable Find(string linguisticVariableName)
        {
            string trimmedName = linguisticVariableName.Trim();
            foreach (LinguisticVariable variable in variables)
            {
                if (variable.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase))
                {
                    return variable;
                }
            }

            throw new Exception("LinguisticVariable not found: " + linguisticVariableName);
        }
    }

}
