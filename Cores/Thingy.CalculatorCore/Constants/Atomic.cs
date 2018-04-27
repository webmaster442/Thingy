using System.Collections.Generic;

namespace Thingy.CalculatorCore.Constants
{
    internal class Atomic : IConstantProvider
    {
        public string Category
        {
            get { return nameof(Atomic); }
        }

        public IEnumerable<Constant> Constants
        {
            get
            {
                yield return new Constant("Fine Structure Constant: alpha = e^2/4*Pi*e_0*h_bar*c_0 [1] (2007 CODATA)", "FineStructureConstant", 7.2973525376e-3);
                yield return new Constant("Rydberg Constant: R_infty = alpha^2*m_e*c_0/2*h [m^-1] (2007 CODATA)", "RydbergConstant", 10973731.568528);
                yield return new Constant("Bor Radius: a_0 = alpha/4*Pi*R_infty [m] (2007 CODATA)", "BohrRadius", 0.52917720859e-10);
                yield return new Constant("Hartree Energy: E_h = 2*R_infty*h*c_0 [J] (2007 CODATA)", "HartreeEnergy", 4.35974394e-18);
                yield return new Constant("Quantum of Circulation: h/2*m_e [m^2 s^-1] (2007 CODATA)", "QuantumOfCirculation", 3.6369475199e-4);
                yield return new Constant("Fermi Coupling Constant: G_F/(h_bar*c_0)^3 [GeV^-2] (2007 CODATA)", "FermiCouplingConstant", 1.16637e-5);
                yield return new Constant("Weak Mixin Angle: sin^2(theta_W) [1] (2007 CODATA)", "WeakMixingAngle", 0.22256);
                yield return new Constant("Electron Mass: [kg] (2007 CODATA)", "ElectronMass", 9.10938215e-31);
                yield return new Constant("Electron Mass Energy Equivalent: [J] (2007 CODATA)", "ElectronMassEnergyEquivalent", 8.18710438e-14);
                yield return new Constant("Electron Molar Mass: [kg mol^-1] (2007 CODATA)", "ElectronMolarMass", 5.4857990943e-7);
                yield return new Constant("Electron Compton Wavelength: [m] (2007 CODATA)", "ComptonWavelength", 2.4263102175e-12);
                yield return new Constant("Classical Electron Radius: [m] (2007 CODATA)", "ClassicalElectronRadius", 2.8179402894e-15);
                yield return new Constant("Tomson Cross Section: [m^2] (2002 CODATA)", "ThomsonCrossSection", 0.6652458558e-28);
                yield return new Constant("Electron Magnetic Moment: [J T^-1] (2007 CODATA)", "ElectronMagneticMoment", -928.476377e-26);
                yield return new Constant("Electon G-Factor: [1] (2007 CODATA)", "ElectronGFactor", -2.0023193043622);
                yield return new Constant("Muon Mass: [kg] (2007 CODATA)", "MuonMass", 1.88353130e-28);
                yield return new Constant("Muon Mass Energy Equivalent: [J] (2007 CODATA)", "MuonMassEnegryEquivalent", 1.692833511e-11);
                yield return new Constant("Muon Molar Mass: [kg mol^-1] (2007 CODATA)", "MuonMolarMass", 0.1134289256e-3);
                yield return new Constant("Muon Compton Wavelength: [m] (2007 CODATA)", "MuonComptonWavelength", 11.73444104e-15);
                yield return new Constant("Muon Magnetic Moment: [J T^-1] (2007 CODATA)", "MuonMagneticMoment", -4.49044786e-26);
                yield return new Constant("Muon G-Factor: [1] (2007 CODATA)", "MuonGFactor", -2.0023318414);
                yield return new Constant("Tau Mass: [kg] (2007 CODATA)", "TauMass", 3.16777e-27);
                yield return new Constant("Tau Mass Energy Equivalent: [J] (2007 CODATA)", "TauMassEnergyEquivalent", 2.84705e-10);
                yield return new Constant("Tau Molar Mass: [kg mol^-1] (2007 CODATA)", "TauMolarMass", 1.90768e-3);
                yield return new Constant("Tau Compton Wavelength: [m] (2007 CODATA)", "TauComptonWavelength", 0.69772e-15);
                yield return new Constant("Proton Mass: [kg] (2007 CODATA)", "ProtonMass", 1.672621637e-27);
                yield return new Constant("Proton Mass Energy Equivalent: [J] (2007 CODATA)", "ProtonMassEnergyEquivalent", 1.503277359e-10);
                yield return new Constant("Proton Molar Mass: [kg mol^-1] (2007 CODATA)", "ProtonMolarMass", 1.00727646677e-3);
                yield return new Constant("Proton Compton Wavelength: [m] (2007 CODATA)", "ProtonComptonWavelength", 1.3214098446e-15);
                yield return new Constant("Proton Magnetic Moment: [J T^-1] (2007 CODATA)", "ProtonMagneticMoment", 1.410606662e-26);
                yield return new Constant("Proton G-Factor: [1] (2007 CODATA)", "ProtonGFactor", 5.585694713);
                yield return new Constant("Proton Shielded Magnetic Moment: [J T^-1] (2007 CODATA)", "ShieldedProtonMagneticMoment", 1.410570419e-26);
                yield return new Constant("Proton Gyro-Magnetic Ratio: [s^-1 T^-1] (2007 CODATA)", "ProtonGyromagneticRatio", 2.675222099e8);
                yield return new Constant("Proton Shielded Gyro-Magnetic Ratio: [s^-1 T^-1] (2007 CODATA)", "ShieldedProtonGyromagneticRatio", 2.675153362e8);
                yield return new Constant("Neutron Mass: [kg] (2007 CODATA)", "NeutronMass", 1.674927212e-27);
                yield return new Constant("Neutron Mass Energy Equivalent: [J] (2007 CODATA)", "NeutronMassEnegryEquivalent", 1.505349506e-10);
                yield return new Constant("Neutron Molar Mass: [kg mol^-1] (2007 CODATA)", "NeutronMolarMass", 1.00866491597e-3);
                yield return new Constant("Neuron Compton Wavelength: [m] (2007 CODATA)", "NeutronComptonWavelength", 1.3195908951e-1);
                yield return new Constant("Neutron Magnetic Moment: [J T^-1] (2007 CODATA)", "NeutronMagneticMoment", -0.96623641e-26);
                yield return new Constant("Neutron G-Factor: [1] (2007 CODATA)", "NeutronGFactor", -3.82608545);
                yield return new Constant("Neutron Gyro-Magnetic Ratio: [s^-1 T^-1] (2007 CODATA)", "NeutronGyromagneticRatio", 1.83247185e8);
                yield return new Constant("Deuteron Mass: [kg] (2007 CODATA)", "DeuteronMass", 3.34358320e-27);
                yield return new Constant("Deuteron Mass Energy Equivalent: [J] (2007 CODATA)", "DeuteronMassEnegryEquivalent", 3.00506272e-10);
                yield return new Constant("Deuteron Molar Mass: [kg mol^-1] (2007 CODATA)", "DeuteronMolarMass", 2.013553212725e-3);
                yield return new Constant("Deuteron Magnetic Moment: [J T^-1] (2007 CODATA)", "DeuteronMagneticMoment", 0.433073465e-26);
                yield return new Constant("Helion Mass: [kg] (2007 CODATA)", "HelionMass", 5.00641192e-27);
                yield return new Constant("Helion Mass Energy Equivalent: [J] (2007 CODATA)", "HelionMassEnegryEquivalent", 4.49953864e-10);
                yield return new Constant("Helion Molar Mass: [kg mol^-1] (2007 CODATA)", "HelionMolarMass", 3.0149322473e-3);
            }
        }
    }
}
