using System.Collections.Generic;

namespace Thingy.CalculatorCore.Constants
{
    internal class Mathematical : IConstantProvider
    {
        public string Category
        {
            get { return nameof(Mathematical); }
        }

        public IEnumerable<Constant> Constants
        {
            get
            {
                yield return new Constant("The number e", "&e", 2.7182818284590452353602874713526624977572470937000d);
                yield return new Constant("The number log[2](e)", "&Log2E", 1.4426950408889634073599246810018921374266459541530d);
                yield return new Constant("The number log[10](e)", "&Log10E", 0.43429448190325182765112891891660508229439700580366d);
                yield return new Constant("The number log[e](2)", "&Ln2", 0.69314718055994530941723212145817656807550013436026d);
                yield return new Constant("The number log[e](10)", "&Ln10", 2.3025850929940456840179914546843642076011014886288d);
                yield return new Constant("The number log[e](pi)", "&LnPi", 1.1447298858494001741434273513530587116472948129153d);
                yield return new Constant("The number log[e](2*pi)/2", "&Ln2PiOver2", 0.91893853320467274178032973640561763986139747363780d);
                yield return new Constant("The number 1/e", "&InvE", 0.36787944117144232159552377016146086744581113103176d);
                yield return new Constant("The number sqrt(e)", "&SqrtE", 1.6487212707001281468486507878141635716537761007101d);
                yield return new Constant("The number sqrt(2)", "&Sqrt2", 1.4142135623730950488016887242096980785696718753769d);
                yield return new Constant("The number sqrt(1/2) = 1/sqrt(2) = sqrt(2)/2", "&Sqrt1Over2", 0.70710678118654752440084436210484903928483593768845d);
                yield return new Constant("The number sqrt(3)/2", "&HalfSqrt3", 0.86602540378443864676372317075293618347140262690520d);
                yield return new Constant("The number pi", "&Pi", 3.1415926535897932384626433832795028841971693993751d);
                yield return new Constant("The number 2*pi", "&Pi2", 6.2831853071795864769252867665590057683943387987502d);
                yield return new Constant("The number 1/pi", "&OneOverPi", 0.31830988618379067153776752674502872406891929148091d);
                yield return new Constant("The number pi/2", "&PiOver2", 1.5707963267948966192313216916397514420985846996876d);
                yield return new Constant("The number pi/4", "&PiOver4", 0.78539816339744830961566084581987572104929234984378d);
                yield return new Constant("The number sqrt(pi)", "&SqrtPi", 1.7724538509055160272981674833411451827975494561224d);
                yield return new Constant("The number sqrt(2pi)", "&Sqrt2Pi", 2.5066282746310005024157652848110452530069867406099d);
                yield return new Constant("The number sqrt(2*pi*e)", "&Sqrt2PiE", 4.1327313541224929384693918842998526494455219169913d);
                yield return new Constant("The number log(sqrt(2*pi))", "&LogSqrt2Pi", 0.91893853320467274178032973640561763986139747363778);
                yield return new Constant("The number log(sqrt(2*pi*e))", "&LogSqrt2PiE", 1.4189385332046727417803297364056176398613974736378d);
                yield return new Constant("The number log(2 * sqrt(e / pi))", "&LogTwoSqrtEOverPi", 0.6207822376352452223455184457816472122518527279025978);
                yield return new Constant("The number 1/pi", "&InvPi", 0.31830988618379067153776752674502872406891929148091d);
                yield return new Constant("The number 2/pi", "&TwoInvPi", 0.63661977236758134307553505349005744813783858296182d);
                yield return new Constant("The number 1/sqrt(pi)", "&InvSqrtPi", 0.56418958354775628694807945156077258584405062932899d);
                yield return new Constant("The number 1/sqrt(2pi)", "&InvSqrt2Pi", 0.39894228040143267793994605993438186847585863116492d);
                yield return new Constant("The number 2/sqrt(pi)", "&TwoInvSqrtPi", 1.1283791670955125738961589031215451716881012586580d);
                yield return new Constant("The number 2 * sqrt(e / pi)", "&TwoSqrtEOverPi", 1.8603827342052657173362492472666631120594218414085755);
                yield return new Constant("The number (pi)/180 - factor to convert from Degree (deg) to Radians (rad).", "&Degree", 0.017453292519943295769236907684886127134428718885417d);
                yield return new Constant("The number (pi)/200 - factor to convert from NewGrad (grad) to Radians (rad).", "&Grad", 0.015707963267948966192313216916397514420985846996876d);
                yield return new Constant("The number ln(10)/20 - factor to convert from Power Decibel (dB) to Neper (Np). Use this version when the Decibel represent a power gain but the compared values are not powers (e.g. amplitude, current, voltage).", "&PowerDecibel", 0.11512925464970228420089957273421821038005507443144d);
                yield return new Constant("The number ln(10)/10 - factor to convert from Neutral Decibel (dB) to Neper (Np). Use this version when either both or neither of the Decibel and the compared values represent powers.", "&NeutralDecibel", 0.23025850929940456840179914546843642076011014886288d);
                yield return new Constant("The Catalan constant", "&Catalan", 0.9159655941772190150546035149323841107741493742816721342664981196217630197762547694794d);
                yield return new Constant("The Euler-Mascheroni constant", "&EulerMascheroni", 0.5772156649015328606065120900824024310421593359399235988057672348849d);
                yield return new Constant("The number (1+sqrt(5))/2, also known as the golden ratio", "&GoldenRatio", 1.6180339887498948482045868343656381177203091798057628621354486227052604628189024497072d);
                yield return new Constant("The Glaisher constant", "&Glaisher", 1.2824271291006226368753425688697917277676889273250011920637400217404063088588264611297d);
                yield return new Constant("The Khinchin constant", "&Khinchin", 2.6854520010653064453097148354817956938203822939944629530511523455572188595371520028011d);
            }
        }
    }
}
