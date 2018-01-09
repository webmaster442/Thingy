using System.Collections.Generic;

namespace Thingy.CalculatorCore.Constants
{
    internal class ElectroMagnetic : IConstantProvider
    {
        public string Category
        {
            get { return "Electro Magnetic"; }
        }

        public IEnumerable<Constant> Constants
        {
            get
            {
                yield return new Constant("Elementary Electron Charge: e = 1.602176487e-19 [C = A s] (2007 CODATA)", "&ElementaryCharge", 1.602176487e-19);
                yield return new Constant("Magnetic Flux Quantum: theta_0 = h/(2*e) [Wb = m^2 kg s^-2 A^-1] (2007 CODATA)", "&MagneticFluxQuantum", 2.067833668e-15);
                yield return new Constant("Conductance Quantum: G_0 = 2*e^2/h [S = m^-2 kg^-1 s^3 A^2] (2007 CODATA)", "&ConductanceQuantum", 7.7480917005e-5);
                yield return new Constant("Josephson Constant: K_J = 2*e/h [Hz V^-1] (2007 CODATA)", "&JosephsonConstant", 483597.891e9);
                yield return new Constant("Von Klitzing Constant: R_K = h/e^2 [Ohm = m^2 kg s^-3 A^-2] (2007 CODATA)", "&VonKlitzingConstant", 25812.807557);
                yield return new Constant("Bohr Magneton: mu_B = e*h_bar/2*m_e [J T^-1] (2007 CODATA)", "&BohrMagneton", 927.400915e-26);
                yield return new Constant("Nuclear Magneton: mu_N = e*h_bar/2*m_p [J T^-1] (2007 CODATA)", "&NuclearMagneton", 5.05078324e-27);
            }
        }
    }
}
