using System.Collections.Generic;

namespace Thingy.CalculatorCore.Constants
{
    internal class Universal : IConstantProvider
    {
        public string Category
        {
            get { return nameof(Universal); }
        }

        public IEnumerable<Constant> Constants
        {
            get
            {
                yield return new Constant("Speed of Light in Vacuum: c_0 = 2.99792458e8 [m s^-1] (defined, exact; 2007 CODATA)", "&SpeedOfLight", 2.99792458e8);
                yield return new Constant("Magnetic Permeability in Vacuum: mu_0 = 4*Pi * 10^-7 [N A^-2 = kg m A^-2 s^-2] (defined, exact; 2007 CODATA)", "&MagneticPermeability", 1.2566370614359172953850573533118011536788677597500e-6);
                yield return new Constant("Electric Permittivity in Vacuum: epsilon_0 = 1/(mu_0*c_0^2) [F m^-1 = A^2 s^4 kg^-1 m^-3] (defined, exact; 2007 CODATA)", "&ElectricPermittivity", 8.8541878171937079244693661186959426889222899381429e-12);
                yield return new Constant("Characteristic Impedance of Vacuum: Z_0 = mu_0*c_0 [Ohm = m^2 kg s^-3 A^-2] (defined, exact; 2007 CODATA)", "&CharacteristicImpedanceVacuum", 376.73031346177065546819840042031930826862350835242);
                yield return new Constant("Newtonian Constant of Gravitation: G = 6.67429e-11 [m^3 kg^-1 s^-2] (2007 CODATA)", "&GravitationalConstant", 6.67429e-11);
                yield return new Constant("Planck's constant: h = 6.62606896e-34 [J s = m^2 kg s^-1] (2007 CODATA)", "&PlancksConstant", 6.62606896e-34);
                yield return new Constant("Reduced Planck's constant: h_bar = h / (2*Pi) [J s = m^2 kg s^-1] (2007 CODATA)", "&DiracsConstant", 1.054571629e-34);
                yield return new Constant("Planck mass: m_p = (h_bar*c_0/G)^(1/2) [kg] (2007 CODATA)", "&PlancksMass", 2.17644e-8);
                yield return new Constant("Planck temperature: T_p = (h_bar*c_0^5/G)^(1/2)/k [K] (2007 CODATA)", "&PlancksTemperature", 1.416786e32);
                yield return new Constant("Planck length: l_p = h_bar/(m_p*c_0) [m] (2007 CODATA)", "&PlancksLength", 1.616253e-35);
                yield return new Constant("Planck time: t_p = l_p/c_0 [s] (2007 CODATA)", "&PlancksTime", 5.39124e-44);
            }
        }
    }
}
